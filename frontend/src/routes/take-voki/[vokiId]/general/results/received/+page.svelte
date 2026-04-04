<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import GeneralVokiResultPagesHeader from '../_c_shared/GeneralVokiResultPagesHeader.svelte';
	import GeneralVokiResultPagesVokiNameSpan from '../_c_shared/GeneralVokiResultPagesVokiNameSpan.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiReceivedResultItem from './_c_page/GeneralVokiReceivedResultItem.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<GeneralVokiResultPagesHeader>
		My received results of the
		<GeneralVokiResultPagesVokiNameSpan vokiName={data.response.data.vokiName} />
		general Voki ({data.response.data.results.length} out of {data.response.data.resultsCount})
	</GeneralVokiResultPagesHeader>

	<ul class="results">
		{#each data.response.data.results as result}
			<GeneralVokiReceivedResultItem {result} vokiId={data.vokiId!} />
		{/each}
	</ul>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load all received voki results "
		authRequiredMessage="To see your received results, you need to sign in"
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}

<style>
	.results {
		display: grid;
		gap: 0.75rem;
		padding: 0;
		margin: 0;
		list-style: none;
	}
</style>
