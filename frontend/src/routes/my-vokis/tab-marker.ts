import type { Cookies } from "@sveltejs/kit";

function cookieName() {
    return "my-vokis-page-last-tab";
}

export namespace MyVokisPageTabMarker {
    const possibleValues = ["draft-vokis", "published-vokis", "vokis-to-manage"] as const;
    export type Tab = typeof possibleValues[number];
    export function isTab(tab: string): tab is Tab {
        return (possibleValues as readonly string[]).includes(tab);
    }
    export function get(cookies: Cookies): Tab | null {
        const tab = cookies.get(cookieName()) || null;
        if (tab && isTab(tab)) {
            return tab;
        }
        return null;
    }
    export function set(cookies: Cookies, tab: Tab) {
        cookies.set(cookieName(), tab, {
            path: "/",
            sameSite: "lax",
            httpOnly: true,
            secure: true,

        });
    }
}