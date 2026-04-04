import { PublishedVokisStore } from "$lib/ts/stores/published-vokis-store.svelte";
import type { PublishedVokiViewState, PublishedVokiBriefInfo } from "$lib/ts/voki";
import type { VokiType } from "$lib/ts/voki-type";
import { toast } from "svelte-sonner";
import { SvelteSet } from "svelte/reactivity";
import type { VokiIdToBriefRatingData, BriefRatingData } from "./types";
import { type VokiItemViewOkStateProps, VokiItemViewUtils, type VokiItemViewState } from "$lib/components/voki_item/_c_voki_item/voki-item";



export class RatedVokisAlbumPageState {
    allLoadedVokis: PublishedVokiViewState[] = $state([]);

    filterAndSort: {
        chosenVokiTypes: SvelteSet<VokiType>,
        currentSortOption: "From A to Z" | "From Z to A" | "Newest Rated" | "Oldest Rated" | "Highest Rated" | "Lowest Rated"
    } = $state({
        chosenVokiTypes: new SvelteSet<VokiType>(),
        currentSortOption: "Newest Rated"
    });

    readonly allSortOptions = [
        "From A to Z",
        "From Z to A",
        "Newest Rated",
        "Oldest Rated",
        "Highest Rated",
        "Lowest Rated"
    ] as const;

    private ids: string[];
    private ratingMap: VokiIdToBriefRatingData;

    readonly #onMoreBtnClick: (e: MouseEvent, voki: PublishedVokiBriefInfo) => void;

    constructor(
        map: VokiIdToBriefRatingData,
        openContextMenu: (e: MouseEvent, voki: PublishedVokiBriefInfo) => void
    ) {
        this.ratingMap = map;
        this.ids = Object.keys(map);
        this.#onMoreBtnClick = openContextMenu;

        this.loadVokis();
    }

    loadVokis() {
        this.allLoadedVokis = this.ids.map((id) => PublishedVokisStore.Get(id));
    }

    private mapToView(voki: PublishedVokiBriefInfo): VokiItemViewOkStateProps {
        return VokiItemViewUtils.briefInfoToVokiItemOkStateProps(
            voki,
            `/catalog/${voki.id}`,
            (e) => this.#onMoreBtnClick(e, voki)
        );
    }
    sortedAndFilteredVokis: () => VokiItemViewStateWithVokiIdAndRating[] = $derived(() => {
        const okItems: PublishedVokiBriefInfo[] = [];
        const nonOkItems: VokiItemViewStateWithVokiIdAndRating[] = [];

        for (const item of this.allLoadedVokis) {
            if (item.state === "ok") { okItems.push(item.data); }
            else if (item.state === "errs") {
                nonOkItems.push({
                    name: "errs",
                    data: { errs: item.errs, vokiId: item.vokiId },
                    rating: this.ratingMap[item.vokiId],
                    vokiId: item.vokiId
                });
            }
            else if (item.state === "loading") {
                nonOkItems.push({
                    name: "loading",
                    rating: this.ratingMap[item.vokiId],
                    vokiId: item.vokiId
                });
            }
        }

        let filtered = okItems;
        if (this.filterAndSort.chosenVokiTypes.size > 0) {
            filtered = filtered.filter((v) =>
                this.filterAndSort.chosenVokiTypes.has(v.type)
            );
        }

        const sort = this.filterAndSort.currentSortOption;

        filtered.sort((a, b) => {
            const ra = this.ratingMap[a.id];
            const rb = this.ratingMap[b.id];

            switch (sort) {
                case "From A to Z":
                    return a.name.localeCompare(b.name);

                case "From Z to A":
                    return b.name.localeCompare(a.name);

                case "Newest Rated":
                    return rb.dateTime.getTime() - ra.dateTime.getTime();

                case "Oldest Rated":
                    return ra.dateTime.getTime() - rb.dateTime.getTime();

                case "Highest Rated":
                    return rb.value - ra.value;

                case "Lowest Rated":
                    return ra.value - rb.value;

                default:
                    return 0;
            }
        });

        const okConverted: VokiItemViewStateWithVokiIdAndRating[] = filtered.map((v) => ({
            name: "ok",
            data: this.mapToView(v),
            rating: this.ratingMap[v.id],
            vokiId: v.id
        }));

        return [...okConverted, ...nonOkItems];
    });

    chooseSortOption(opt: string) {
        if (this.allSortOptions.includes(opt as any)) {
            this.filterAndSort.currentSortOption = opt as any;
        } else {
            toast.error("Unknown sort option");
        }
    }

    toggleTypeFilter(type: VokiType) {
        if (this.filterAndSort.chosenVokiTypes.has(type)) {
            this.filterAndSort.chosenVokiTypes.delete(type);
        } else {
            this.filterAndSort.chosenVokiTypes.add(type);
        }
    }

    isInitialListEmpty(): boolean {
        return this.allLoadedVokis.length === 0;
    }
}
type VokiItemViewStateWithVokiIdAndRating = VokiItemViewState & { vokiId: string, rating: BriefRatingData };
