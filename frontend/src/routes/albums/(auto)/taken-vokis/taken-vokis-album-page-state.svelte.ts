import {
    VokiItemViewUtils,
    type VokiItemViewOkStateProps,
    type VokiItemViewState
} from "$lib/components/voki_item/_c_voki_item/voki-item";
import { PublishedVokisStore } from "$lib/ts/stores/published-vokis-store.svelte";
import type { PublishedVokiViewState, PublishedVokiBriefInfo } from "$lib/ts/voki";
import type { VokiType } from "$lib/ts/voki-type";
import { toast } from "svelte-sonner";
import { SvelteSet } from "svelte/reactivity";

export type VokiIdToBriefVokiTakenData = Record<string, BriefVokiTakenData>;

export type BriefVokiTakenData = {
    timesTaken: number;
    lastTimeTaken: Date;
};

export class TakenVokisAlbumPageState {
    allLoadedVokis: PublishedVokiViewState[] = $state([]);

    filterAndSort: {
        chosenVokiTypes: SvelteSet<VokiType>;
        currentSortOption:
        | "From A to Z"
        | "From Z to A"
        | "Most Taken"
        | "Least Taken"
        | "Recently Taken"
        | "Longest Ago Taken";
    } = $state({
        chosenVokiTypes: new SvelteSet<VokiType>(),
        currentSortOption: "Recently Taken"
    });

    readonly allSortOptions = [
        "From A to Z",
        "From Z to A",
        "Most Taken",
        "Least Taken",
        "Recently Taken",
        "Longest Ago Taken"
    ] as const;

    private ids: string[];
    private takenMap: VokiIdToBriefVokiTakenData;

    readonly #onMoreBtnClick: (e: MouseEvent, voki: PublishedVokiBriefInfo) => void;

    constructor(
        map: VokiIdToBriefVokiTakenData,
        openContextMenu: (e: MouseEvent, voki: PublishedVokiBriefInfo) => void
    ) {
        this.takenMap = map;
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

    sortedAndFilteredVokis: () => VokiItemViewStateWithVokiIdAndTakenData[] = $derived(() => {
        const okItems: PublishedVokiBriefInfo[] = [];
        const nonOkItems: VokiItemViewStateWithVokiIdAndTakenData[] = [];

        for (const item of this.allLoadedVokis) {
            if (item.state === "ok") {
                okItems.push(item.data);
            } else if (item.state === "errs") {
                nonOkItems.push({
                    name: "errs",
                    data: { errs: item.errs, vokiId: item.vokiId },
                    taken: this.takenMap[item.vokiId],
                    vokiId: item.vokiId
                });
            } else if (item.state === "loading") {
                nonOkItems.push({
                    name: "loading",
                    taken: this.takenMap[item.vokiId],
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
            const ta = this.takenMap[a.id];
            const tb = this.takenMap[b.id];

            switch (sort) {
                case "From A to Z":
                    return a.name.localeCompare(b.name);

                case "From Z to A":
                    return b.name.localeCompare(a.name);

                case "Most Taken":
                    return tb.timesTaken - ta.timesTaken;

                case "Least Taken":
                    return ta.timesTaken - tb.timesTaken;

                case "Recently Taken":
                    return (
                        tb.lastTimeTaken.getTime() - ta.lastTimeTaken.getTime()
                    );

                case "Longest Ago Taken":
                    return (
                        ta.lastTimeTaken.getTime() - tb.lastTimeTaken.getTime()
                    );

                default:
                    return 0;
            }
        });

        const okConverted: VokiItemViewStateWithVokiIdAndTakenData[] = filtered.map(
            (v) => ({
                name: "ok",
                data: this.mapToView(v),
                taken: this.takenMap[v.id],
                vokiId: v.id
            })
        );

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

type VokiItemViewStateWithVokiIdAndTakenData = VokiItemViewState & {
    vokiId: string;
    taken: BriefVokiTakenData;
};
