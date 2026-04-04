import type { Cookies } from "@sveltejs/kit";
import { CookieUtils } from "../utils/cookie-utils";


function cookieName(vokiId: string) {
    return `voki-taking-${vokiId}`;
}

export namespace VokiCatalogVisitMarkerCookie {
    export function markSeenFor5Mins(vokiId: string) {
        CookieUtils.setCookie(cookieName(vokiId), 'seen', { seconds: 600 });
    }
    export function clear(vokiId: string) {
        CookieUtils.deleteCookie(cookieName(vokiId));
    }
    export function checkIfSeen(cookies: Cookies, vokiId: string): boolean {
        return cookies.get(cookieName(vokiId)) !== undefined;
    }
}
