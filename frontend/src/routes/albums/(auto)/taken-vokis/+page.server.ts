import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "./$types";
import type { VokiIdToBriefVokiTakenData } from "./taken-vokis-album-page-state.svelte";


export const load: PageServerLoad = async ({ fetch }) => {
    return {
        response: await ApiVokisCatalog.serverFetchJsonResponse<{ vokis: VokiIdToBriefVokiTakenData }>(
            fetch, `/taken-vokis`, { method: 'GET' }
        )
    };
};
