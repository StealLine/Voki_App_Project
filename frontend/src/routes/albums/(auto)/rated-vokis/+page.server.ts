import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "../../$types";
import type { VokiIdToBriefRatingData } from "./types";

export const load: PageServerLoad = async ({ fetch }) => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<{ vokiIdToLastRatingData: VokiIdToBriefRatingData }>(
            fetch, `/rated-vokis`, { method: "GET" }
        )
    };
};
