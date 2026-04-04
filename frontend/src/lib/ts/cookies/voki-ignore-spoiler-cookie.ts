import { CookieUtils } from "../utils/cookie-utils";

function cookieName(vokiId: string) {
    return `voki-ignore-spoiler-${vokiId}`;
}

export namespace VokiIgnoreSpoilerCookie {
    const TWELVE_HOURS_IN_SECONDS = 12 * 60 * 60;

    export function markAsIgnoredFor12Hours(vokiId: string) {
        CookieUtils.setCookie(cookieName(vokiId), 'true', { seconds: TWELVE_HOURS_IN_SECONDS });
    }

    export function isIgnored(vokiId: string): boolean {
        return CookieUtils.getCookie(cookieName(vokiId)) === 'true';
    }
}
