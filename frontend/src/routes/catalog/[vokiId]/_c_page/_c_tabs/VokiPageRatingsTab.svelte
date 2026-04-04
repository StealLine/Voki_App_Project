<script lang="ts">
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import type { RatingsTabDataType, VokiRatingData } from '../../types';
	import RatingsTabAllRatingsList from './_c_ratings_tab/RatingsTabAllRatingsList.svelte';
	import RatingsTabAverageRating from './_c_ratings_tab/RatingsTabAverageRating.svelte';
	import { onMount } from 'svelte';

	interface Props {
		tabData: RatingsTabDataType;
		fetchTabData: () => Promise<void>;
		saveNewUserRating: (
			value: number
		) => Promise<ResponseResult<VokiRatingData>>;
		reloadOutdatedRatings: () => Promise<void>;
	}
	let { tabData, fetchTabData, saveNewUserRating, reloadOutdatedRatings }: Props = $props();

	onMount(() => {
		if (tabData.state == 'empty') {
			fetchTabData();
		}
	});
</script>

{#if tabData.state === 'loading'}
	<div class="loading-container">
		<CubesLoader sizeRem={5} color= 'var(--primary)' />
		<h1 class="loading-text">Loading Voki ratings</h1>
	</div>
{:else if tabData.state === 'error'}
	<h1 class="error-text">Ratings data loading error</h1>
	<DefaultErrBlock errList={tabData.errs} />
	<ReloadButton onclick={() => fetchTabData()} />
{:else if tabData.state === 'empty'}
	<h1 class="error-text">Something went wrong</h1>
	<ReloadButton onclick={() => fetchTabData()} />
{:else if tabData.state === 'fetched'}
	<RatingsTabAverageRating
		averageRating={tabData.averageRating}
		count={tabData.allRatings.length}
		isOutdated={tabData.isAverageOutdated}
		reloadOutdated={reloadOutdatedRatings}
	/>

	<RatingsTabAllRatingsList
		allRatings={tabData.allRatings}
		hasUserTakenVoki={tabData.userHasTaken}
		saveNewRating={saveNewUserRating}
	/>
{:else}
	<h1>Something is wrong</h1>
{/if}

<style>
	.loading-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		margin: 2rem 0;
	}

	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 0.25px;
	}
</style>
