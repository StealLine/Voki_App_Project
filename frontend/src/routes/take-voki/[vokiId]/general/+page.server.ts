import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import { type ServerLoad } from "@sveltejs/kit";
import type { BaseVokiTakingSessionData, GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from "./types";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import { VokiTakingServerLoad } from "../shared-page-server-load";

export const load: ServerLoad = async ({ cookies, params, fetch, url }) => {
    return VokiTakingServerLoad.LoadVokiTakingSession<ServerSuccessType>(
        fetch,
        params,
        cookies,
        url,
        continueExistingUnfinishedSessionFunc,
        startNewSessionFunc
    );
}

async function continueExistingUnfinishedSessionFunc(
    fetchFunc: typeof fetch,
    vokiId: string,
    sessionId: string
): Promise<ResponseResult<VokiTakingServerLoad.ServerBaseResultSuccessErrType | ServerSuccessType>> {
    const response = await ApiVokiTakingGeneral.serverFetchJsonResponse<ContinueTakingServerSuccessResponse>(
        fetchFunc, `/vokis/${vokiId}/continue-taking`, RJO.POST({
            sessionId: sessionId
        })
    );
    if (!response.isSuccess) {
        return {
            isSuccess: false,
            errs: response.errs
        }
    }
    return {
        isSuccess: true,
        data: {
            serverResultType: "Success",
            sessionData: response.data,
            savedData: {
                anySave: true,
                savedChosenAnswers: response.data.savedChosenAnswers,
                currentQuestionId: response.data.currentQuestionId

            }
        }
    };
}
async function startNewSessionFunc(
    fetchFunc: typeof fetch,
    vokiId: string,
    terminateExistingUnfinishedSession: boolean
): Promise<ResponseResult<VokiTakingServerLoad.ServerBaseResultSuccessErrType | ServerSuccessType>> {

    const response = await ApiVokiTakingGeneral.serverFetchJsonResponse<StartTakingServerSuccessResponse>(
        fetchFunc, `/vokis/${vokiId}/start-taking`, RJO.POST({
            terminateExistingUnfinishedSession
        })
    );
    if (!response.isSuccess) {
        return {
            isSuccess: false,
            errs: response.errs
        }
    }
    if (response.data.newSessionStarted) {
        return {
            isSuccess: true,
            data: {
                serverResultType: "Success",
                sessionData: response.data,
                savedData: { anySave: false }
            },
        }
    }
    else {
        return {
            isSuccess: true,
            data: {
                serverResultType: "StartNewErr:UnfinishedSessionExists",
                sessionData: response.data
            },
        }
    }

}

type ServerSuccessType =
    | {
        serverResultType: "StartNewErr:UnfinishedSessionExists",
        sessionData: { questionsWithSavedAnswersCount: number; } & BaseVokiTakingSessionData
    }
    | {
        serverResultType: "Success",
        sessionData: GeneralVokiTakingData,
        savedData: PosssibleGeneralVokiTakingDataSaveData
    }
type ContinueTakingServerSuccessResponse = GeneralVokiTakingData & {
    savedChosenAnswers: Record<string, string[]>;
    currentQuestionId: string;
}
type StartTakingServerSuccessResponse =
    | ({ newSessionStarted: true } & GeneralVokiTakingData)
    | { newSessionStarted: false; questionsWithSavedAnswersCount: number } & BaseVokiTakingSessionData;