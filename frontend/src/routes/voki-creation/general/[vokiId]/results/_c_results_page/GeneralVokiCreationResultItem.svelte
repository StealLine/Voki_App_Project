<script lang="ts">
	import type { ResultOverViewData } from './types';
	import ResultItemEditingState from './_c_result_item/ResultItemEditingState.svelte';
	import ResultItemViewState from './_c_result_item/ResultItemViewState.svelte';

	let {
		result,
		vokiId,
		updateParentOnSave,
		updateParentOnDelete,
		isEditing = $bindable()
	}: {
		result: ResultOverViewData;
		vokiId: string;
		updateParentOnSave: (result: ResultOverViewData) => void;
		updateParentOnDelete: (resultId: string) => void;
		isEditing?: boolean;
	} = $props<{
		result: ResultOverViewData;
		vokiId: string;
		updateParentOnSave: (result: ResultOverViewData) => void;
		updateParentOnDelete: (resultId: string) => void;
		isEditing?: boolean;
	}>();
</script>

<div class="result" class:editing={isEditing}>
	{#if isEditing}
		<ResultItemEditingState
			{result}
			{vokiId}
			{updateParentOnSave}
			cancelEditing={() => {
				isEditing = false;
			}}
		/>
	{:else}
		<ResultItemViewState
			{result}
			{vokiId}
			{updateParentOnDelete}
			startEditing={() => {
				isEditing = true;
			}}
		/>
	{/if}
</div>

<style>
	.result {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		padding: 0.25rem 0.5rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.75rem;
	}

	.result.editing {
		gap: 0.5rem;
		border: 0.125rem dashed var(--muted);
	}
</style>
