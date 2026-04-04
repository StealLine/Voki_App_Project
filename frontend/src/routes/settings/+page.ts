import { ApiUserProfiles } from "$lib/ts/backend-communication/backend-services";
import type { UserSettingsData } from "./types";
import type { PageLoad } from './$types';



export const load: PageLoad = async ({ fetch }) => {

    return {
        response: await ApiUserProfiles.serverFetchJsonResponse<UserSettingsData>(
            fetch, `/settings`, { method: 'GET' }
        ),
    };
};

