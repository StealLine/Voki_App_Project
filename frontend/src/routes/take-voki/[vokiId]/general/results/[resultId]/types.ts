import type { GeneralVokiResultsVisibility } from "$lib/ts/voki";

export type GeneralVokiSingleResultViewData = {
    id: string;
    name: string;
    text: string;
    image: string | null;
    resultsVisibility: GeneralVokiResultsVisibility;
    resultsCount: number;
    vokiName: string;
    hasUserTakenThisVoki: boolean;
}

