import type { PageServerLoad } from "./_c_page/$types";
import { AllAuthorPageTabs, type AuthorPageTab, type AuthorViewData } from "./types";
import { ApiUserProfiles } from "$lib/ts/backend-communication/backend-services";

export const load: PageServerLoad = async ({ url, params, fetch }) => {
    const raw = (url.searchParams.get('tab') ?? '').toLowerCase();
    const tab: AuthorPageTab = ((AllAuthorPageTabs as readonly string[]).includes(raw) ? raw : 'about') as AuthorPageTab;

    return {
        response: await ApiUserProfiles.serverFetchJsonResponse<AuthorViewData>(
            fetch, `/users/${params.userId}/profile-view`, { method: 'GET' }
        ),
        userId: params.userId,
        currentTab: tab
    };
};