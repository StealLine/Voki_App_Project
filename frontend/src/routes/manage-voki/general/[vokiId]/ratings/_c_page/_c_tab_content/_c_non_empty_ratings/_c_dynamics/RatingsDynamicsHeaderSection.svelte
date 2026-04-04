<script lang="ts">
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import RatingsDateInput from './_c_header/RatingsDateInput.svelte';

	interface Props {
		from: Date | null;
		to: Date | null;
		today: Date;
		vokiPublicationDate: Date;
	}
	let { today, from = $bindable(), to = $bindable(), vokiPublicationDate }: Props = $props();

	let fromError = $derived.by(() => {
		if (from) {
			const selectedFrom = new Date(from);
			selectedFrom.setHours(0, 0, 0, 0);
			const pubDate = new Date(vokiPublicationDate);
			pubDate.setHours(0, 0, 0, 0);
			const todayDate = new Date(today);
			todayDate.setHours(0, 0, 0, 0);

			if (selectedFrom < pubDate) {
				return `Cannot be before publication date (${DateUtils.toLocaleDateOnly(vokiPublicationDate)})`;
			}
			if (selectedFrom > todayDate) {
				return 'Cannot be in the future';
			}
			const effectiveTo = to ? new Date(to) : new Date(today);
			effectiveTo.setHours(0, 0, 0, 0);
			if (selectedFrom >= effectiveTo) {
				return 'Cannot be same or after end date';
			}
		}
		return null;
	});

	let toError = $derived.by(() => {
		if (to) {
			const selectedTo = new Date(to);
			selectedTo.setHours(0, 0, 0, 0);
			const pubDate = new Date(vokiPublicationDate);
			pubDate.setHours(0, 0, 0, 0);
			const todayDate = new Date(today);
			todayDate.setHours(0, 0, 0, 0);

			if (selectedTo > todayDate) {
				return 'Cannot be in the future';
			}
			if (selectedTo < pubDate) {
				return `Cannot be before publication date (${DateUtils.toLocaleDateOnly(vokiPublicationDate)})`;
			}
			const effectiveFrom = from ? new Date(from) : new Date(vokiPublicationDate);
			effectiveFrom.setHours(0, 0, 0, 0);
			if (selectedTo <= effectiveFrom) {
				return 'Cannot be same or before start date';
			}
		}
		return null;
	});

	export function anyInputError() {
		return !!(fromError || toError);
	}
</script>

<div class="header-section">
	See voki ratings dynamics from
	<RatingsDateInput
		date={from}
		error={fromError}
		quickPickLabel="Voki publishing"
		onQuickPick={() => {
			from = null;
		}}
		onCustomDateSelect={() => {
			from = vokiPublicationDate;
		}}
		onDateChange={(d) => {
			from = d;
		}}
	/>
	to
	<RatingsDateInput
		date={to}
		error={toError}
		quickPickLabel="Today"
		onQuickPick={() => {
			to = null;
		}}
		onCustomDateSelect={() => {
			to = today;
		}}
		onDateChange={(d) => {
			to = d;
		}}
	/>
</div>

<style>
	.header-section {
		display: flex;
		flex-direction: row;
		gap: 1rem;
		padding: 1.75rem 2rem;
		align-items: center;
		font-size: 1.375rem;
		font-weight: 550;
		color: var(--text);
		margin: 0 auto;
	}
</style>
