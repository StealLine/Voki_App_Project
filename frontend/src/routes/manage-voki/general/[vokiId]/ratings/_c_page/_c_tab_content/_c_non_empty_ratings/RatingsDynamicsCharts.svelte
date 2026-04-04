<script lang="ts">
	import type { VokiDailyRatingsSnapshot } from '../../../types';
	import RatingsDynamicsHeaderSection from './_c_dynamics/RatingsDynamicsHeaderSection.svelte';
	import RatingsHistoryLineChartComponent from './_c_dynamics/RatingsHistoryLineChartComponent.svelte';

	interface Props {
		from: Date | null;
		to: Date | null;
		snapshotsToShow: VokiDailyRatingsSnapshot[];
		vokiPublicationDate: Date;
		today: Date;
	}
	let {
		from = $bindable(),
		to = $bindable(),
		snapshotsToShow,
		vokiPublicationDate,
		today
	}: Props = $props();
	let inputsComponent: RatingsDynamicsHeaderSection | null = $state(null);

	const formatter = new Intl.DateTimeFormat(undefined, {
		month: 'short',
		day: 'numeric',
		year: 'numeric'
	});

	const chartData = $derived.by(() => {
		return snapshotsToShow
			.filter((s) => {
				const sDate = new Date(s.date);
				sDate.setHours(0, 0, 0, 0);
				return sDate <= new Date();
			})
			.map((s) => {
				const dist = s.distribution;
				const totalSum = dist[1] * 1 + dist[2] * 2 + dist[3] * 3 + dist[4] * 4 + dist[5] * 5;
				const totalCount = dist[1] + dist[2] + dist[3] + dist[4] + dist[5];
				const average = totalCount === 0 ? 0 : Math.round((totalSum * 100) / totalCount) / 100;
				const xLabel = formatter.format(new Date(s.date));

				return {
					xLabel,
					average,
					totalCount
				};
			});
	});

	const averageData = $derived(chartData.map((d) => ({ xLabel: d.xLabel, yValue: d.average })));
	const totalData = $derived(chartData.map((d) => ({ xLabel: d.xLabel, yValue: d.totalCount })));
</script>

<div class="line-charts-wrapper">
	<RatingsDynamicsHeaderSection
		bind:this={inputsComponent}
		{vokiPublicationDate}
		bind:from
		bind:to
		{today}
	/>
	<div class="charts-grid">
		<RatingsHistoryLineChartComponent
			title="Average Rating"
			data={averageData}
			color="var(--primary)"
			yMax={5}
			anyInputError={inputsComponent?.anyInputError()}
		/>
		<RatingsHistoryLineChartComponent
			title="Total Ratings Count"
			data={totalData}
			color="var(--primary)"
			anyInputError={inputsComponent?.anyInputError()}
		/>
	</div>
</div>

<style>
	.line-charts-wrapper {
		display: flex;
		flex-direction: column;
		margin-top: 1.5rem;
		background: var(--back);
		border-radius: 1.25rem;
		box-shadow: var(--shadow-xs);
		overflow: hidden;
	}

	.charts-grid {
		display: flex;
		flex-direction: column;
		gap: 3.5rem;
		padding: 2.5rem;
	}
</style>
