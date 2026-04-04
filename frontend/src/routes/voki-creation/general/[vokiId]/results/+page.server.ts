import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { ResultOverViewData } from "./_c_results_page/types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<{
        results: ResultOverViewData[]
        maxVokiResultsCount: number
    }>(
        fetch, `/vokis/${params.vokiId}/results/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}

