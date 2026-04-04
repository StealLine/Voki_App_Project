import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { GeneralVokiResultsVisibility } from "$lib/ts/voki";

export type GeneralVokiMainInfo = {
    name: string;
    cover: string;
    tags: string[];
    details: VokiDetails;
    interactionSettings: GeneralVokiInteractionSettings
}
export type GeneralVokiInteractionSettings = {
    signedInOnlyTaking : boolean;
    resultsVisibility : GeneralVokiResultsVisibility;
    showResultsDistribution : boolean;
}
