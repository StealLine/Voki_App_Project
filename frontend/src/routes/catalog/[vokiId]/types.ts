import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki-type";


export type VokiOverviewInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    managerIds: string[];
    description: string;
    hasMatureContent: boolean;
    language: Language;
    tags: string[];
    publicationDate: Date;
    ratingsCount: number;
    commentsCount: number;
    signedInOnlyTaking: boolean;
} & VokiTypeWithSpecificData;

type VokiTypeMap = {
    General: GeneralVokiTypeSpecificData;
    Scoring: ScoringVokiTypeSpecificData;
    TierList: TierListVokiTypeSpecificData;
};

export type VokiTypeWithSpecificData = {
    [K in keyof VokiTypeMap]: {
        type: K;
        typeSpecificData: VokiTypeMap[K];
    }
}[keyof VokiTypeMap];

export type GeneralVokiTypeSpecificData = { forceSequentialAnswering: boolean, shuffleQuestions: boolean, anyAudios: boolean };
export type TierListVokiTypeSpecificData = object;
export type ScoringVokiTypeSpecificData = object;


export const AllVokiPageTabs = ['about', 'comments', 'ratings'] as const;
export type VokiPageTab = typeof AllVokiPageTabs[number];

export type RatingsTabDataType =
    | { state: 'empty' }
    | { state: 'loading' }
    | { state: 'error', errs: Err[] }
    | {
        state: 'fetched',
        averageRating: number,
        allRatings: VokiRatingData[],
        userHasTaken: boolean,
        isAverageOutdated: boolean
    };

export type VokiRatingsWithAverage = { averageRating: number, ratings: VokiRatingData[] };
export type VokiRatingData = { value: number; userId: string; dateTime: Date; };
