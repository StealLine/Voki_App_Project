import { ApiVokiRatings, RJO } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { DateUtils } from "$lib/ts/utils/date-utils";
import type { RatingValueToCountType, VokiDailyRatingsSnapshot } from "../types";


export class VokiViewRatingsState {
	readonly vokiId: string;
	#publicationDate: Date;
	#allVokiSnapshots: VokiDailyRatingsSnapshot[] = $state()!;

	lineChartFilter: { from: Date | null; to: Date | null; } = $state({
		from: null,
		to: null
	});
	refetchingState:
		| { name: 'ok' }
		| { name: 'loading' }
		| { name: 'errs', errs: Err[] } = $state()!;

	lastVokiSnapshot = $derived.by(() => {
		if (this.#allVokiSnapshots.length === 0) {
			return undefined;
		}
		return this.#allVokiSnapshots.reduce((max, s) => (s.date > max.date ? s : max));
	})
	constructor(vokiId: string, publicationDate: Date, allSnapshots: VokiDailyRatingsSnapshot[]) {
		this.vokiId = vokiId;
		this.#publicationDate = publicationDate;
		this.#allVokiSnapshots = allSnapshots;
		this.refetchingState = { name: 'ok' };
	}

	async takeNewVokiSnapshot() {
		this.refetchingState = { name: 'loading' };

		const response = await ApiVokiRatings.fetchJsonResponse<{
			vokiPublicationDate: Date;
			snapshots: VokiDailyRatingsSnapshot[]
		}>(
			`/vokis/${this.vokiId}/manage/take-snapshot`, RJO.POST({})
		)
		if (response.isSuccess) {
			this.#allVokiSnapshots = response.data.snapshots;
			this.#publicationDate = response.data.vokiPublicationDate;
			this.refetchingState = { name: 'ok' };
		} else {
			this.refetchingState = { name: 'errs', errs: response.errs };
		}
	}

	emptyDistribution(): RatingValueToCountType {
		return { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
	}

	lineChartSnapshotsToShow: VokiDailyRatingsSnapshot[] = $derived.by(() => {
		if (this.refetchingState.name !== 'ok') {
			return [];
		}

		const { from, to } = this.lineChartFilter;

		const sorted = [...this.#allVokiSnapshots]
			.map((s) => ({
				...s,
				Date: DateUtils.startOfDay(s.date)
			}))
			.sort((a, b) => a.Date.getTime() - b.Date.getTime());

		if (sorted.length === 0) {
			return [];
		}

		const startDate = DateUtils.startOfDay(from ?? this.#publicationDate);
		const endDate = DateUtils.startOfDay(to ?? new Date());

		const result: VokiDailyRatingsSnapshot[] = [];

		let currentDate = startDate;
		const reversedSorted = [...sorted].reverse();

		while (currentDate <= endDate) {
			const found = sorted.find((s) => DateUtils.sameDay(s.Date, currentDate));

			if (found) {
				result.push(found);
			} else {
				const previousSnapshot = reversedSorted.find((s) => s.Date < currentDate);
				result.push({
					date: currentDate,
					distribution: previousSnapshot ? previousSnapshot.distribution : this.emptyDistribution()
				});
			}

			currentDate = DateUtils.addDays(currentDate, 1);
		}

		return result;
	});
}
