import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";
import type { ViewReceivedResultsData } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<ViewReceivedResultsData>(
            fetch, `/vokis/${params.vokiId}/results/received`, {
            method: "GET",
        }
        ),
        vokiId: params.vokiId
    }
}
