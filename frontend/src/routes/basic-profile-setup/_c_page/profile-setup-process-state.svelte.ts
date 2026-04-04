import { ApiTags } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import { StringUtils } from "$lib/ts/utils/string-utils";
import { SvelteSet } from "svelte/reactivity";


export class ProfileSetupProcessState {

    #tagsSuggestionsState: TagsSuggestionLoadingState = $state({ name: 'loading' });

    readonly chosenLanguages = new SvelteSet<Language>();
    readonly chosenFavoriteTags = new SvelteSet<Tag>();
    readonly profilePicInputValue = $state<string>("");
    readonly displayNameInputValue = $state<string>("");
    readonly displayNameToSave;

    readonly initialUniqueName;

    readonly suggestedTagsState: () => (
        | { name: "loading" }
        | { name: "ok", tags: Iterable<Tag> }
        | { name: "errs", errs: Err[] }
    ) = $derived(() => {
        if (this.#tagsSuggestionsState.name === 'errs') {
            return { name: 'errs', errs: this.#tagsSuggestionsState.errs }
        }
        if (this.#tagsSuggestionsState.name === 'loading') {
            return { name: 'loading' }
        }
        const tags = new Set<string>();
        for (const lang of this.chosenLanguages) {
            const arr = this.#tagsSuggestionsState.suggestionsByLangs[lang];
            if (arr) arr.forEach((t) => tags.add(t));
        }
        this.#tagsSuggestionsState.defaultSuggestions.forEach((t) => tags.add(t));
        return {
            name: 'ok',
            tags
        };
    });

    constructor(userUniqueName: string, initialLangs: Language[], initialTags: Tag[], initialProfilePic: string, initialDisplayName: string) {
        this.initialUniqueName = userUniqueName;
        this.displayNameToSave = $derived(StringUtils.isNullOrWhiteSpace(this.displayNameInputValue) ? this.initialUniqueName : this.displayNameInputValue);

        initialLangs.forEach((l) => this.chosenLanguages.add(l));
        initialTags.forEach((t) => this.chosenFavoriteTags.add(t));
        this.profilePicInputValue = initialProfilePic;
        this.displayNameInputValue = initialDisplayName;
        this.#loadTagsSuggestion();
    }

    isLanguageChosen(language: Language): boolean {
        return this.chosenLanguages.has(language);
    }

    toggleLanguage(language: Language): void {
        if (this.chosenLanguages.has(language)) {
            this.chosenLanguages.delete(language);
        } else {
            this.chosenLanguages.add(language);
        }
    }
    anyLanguagesChosen() {
        return this.chosenLanguages.size > 0
    }
    chooseTag(tag: Tag): void {
        this.chosenFavoriteTags.add(tag);

    }
    removeChosenTag(tag: Tag): void {
        this.chosenFavoriteTags.delete(tag);
    }

    async #loadTagsSuggestion(): Promise<void> {
        this.#tagsSuggestionsState = { name: 'loading' };
        const response = await ApiTags.fetchJsonResponse<TagSuggestionsSuccessResponse>("/tags-popular-for-languages", { method: "GET" });
        if (response.isSuccess) {
            this.#tagsSuggestionsState = { name: 'ok', ...response.data };
        } else {
            this.#tagsSuggestionsState = { name: 'errs', errs: response.errs };
        }
    }
}

type Tag = string;
type TagSuggestionsSuccessResponse = { defaultSuggestions: Tag[], suggestionsByLangs: Record<Language, Tag[]> };
type TagsSuggestionLoadingState =
    | { name: 'loading' }
    | { name: 'ok' } & TagSuggestionsSuccessResponse
    | { name: 'errs', errs: Err[] };