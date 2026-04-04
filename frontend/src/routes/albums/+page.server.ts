import { ApiAlbums } from "$lib/ts/backend-communication/backend-services";
import { type ServerLoad } from "@sveltejs/kit";
import type { AutoAlbumsAppearance, AutoAlbumsColorsPair, VokiAlbumPreviewData } from "./types";

export const load: ServerLoad = async ({ fetch }) => {
    return ApiAlbums.serverFetchJsonResponse<{
        albums: VokiAlbumPreviewData[],
        autoAlbumsAppearance: AutoAlbumsAppearance
    }>(
        fetch, '/all-albums-preview', { method: 'GET' }
    );
};



