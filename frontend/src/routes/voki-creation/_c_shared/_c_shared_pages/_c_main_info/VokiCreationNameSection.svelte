<script lang="ts">
	import { TextareaAutosize } from 'runed';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import VokiCreationDefaultButton from '../../VokiCreationDefaultButton.svelte';
	import { getVokiCreationPageContext } from '../../../voki-creation-page-context';
	import type { Err } from '$lib/ts/err';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiCreationSaveAndCancelButtons from '../../VokiCreationSaveAndCancelButtons.svelte';
	interface Props {
		savedName: string;
		vokiId: string;
		isEditing: boolean;
		updateSavedVokiName: (newName: string) => void;
	}
	let { savedName, vokiId, isEditing = $bindable(), updateSavedVokiName }: Props = $props();

	let textarea = $state<HTMLTextAreaElement>()!;
	let nameEditingValue = $derived(savedName);
	let isLoading = $state(false);
	let savingErrs = $state<Err[]>([]);
	const { vokiCreationApi, headerVokiName } = getVokiCreationPageContext();

	new TextareaAutosize({ element: () => textarea, input: () => nameEditingValue });

	function startEditing() {
		nameEditingValue = savedName;
		isEditing = true;
		savingErrs = [];
	}
	async function saveChanges() {
		isLoading = true;
		const response = await vokiCreationApi.updateVokiName(vokiId, nameEditingValue);
		isLoading = false;
		if (response.isSuccess) {
			updateSavedVokiName(response.data.newVokiName);
			isEditing = false;
			savingErrs = [];
			headerVokiName.invalidate();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<div class="voki-name-section">
	{#if isEditing}
		<VokiCreationFieldName fieldName="Voki name:" />

		<textarea
			class="name-input"
			bind:this={textarea}
			bind:value={nameEditingValue}
			name={StringUtils.rndStr()}
		/>
		{#if savingErrs.length > 0}
			<DefaultErrBlock errList={savingErrs} />
		{/if}
		<VokiCreationSaveAndCancelButtons
			onCancel={() => (isEditing = false)}
			onSave={() => saveChanges()}
			isSaveLoading={isLoading}
		/>
	{:else}
		<p class="voki-name-p">
			<VokiCreationFieldName fieldName="Voki name:" />
			<label class="voki-name-value">{savedName}</label>
		</p>
		<VokiCreationDefaultButton text="Edit name" onclick={startEditing} />
	{/if}
</div>

<style>
	.voki-name-section {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.name-input {
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

	.name-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.name-input:focus {
		outline-color: var(--primary);
	}

	.voki-name-section :global(.err-block) {
		margin-top: 0.5rem;
	}

	.voki-name-p {
		width: 100%;
	}

	.voki-name-value {
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
		text-decoration: none;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
