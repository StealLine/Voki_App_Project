import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { QuestionFullInfo } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<ServerSuccesRes>(
        fetch, `/vokis/${params.vokiId}/questions/${params.questionId}`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId, questionId: params.questionId };
}

type ServerSuccesRes = QuestionFullInfo & {
    resultsIdToName: Record<string, string>;
}