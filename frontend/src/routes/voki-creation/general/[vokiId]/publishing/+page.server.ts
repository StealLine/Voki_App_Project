import { ApiVokiCreationGeneral, type DraftVokiPublishingData } from '$lib/ts/backend-communication/voki-creation-backend-service';
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params, fetch }) => {
    return {
        response: await ApiVokiCreationGeneral.serverFetchJsonResponse<DraftVokiPublishingData>(
            fetch, `/vokis/${params.vokiId}/publishing-data`, { method: 'GET' }
        ),
        vokiId: params.vokiId
    };
};