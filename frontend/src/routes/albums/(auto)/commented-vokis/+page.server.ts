import { ApiVokiComments } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "../../$types";

export const load: PageServerLoad<{
    response: ResponseResult<{ vokiIdWithCommentDate: Record<string,string> }>;
}> = async ({ fetch }) => {
    return {
        response: await ApiVokiComments.serverFetchJsonResponse<{ vokiIdWithCommentDate: Record<string,string> }>(
            fetch, `/commented-vokis`, { method: "GET" }
        )
    };
};
