<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { ResultOverViewData } from '../types';
	import ResultItemButtons from './ResultItemButtons.svelte';
	import { getConfirmActionDialogOpenFunction } from '../../../../../../_c_layout/_ts_layout_contexts/confirm-action-dialog-context';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	let {
		result,
		vokiId,
		updateParentOnDelete,
		startEditing
	}: {
		result: ResultOverViewData;
		vokiId: string;
		updateParentOnDelete: (resultId: string) => void;
		startEditing: () => void;
	} = $props<{
		result: ResultOverViewData;
		vokiId: string;
		updateParentOnDelete: (resultId: string) => void;
		startEditing: () => void;
	}>();

	const { open: openConfirmationDialog, close: closeConfirmationDialog } =
		getConfirmActionDialogOpenFunction();

	function openResultDeleteConfirmationDialog() {
		const deleteResult = async () => {
			const response = await ApiVokiCreationGeneral.fetchVoidResponse(
				`/vokis/${vokiId}/results/${result.id}/delete`,
				RJO.DELETE({})
			);
			if (response.isSuccess) {
				updateParentOnDelete(result.id);
				closeConfirmationDialog();
				return [];
			} else {
				return response.errs;
			}
		};
		openConfirmationDialog({
			mainContent: {
				mainText: `Are you sure you want to delete '${result.name}' result?`,
				subheading: undefined
			},
			dialogButtons: {
				confirmBtnText: 'Delete',
				confirmBtnOnclick: () => deleteResult(),
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: closeConfirmationDialog
			}
		});
	}
</script>

<div class="result-item-name">{result.name}</div>
<div class="result-content">
	<p class="result-text">{result.text}</p>
	{#if result.image}
		<img
			class="result-image"
			src={StorageBucketMain.fileSrcWithVersion(result.image)}
			alt="result"
		/>
	{/if}
</div>
<ResultItemButtons
	mainBtnText="Edit"
	mainBtnOnClick={() => startEditing()}
	secondaryBtnIconId="#common-trash-can-icon"
	secondaryBtnOnClick={() => openResultDeleteConfirmationDialog()}
/>

<style>
	.result-item-name {
		font-size: 1.675rem;
		font-weight: 475;
		letter-spacing: 0.5px;
		text-indent: 0.5em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.result-content {
		display: grid;
		grid-template-columns: 1fr auto;
	}

	.result-text {
		word-break: normal;
		overflow-wrap: anywhere;
		font-size: 1.25rem;
		font-weight: 420;
		letter-spacing: 0.1px;
	}

	.result-image {
		width: 100%;
		max-width: 15rem;
		height: 100%;
		max-height: 18rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs);
		object-fit: contain;
	}
</style>
