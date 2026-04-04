import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { PublishedVokiBriefInfo } from "$lib/ts/voki";
import { type ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
    return ApiVokisCatalog.serverFetchJsonResponse<{ vokis: PublishedVokiBriefInfo[] }>(
        fetch, '/vokis/all', { method: 'GET' }
    );
};

