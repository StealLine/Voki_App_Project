<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { TextareaAutosize } from 'runed';
	import type { ResultOverViewData } from '../types';
	import ResultEditingStateImage from './_c_editing_state/ResultEditingStateImage.svelte';
	import ResultItemButtons from './ResultItemButtons.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	let {
		vokiId,
		result,
		updateParentOnSave,
		cancelEditing
	}: {
		vokiId: string;
		result: ResultOverViewData;
		updateParentOnSave: (result: ResultOverViewData) => void;
		cancelEditing: () => void;
	} = $props<{
		vokiId: string;
		result: ResultOverViewData;
		updateParentOnSave: (result: ResultOverViewData) => void;
		cancelEditing: () => void;
	}>();
	let errs = $state<Err[]>([]);
	let resultEditing = $state({ ...result });
	async function saveResult() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<ResultOverViewData>(
			`/vokis/${vokiId}/results/${result.id}/update`,
			RJO.PUT({
				newName: resultEditing.name,
				newText: resultEditing.text,
				newImage: resultEditing.image
			})
		);

		if (response.isSuccess) {
			updateParentOnSave(response.data);
			cancelEditing();
		} else {
			errs = response.errs;
		}
	}

	let resultTextInput = $state<HTMLTextAreaElement>()!;
	new TextareaAutosize({ element: () => resultTextInput, input: () => resultEditing.text });
</script>

<input class="result-name result-input" bind:value={resultEditing.name} />
<div class="result-content">
	<textarea
		class="result-text result-input"
		bind:value={resultEditing.text}
		bind:this={resultTextInput}
		name={StringUtils.rndStr()}
	/>
	<ResultEditingStateImage bind:image={resultEditing.image} bind:errs resultId={result.id} />
</div>
{#if errs.length > 0}
	<DefaultErrBlock errList={errs} />
{/if}
<ResultItemButtons
	mainBtnText="Save"
	mainBtnOnClick={() => saveResult()}
	secondaryBtnIconId="#common-cross-icon"
	secondaryBtnOnClick={() => cancelEditing()}
/>

<style>
	.result-content {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
	}

	.result-input {
		width: 100%;
		box-sizing: border-box;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 450;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.result-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.result-input:focus {
		outline-color: var(--primary);
	}

	.result-name {
		padding: 0.125rem 0.375rem;
		font-size: 1.675rem;
		font-weight: 475;
		letter-spacing: 0.5px;
	}

	.result-text {
		padding: 0.125rem 0.25rem;
		font-size: 1.25rem;
		font-weight: 420;
		letter-spacing: 0.1px;
	}
</style>
