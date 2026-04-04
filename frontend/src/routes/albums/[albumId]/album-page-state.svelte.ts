import { type VokiItemViewOkStateProps, VokiItemViewUtils, type VokiItemViewState } from "$lib/components/voki_item/_c_voki_item/voki-item";
import { PublishedVokisStore } from "$lib/ts/stores/published-vokis-store.svelte";
import type { PublishedVokiBriefInfo, PublishedVokiViewState } from "$lib/ts/voki";
import type { VokiType } from "$lib/ts/voki-type";
import { toast } from "svelte-sonner";
import { SvelteSet } from "svelte/reactivity";

export class AlbumPageState {
    allLoadedVokis: PublishedVokiViewState[] = $state([]);
    filterAndSort = $state({
        chosenVokiTypes: new SvelteSet<VokiType>(),
        currentSortOption: "Newest" as "From A to Z" | "From Z to A" | "Newest" | "Oldest"
    });

    readonly allSortOptions = ["From A to Z", "From Z to A", "Newest", "Oldest"] as const;
    readonly #onMoreBtnClick: (e: MouseEvent, voki: PublishedVokiBriefInfo) => void;
    private vokiIds: string[];
    constructor(vokiIds: string[], openContextMenu: (mEvent: MouseEvent, voki: PublishedVokiBriefInfo) => void) {
        this.vokiIds = vokiIds;
        this.loadVokis();
        this.#onMoreBtnClick = (e, voki) => openContextMenu(e, voki);
    }
    removeVokiFromAlbum(voki: PublishedVokiBriefInfo) {
        const id = voki.id;
        this.vokiIds = this.vokiIds.filter((x) => x !== id);

        this.allLoadedVokis = this.allLoadedVokis.filter((x) => {
            if (x.state === "ok" && x.data.id === id) {
                return false;
            }
            if (x.state === "loading" && x.vokiId === id || x.state === "errs" && x.vokiId === id) {
                return false;
            }
            return true;
        });

    }

    loadVokis() {
        this.allLoadedVokis = this.vokiIds.map((id) => PublishedVokisStore.Get(id));
    }


    private mapToView(voki: PublishedVokiBriefInfo): VokiItemViewOkStateProps {
        return VokiItemViewUtils.briefInfoToVokiItemOkStateProps(
            voki,
            `/catalog/${voki.id}`,
            (e) => this.#onMoreBtnClick(e, voki)
        );
    }

    sortedAndFilteredVokis: () => VokiItemViewState[] = $derived(() => {
        const okItems: PublishedVokiBriefInfo[] = [];
        const nonOkItems: VokiItemViewState[] = [];

        for (const item of this.allLoadedVokis) {
            if (item.state === "ok") {
                okItems.push(item.data);
            } else if (item.state === "errs") {
                nonOkItems.push({
                    name: "errs",
                    data: { errs: item.errs, vokiId: item.vokiId }
                });
            } else {
                nonOkItems.push({ name: "loading" });
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
            switch (sort) {
                case "From A to Z":
                    return a.name.localeCompare(b.name);

                case "From Z to A":
                    return b.name.localeCompare(a.name);

                case "Newest":
                    return b.publicationDate.getTime() - a.publicationDate.getTime();

                case "Oldest":
                    return a.publicationDate.getTime() - b.publicationDate.getTime();

                default:
                    return 0;
            }
        });

        const okConverted: VokiItemViewState[] = filtered.map((v) => ({
            name: "ok",
            data: this.mapToView(v)
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
