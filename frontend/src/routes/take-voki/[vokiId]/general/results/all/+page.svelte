<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import GeneralVokiResultPagesHeader from '../_c_shared/GeneralVokiResultPagesHeader.svelte';
	import GeneralVokiResultPagesVokiNameSpan from '../_c_shared/GeneralVokiResultPagesVokiNameSpan.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiResultWithDistributionItem from './_c_page/GeneralVokiResultWithDistributionItem.svelte';
	import type { ViewAllVokiResultsResponse } from './types';

	let { data }: PageProps = $props();
	function getResultsSortedByDescendingDistribution(data: ViewAllVokiResultsResponse) {
		return data.results.sort((a, b) => b.distributionPercent - a.distributionPercent);
	}
</script>

{#if data.response.isSuccess}
	<GeneralVokiResultPagesHeader>
		All({data.response.data.results.length}) results of the
		<GeneralVokiResultPagesVokiNameSpan vokiName={data.response.data.vokiName} />
		general Voki
	</GeneralVokiResultPagesHeader>
	<div class="results-grid">
		{#each getResultsSortedByDescendingDistribution(data.response.data) as r}
			<div class="results-list">
				<GeneralVokiResultWithDistributionItem
					showDistribution={data.response.data.showResultsDistribution}
					result={r}
				/>
			</div>
		{/each}
	</div>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load all voki results "
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}

<style>
	.results-grid {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}
</style>
