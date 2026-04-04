import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import { VokiCreationMainPageState } from "../../../voki-creation-main-page-state.svelte";
import type { GeneralVokiInteractionSettings } from "./types";

export class GeneralVokiCreationMainPageState extends VokiCreationMainPageState {
    public savedInteractionSettings: GeneralVokiInteractionSettings;

    constructor(
        name: string,
        cover: string,
        tags: string[],
        details: VokiDetails,
        interactionSettings: GeneralVokiInteractionSettings,
    ) {
        super(name, cover, tags, details);
        this.savedInteractionSettings = interactionSettings;
    }
    public isInteractionSettingsEditing = $state<boolean>(false);

    get hasAnyUnsavedChanges(): boolean {
        return this.isNameEditing
            || this.isDetailsEditing
            || this.isInteractionSettingsEditing;
    }

}