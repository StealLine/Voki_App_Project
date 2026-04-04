import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";
import { VokiTakingSessionMarkerCookie } from "$lib/ts/cookies/voki-taking-session-marker";
import type { VokiType } from "$lib/ts/voki-type";
import { redirect, type Cookies } from "@sveltejs/kit";

export namespace VokiTakingServerLoad {
    export async function LoadVokiTakingSession<T extends { serverResultType: string; }>(
        fetchFunc: typeof fetch,
        params: { vokiId?: string },
        cookies: Cookies,
        url: URL,
        continueExistingUnfinishedSessionFunc: (
            fetchFunc: typeof fetch,
            vokiId: string,
            sessionId: string
        ) => Promise<ResponseResult<ServerBaseResultSuccessErrType | T>>,
        startNewSessionFunc: (
            fetchFunc: typeof fetch,
            vokiId: string,
            terminateExistingUnfinishedSession: boolean
        ) => Promise<ResponseResult<ServerBaseResultSuccessErrType | T>>
    ): ServerFullReturtnType<T> {
        const vokiId = params.vokiId;
        if (!vokiId) {
            throw redirect(302, `/`);
        }
        const sessionMarker = VokiTakingSessionMarkerCookie.get(cookies, vokiId);
        VokiTakingSessionMarkerCookie.clear(vokiId);
        if (url.searchParams.get('terminateExistingUnfinishedSession') === 'true') {
            if (!sessionMarker || sessionMarker.action !== "TERMINATE") {
                return {
                    vokiId: vokiId,
                    vokiType: "General",
                    isSuccess: true,
                    data: { serverResultType: "TerminateErr:NoSessionId" }
                }
            }
            const response = await startNewSessionFunc(fetchFunc, vokiId, true);
            return {
                vokiId: vokiId,
                vokiType: "General",
                ...response
            }
        }
        if (url.searchParams.get('continueExistingUnfinishedSession') === 'true') {
            if (!sessionMarker || sessionMarker.action !== "CONTINUE") {
                return {
                    vokiId: vokiId,
                    vokiType: "General",
                    isSuccess: true,
                    data: { serverResultType: "ContinueErr:NoSessionId" }
                }
            }
            const response = await continueExistingUnfinishedSessionFunc(fetchFunc, vokiId, sessionMarker.sessionId);
            return {
                vokiId: vokiId,
                vokiType: "General",
                ...response
            }
        }
        if (!VokiCatalogVisitMarkerCookie.checkIfSeen(cookies, vokiId)) {
            throw redirect(302, `/catalog/${vokiId}`);
        }
        const response = await startNewSessionFunc(fetchFunc, vokiId, false);
        return {
            vokiId: vokiId,
            vokiType: "General",
            ...response
        }
    }
    type ServerBaseResultSuccessType<T extends { serverResultType: string }> =
        | ServerBaseResultSuccessErrType
        | T
    export type ServerBaseResultSuccessErrType =
        | { serverResultType: "ContinueErr:NoSessionId" }
        | { serverResultType: "TerminateErr:NoSessionId" }

    export type ServerFullReturtnType<T extends { serverResultType: string; }> =
        Promise<
            ResponseResult<ServerBaseResultSuccessType<T>>
            & {
                vokiId: string;
                vokiType: VokiType
            }
        >
} 