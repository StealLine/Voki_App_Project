import { ApiAlbums, RJO } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import { SvelteSet } from "svelte/reactivity";

export type AlbumViewData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondaryColor: string;
    creationDate: Date;
};

type AlbumsLoadingState =
    | { name: "loading" }
    | { name: "ok"; albums: AlbumViewData[] }
    | { name: "errs"; errs: Err[] };

export class AddVokiToAlbumsDialogState {
    albumsState: AlbumsLoadingState = $state({ name: "loading" });
    albumToIsChosen: Record<string, boolean> = $state({});

    private _inflight: { vokiId: string; promise: Promise<void> } | null = null;

    private _initialChosenAlbumIds = new SvelteSet<string>();

    constructor() {

    }
    isAlbumChosenChanged(albumId: string): boolean {
        const initial = this._initialChosenAlbumIds.has(albumId);
        const now = this.albumToIsChosen[albumId];
        return now !== initial;
    }
    async setVokiAndUpdate(vokiId: string): Promise<void> {
        if (this._inflight && this._inflight.vokiId === vokiId) {
            return this._inflight.promise;
        }

        const task = (async () => {
            this.albumsState = { name: "loading" };

            const response = await ApiAlbums.fetchJsonResponse<{ albums: AlbumDataWithVokiPresence[] }>(
                `/vokis/${vokiId}/albums-data`,
                { method: "GET" }
            );

            this.handleFetchAlbumsWithVokiPresence(response);
        })();

        this._inflight = {
            vokiId,
            promise: task.finally(() => (this._inflight = null))
        };

        return this._inflight.promise;
    }



    async updateVokiPresenceInAlbums(vokiId: string) {
        this.albumsState = { name: "loading" };
        const response = await ApiAlbums.fetchJsonResponse<{ albums: AlbumDataWithVokiPresence[] }>(
            `/vokis/${vokiId}/update-presence-in-albums`,
            RJO.PATCH({ albumIdToIsChosen: this.albumToIsChosen })
        );
        this.handleFetchAlbumsWithVokiPresence(response);

    }

    handleFetchAlbumsWithVokiPresence(response: ResponseResult<{ albums: AlbumDataWithVokiPresence[]; }>) {
        this.albumToIsChosen = {};
        this._initialChosenAlbumIds.clear();
        if (response.isSuccess) {
            const albums: AlbumViewData[] = response.data.albums
                .map((a) => ({
                    id: a.id,
                    name: a.name,
                    icon: a.icon,
                    mainColor: a.mainColor,
                    secondaryColor: a.secondaryColor,
                    creationDate: a.creationDate instanceof Date
                        ? a.creationDate
                        : new Date(a.creationDate as unknown as string),
                }))
                .sort((a, b) => b.creationDate.getTime() - a.creationDate.getTime());

            for (const a of response.data.albums) {
                this.albumToIsChosen[a.id] = a.isVokiInAlbum;
                if (a.isVokiInAlbum) {
                    this._initialChosenAlbumIds.add(a.id);
                }
            }

            this.albumsState = { name: "ok", albums };
        } else {
            this.albumsState = { name: "errs", errs: response.errs };
        }
    }

}
type AlbumDataWithVokiPresence = AlbumViewData & { isVokiInAlbum: boolean };
