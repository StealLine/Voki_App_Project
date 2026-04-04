import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingProcessSettings, QuestionBriefInfo } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<{
        questions: QuestionBriefInfo[],
        settings: GeneralVokiTakingProcessSettings,
        maxVokiQuestionsCount: number
    }>(
        fetch, `/vokis/${params.vokiId}/questions/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
