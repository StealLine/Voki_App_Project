import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "./$types";
import { AllVokiPageTabs, type VokiOverviewInfo, type VokiPageTab } from "./types";

export const load: PageServerLoad = async ({ url, params, fetch }) => {
    const raw = (url.searchParams.get('tab') ?? '').toLowerCase();
    const tab: VokiPageTab = ((AllVokiPageTabs as readonly string[]).includes(raw) ? raw : 'about') as VokiPageTab;

    return {
        response: await ApiVokisCatalog.serverFetchJsonResponse<VokiOverviewInfo>(
            fetch, `/vokis/${params.vokiId}/overview`, { method: 'GET' }
        ),
        vokiId: params.vokiId,
        currentTab: tab
    };
};