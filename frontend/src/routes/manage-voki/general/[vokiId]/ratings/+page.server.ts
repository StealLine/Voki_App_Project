import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "./$types";
import type { VokiDailyRatingsSnapshot } from "./types";



export const load: PageServerLoad = async ({ params, fetch }): Promise<{
    response: ResponseResult<{
        vokiPublicationDate: Date;
        snapshots: VokiDailyRatingsSnapshot[]
    }>,
    vokiId: string
}> => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<{
            vokiPublicationDate: Date;
            snapshots: VokiDailyRatingsSnapshot[]
        }>(
            fetch, `/vokis/${params.vokiId}/manage/overview`, { method: 'GET' }
        ),
        vokiId: params.vokiId
    };
};