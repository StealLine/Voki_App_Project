import type { VokiRatingValue } from "$lib/ts/voki";

export type RatingValueToCountType = {
    [key in VokiRatingValue]: number;
};
export type VokiDailyRatingsSnapshot = {
    date: Date,
    distribution: RatingValueToCountType
};