<script lang="ts">
	import { browser } from '$app/environment';
	import { VokiCatalogVisitMarkerCookie } from '$lib/ts/cookies/voki-catalog-visit-marker-cookie';
	import { onMount, onDestroy } from 'svelte';
	import { goto } from '$app/navigation';
	import type { GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from '../types';
	import VokiTakingHeader from '../../_c_shared/VokiTakingHeader.svelte';
	import SequentialAnsweringGeneralVokiTaking from './SequentialAnsweringGeneralVokiTaking.svelte';
	import FreeAnsweringGeneralVokiTaking from './FreeAnsweringGeneralVokiTaking.svelte';

	interface Props {
		sessionData: GeneralVokiTakingData;
		saveData: PosssibleGeneralVokiTakingDataSaveData;
	}
	let { sessionData: vokiTakingData, saveData }: Props = $props();
	function onResultReceived(receivedResultId: string) {
		goto(`/take-voki/${vokiTakingData.vokiId}/general/results/${receivedResultId}`, {
			replaceState: true
		});
	}
	let refreshTimer: number | undefined;
	function clearMarkerCookie() {
		if (refreshTimer) {
			clearInterval(refreshTimer);
		}
		VokiCatalogVisitMarkerCookie.clear(vokiTakingData.vokiId);
	}
	onMount(() => {
		if (browser) {
			VokiCatalogVisitMarkerCookie.markSeenFor5Mins(vokiTakingData.vokiId);

			refreshTimer = window.setInterval(() => {
				VokiCatalogVisitMarkerCookie.markSeenFor5Mins(vokiTakingData.vokiId);
			}, 60_000);
		}
	});
	onDestroy(clearMarkerCookie);
</script>

<VokiTakingHeader vokiType={'General'} vokiName={vokiTakingData.vokiName} />

{#if vokiTakingData.isWithForceSequentialAnswering}
	<SequentialAnsweringGeneralVokiTaking
		takingData={vokiTakingData}
		{saveData}
		clearVokiSeenUpdateTimer={clearMarkerCookie}
		{onResultReceived}
	/>
{:else}
	<FreeAnsweringGeneralVokiTaking
		takingData={vokiTakingData}
		{saveData}
		clearVokiSeenUpdateTimer={clearMarkerCookie}
		{onResultReceived}
	/>
{/if}
