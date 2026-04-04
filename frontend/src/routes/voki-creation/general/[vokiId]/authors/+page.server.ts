import type { ServerLoad } from "@sveltejs/kit";
import type { VokiCreationAuthorsInfo } from "../../../_c_shared/_c_shared_pages/_c_authors/types";
import { ApiVokiCreationCore } from "$lib/ts/backend-communication/backend-services";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationCore.serverFetchJsonResponse<VokiCreationAuthorsInfo>(
        fetch, `/vokis/${params.vokiId}/authors-info`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}

