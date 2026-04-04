<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { TextareaAutosize } from 'runed';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../_c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import VokiCreationDefaultButton from '../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	interface Props {
		savedText: string;
		questionId: string;
		vokiId: string;
		isEditing: boolean;
		updateSavedText: (newText: string) => void;
	}
	let { savedText, questionId, vokiId, isEditing = $bindable(), updateSavedText }: Props = $props();
	let textarea: HTMLTextAreaElement = $state()!;
	let newText = $derived(savedText);

	let savingErrs = $state<Err[]>([]);
	let isSaveLoading = $state(false);

	new TextareaAutosize({ element: () => textarea, input: () => newText });

	function startEditing() {
		newText = savedText;
		isEditing = true;
		savingErrs = [];
	}
	async function saveChanges() {
		isSaveLoading = true;
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ newText: string }>(
			`/vokis/${vokiId}/questions/${questionId}/update-text`,
			RJO.PATCH({ newText: newText })
		);
		isSaveLoading = false;

		if (response.isSuccess) {
			updateSavedText(response.data.newText);
			isEditing = false;
			savingErrs = [];
		} else {
			savingErrs = response.errs;
		}
	}
</script>

{#if isEditing}
	<VokiCreationFieldName fieldName="Question text:" />

	<textarea
		class="text-input"
		bind:this={textarea}
		bind:value={newText}
		name={StringUtils.rndStr()}
	/>
	{#if savingErrs.length > 0}
		<DefaultErrBlock errList={savingErrs} class="question-text-err-block" />
	{/if}
	<VokiCreationSaveAndCancelButtons
		onCancel={() => (isEditing = false)}
		onSave={() => saveChanges()}
		{isSaveLoading}
	/>
{:else}
	<p class="question-text-p">
		<VokiCreationFieldName fieldName="Question text:" />
		<label class="question-text-value">{savedText}</label>
	</p>
	<VokiCreationDefaultButton text="Edit text" onclick={startEditing} />
{/if}

<style>
	.text-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.375rem;
		margin-top: 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.25rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.text-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.text-input:focus {
		outline-color: var(--primary);
	}

	:global(.err-block.question-text-err-block) {
		margin-top: 0.5rem;
	}

	.question-text-p {
		width: 100%;
	}

	.question-text-value {
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
		text-decoration: none;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
