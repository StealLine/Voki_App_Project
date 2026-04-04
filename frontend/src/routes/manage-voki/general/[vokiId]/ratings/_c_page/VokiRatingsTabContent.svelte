<script lang="ts">
	import type { VokiDailyRatingsSnapshot } from '../types';
	import NonEmptyRatingsTabContent from './_c_tab_content/NonEmptyRatingsTabContent.svelte';
	import VokiHasNoRatings from './_c_tab_content/VokiHasNoRatings.svelte';
	import { VokiViewRatingsState } from './voki-view-ratings-state.svelte';

	interface Props {
		vokiId: string;
		vokiPublicationDate: Date;
		snapshots: VokiDailyRatingsSnapshot[];
	}
	let { vokiId, vokiPublicationDate, snapshots }: Props = $props();
	// svelte-ignore state_referenced_locally
	let componentState = new VokiViewRatingsState(vokiId, vokiPublicationDate, snapshots);

	function noRatingsInSnapshot(sn: VokiDailyRatingsSnapshot) {
		return (
			sn.distribution[1] === 0 &&
			sn.distribution[2] === 0 &&
			sn.distribution[3] === 0 &&
			sn.distribution[4] === 0 &&
			sn.distribution[5] === 0
		);
	}
</script>

{#if componentState.lastVokiSnapshot == undefined || noRatingsInSnapshot(componentState.lastVokiSnapshot)}
	<VokiHasNoRatings
		{vokiId}
		lastSnapshotDate={componentState.lastVokiSnapshot?.date ?? null}
		onTakeAndRetrieveRatingsSnapshotBtnClicked={() => componentState.takeNewVokiSnapshot()}
		snapshotsRetrievingState={componentState.refetchingState}
	/>
{:else}
	<NonEmptyRatingsTabContent
		lastSnapshot={componentState.lastVokiSnapshot}
		onTakeAndRetrieveRatingsSnapshotBtnClicked={() => componentState.takeNewVokiSnapshot()}
		snapshotsRetrievingState={componentState.refetchingState}
		lineChartFilter={componentState.lineChartFilter}
		snapshotsToShow={componentState.lineChartSnapshotsToShow}
		{vokiPublicationDate}
	/>
{/if}
