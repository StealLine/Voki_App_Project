import { ApiAlbums } from "$lib/ts/backend-communication/backend-services";
import { type ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch, params }) => {
    return {
        response: await ApiAlbums.serverFetchJsonResponse<{
            id: string;
            name: string;
            icon: string;
            mainColor: string;
            secondaryColor: string;
            vokiIds: string[];
        }>(
            fetch, `/albums/${params.albumId}`, { method: 'GET' }
        ),
        albumId: params.albumId,

    };
};

