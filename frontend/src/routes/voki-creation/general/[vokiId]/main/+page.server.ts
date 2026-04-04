import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiMainInfo } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<GeneralVokiMainInfo>(
        fetch, `/vokis/${params.vokiId}/main-info`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
