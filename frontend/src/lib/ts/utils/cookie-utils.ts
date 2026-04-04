import { browser } from "$app/environment";

export namespace CookieUtils {
    export type SetCookieOpts = {
        seconds?: number;
        path?: string;
        sameSite?: 'Lax' | 'Strict' | 'None';
        secure?: boolean;
    };

    export function setCookie(name: string, value: string, opts: SetCookieOpts = {}) {
        if (browser) {

            const {
                seconds = 180,
                path = '/',
                sameSite = 'Lax',
                secure = location.protocol === 'https:',
            } = opts;

            const expires = new Date(Date.now() + seconds * 1000).toUTCString();
            let cookie = `${encodeURIComponent(name)}=${encodeURIComponent(value)}; Expires=${expires}; Path=${path}; SameSite=${sameSite}`;
            if (secure) cookie += '; Secure';
            document.cookie = cookie;
        }
    }

    export function deleteCookie(name: string, path = '/') {
        if (browser) {
            document.cookie = `${encodeURIComponent(name)}=; Expires=Thu, 01 Jan 1970 00:00:00 GMT; Path=${path}`;
        }
    }

    export function getCookie(name: string): string | undefined {
        if (browser) {

            const map = Object.fromEntries(
                document.cookie.split('; ')
                    .filter(x => x)
                    .map(p => {
                        const i = p.indexOf('=');
                        return [decodeURIComponent(p.slice(0, i)), decodeURIComponent(p.slice(i + 1))];
                    })
            );
            return map[name];
        }
    }
}
