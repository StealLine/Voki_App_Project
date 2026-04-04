import type { Err } from "./err";
import type { Language } from "./language";
import type { VokiType } from "./voki-type";


export type GeneralVokiQuestionContentType =
    | "TextOnly"
    | "ImageOnly"
    | "ImageAndText"
    | "ColorOnly"
    | "ColorAndText"
    | "AudioOnly"
    | "AudioAndText";

export type GeneralVokiResultsVisibility =
    | "Anyone"
    | "AfterTaking"
    | "OnlyReceived";

export type PublishedVokiBriefInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    managerIds: string[];
    hasMatureContent: boolean;
    language: Language;
    signedInOnlyTaking: boolean;
    publicationDate: Date;
};
export type PublishedVokiViewState =
    | { state: "loading", vokiId: string }
    | { state: "ok"; data: PublishedVokiBriefInfo }
    | { state: "errs"; errs: Err[], vokiId: string };

export type VokiRatingValue = 1 | 2 | 3 | 4 | 5;

export namespace VokiUtils {
    export function canUserManageVoki(voki: PublishedVokiBriefInfo, signedInUserId: string): boolean {
        return voki.primaryAuthorId === signedInUserId || voki.managerIds.includes(signedInUserId);
    }
    export function canUserSeeAllGeneralVokiResults(
        resultsVisibility: GeneralVokiResultsVisibility,
        resultIdsReceivedByUser: string[],
        allVokiResultIds: string[]
    ): boolean {
        if (resultsVisibility === "Anyone") {
            return true;
        }
        const receivedResultIds = new Set(resultIdsReceivedByUser);
        if (resultsVisibility === "AfterTaking") {
            return allVokiResultIds.some((resultId) => receivedResultIds.has(resultId));
        }
        if (resultsVisibility === "OnlyReceived") {
            return allVokiResultIds.every((resultId) => receivedResultIds.has(resultId));
        }
        return false;
    }
}