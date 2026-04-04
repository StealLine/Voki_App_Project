import type { Cookies } from '@sveltejs/kit';
import { CookieUtils } from '../utils/cookie-utils';

function cookieName(vokiId: string) {
    return `voki-continue-session-${vokiId}`;
}

export namespace VokiTakingSessionMarkerCookie {

    const cookieMaxAge = 60 * 2;
    const ContinueKey = "CONTINUE";
    const TerminateKey = "TERMINATE";

    export function markContinueFor2Min(vokiId: string, sessionId: string) {
        let cookieValue = `${sessionId}|${ContinueKey}`;
        CookieUtils.setCookie(cookieName(vokiId), cookieValue, {
            seconds: cookieMaxAge
        });
    }

    export function markTerminateFor2Min(vokiId: string, sessionId: string) {
        let cookieValue = `${sessionId}|${TerminateKey}`;
        CookieUtils.setCookie(cookieName(vokiId), cookieValue, {
            seconds: cookieMaxAge
        });
    }

    export function clear(vokiId: string) {
        CookieUtils.deleteCookie(cookieName(vokiId));
    }

    export function get(cookies: Cookies, vokiId: string):
        | { sessionId: string, action: "CONTINUE" | "TERMINATE" }
        | null {
        const v = cookies.get(cookieName(vokiId));
        if (!v) {
            return null;
        }
        const parts = v.split("|");
        if (parts.length !== 2) {
            return null;
        }
        const sessionId = parts[0];
        const action = parts[1];
        if (action !== "CONTINUE" && action !== "TERMINATE") {
            return null;
        }
        return { sessionId, action };
    }
}
