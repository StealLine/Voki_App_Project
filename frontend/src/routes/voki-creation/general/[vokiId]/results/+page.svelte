<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import ListEmptyMessage from '../../../_c_shared/ListEmptyMessage.svelte';
	import VokiCreationBasicHeader from '../../../_c_shared/VokiCreationBasicHeader.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiCreationResultItem from './_c_results_page/GeneralVokiCreationResultItem.svelte';
	import ResultInitializingDialog from './_c_results_page/ResultInitializingDialog.svelte';
	import type { ResultOverViewData } from './_c_results_page/types';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import {
		GeneralVokiCreationResultsPageState,
		ResultItemState
	} from './general-voki-creation-results-page-state.svelte';
	import { setVokiCreationCurrentPageState } from '../../../voki-creation-page-context';

	let { data }: PageProps = $props();
	let resultCreationDialog = $state<ResultInitializingDialog>()!;
	let pageState = new GeneralVokiCreationResultsPageState(
		data.data?.results ?? [],
		data.data?.maxVokiResultsCount ?? 0
	);

	setVokiCreationCurrentPageState(pageState);

	function updateOnSave(result: ResultOverViewData) {
		pageState.updateResult(result);
	}
	function updateOnDelete(resultId: string) {
		pageState.deleteResult(resultId);
	}
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<ResultInitializingDialog
		bind:this={resultCreationDialog}
		vokiId={data.vokiId!}
		updateParentResults={(newResults) => {
			pageState.results = newResults.map((r) => new ResultItemState(r));
		}}
	/>

	{#if pageState.results.length === 0}
		<ListEmptyMessage
			messageText="This voki doesn't have any results yet"
			btnText="Create first result"
			onBtnClick={() => resultCreationDialog.open()}
		/>
	{:else}
		<VokiCreationBasicHeader header={`Voki results (${pageState.results.length})`} />
		<div class="results">
			{#each pageState.results as resultItem (resultItem.data.id)}
				<GeneralVokiCreationResultItem
					vokiId={data.vokiId!}
					result={resultItem.data}
					updateParentOnSave={updateOnSave}
					updateParentOnDelete={updateOnDelete}
					bind:isEditing={resultItem.isEditing}
				/>
			{/each}
		</div>
		{#if pageState.results.length < pageState.maxResultsCount}
			<div class="add-new-result-btn-container">
				<PrimaryButton onclick={() => resultCreationDialog.open()}>Add new result</PrimaryButton>
			</div>
		{/if}
	{/if}
{/if}

<style>
	.add-new-result-btn-container {
		display: flex;
		justify-content: center;
		margin: 1.25rem auto;
	}

	.results {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
	}
</style>
