import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiSingleResultViewData } from "./types";

export const load: ServerLoad = async ({  params, fetch }) => {
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<GeneralVokiSingleResultViewData>(
            fetch, `/vokis/${params.vokiId}/results/${params.resultId}`, {
            method: "GET",
        }
        ),
        vokiId: params.vokiId,
        resultId: params.resultId
    }
}
