<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { TextareaAutosize } from 'runed';
	import { getVokiCreationPageContext } from '../../../../voki-creation-page-context';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import DefaultLanguageSelect from '$lib/components/inputs/DefaultLanguageSelect.svelte';
	interface Props {
		vokiId: string;
		savedDetails: VokiDetails;
		updateSavedDetails: (newDetails: VokiDetails) => void;
		cancelEditing: () => void;
	}
	let { vokiId, savedDetails, updateSavedDetails, cancelEditing }: Props = $props();

	const vokiCreationCtx = getVokiCreationPageContext();
	let descriptionTextarea: HTMLTextAreaElement = $state()!;

	let description = $derived(savedDetails.description);
	let language = $derived(savedDetails.language);
	let hasMatureContent = $derived(savedDetails.hasMatureContent);

	let isLoading = $state(false);
	let savingErrs: Err[] = $state([]);
	async function saveChanges() {
		isLoading = true;
		const response = await vokiCreationCtx.vokiCreationApi.updateVokiDetails(vokiId, {
			description,
			language,
			hasMatureContent
		});
		isLoading = false;
		if (response.isSuccess) {
			updateSavedDetails({
				description: response.data.description,
				language: response.data.language,
				hasMatureContent: response.data.hasMatureContent
			});
			savingErrs = [];
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
	new TextareaAutosize({ element: () => descriptionTextarea, input: () => description });
</script>

<VokiCreationFieldName fieldName="Description:" />
<textarea
	class="description-input"
	bind:this={descriptionTextarea}
	bind:value={description}
	name={StringUtils.rndStr()}
/>
<p class="field-p">
	<VokiCreationFieldName fieldName="Language:" />
	<DefaultLanguageSelect bind:value={language} />
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Mature content:" />
	<DefaultCheckBox bind:checked={hasMatureContent} />
</p>
<DefaultErrBlock errList={savingErrs} />
<VokiCreationSaveAndCancelButtons
	onCancel={cancelEditing}
	onSave={() => saveChanges()}
	isSaveLoading={isLoading}
/>

<style>
	.description-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.125rem 0.375rem;
		margin-top: 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.description-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.description-input:focus {
		outline-color: var(--primary);
	}

	.field-p {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		margin-top: 1rem;
	}
</style>
