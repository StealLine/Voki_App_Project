<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { watch } from 'runed';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../_c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import VokiCreationOneOfMultipleChoicesInput from '../../../../../_c_shared/VokiCreationOneOfMultipleChoicesInput.svelte';
	import type { GeneralVokiInteractionSettings } from '../../types';

	interface Props {
		vokiId: string;
		savedInteractionSettings: GeneralVokiInteractionSettings;
		cancelEditing: () => void;
		updateParent: (newSettings: GeneralVokiInteractionSettings) => void;
	}
	let { vokiId, savedInteractionSettings, cancelEditing, updateParent }: Props = $props();
	let editingSettingsState = $derived(savedInteractionSettings);
	let savingErrs: Err[] = $state([]);
	let isLoading = $state(false);
	async function saveChanges() {
		isLoading = true;
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<GeneralVokiInteractionSettings>(
			`/vokis/${vokiId}/update-interaction-settings`,
			RJO.PATCH({
				signedInOnlyTaking: editingSettingsState.signedInOnlyTaking,
				resultsVisibility: editingSettingsState.resultsVisibility,
				showResultsDistribution: editingSettingsState.showResultsDistribution
			})
		);
		isLoading = false;
		if (response.isSuccess) {
			updateParent(response.data);
			savingErrs = [];
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
	watch(
		() => editingSettingsState.signedInOnlyTaking,
		() => {
			if (!editingSettingsState.signedInOnlyTaking) {
				editingSettingsState.resultsVisibility = 'Anyone';
			}
		}
	);
</script>

<p class="field-p">
	<VokiCreationFieldName fieldName="Signed in only taking:" />
	<DefaultCheckBox bind:checked={editingSettingsState.signedInOnlyTaking} />
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Results visibility:" />

	<VokiCreationOneOfMultipleChoicesInput
		containerClass="results-visibility-input"
		options={[
			{
				name: 'Anyone',
				selected: editingSettingsState.resultsVisibility === 'Anyone',
				disabled: false,
				onclick: () => (editingSettingsState.resultsVisibility = 'Anyone')
			},
			{
				name: 'After taking',
				selected: editingSettingsState.resultsVisibility === 'AfterTaking',
				disabled: !editingSettingsState.signedInOnlyTaking,
				onclick: () => (editingSettingsState.resultsVisibility = 'AfterTaking')
			},
			{
				name: 'Only received',
				selected: editingSettingsState.resultsVisibility === 'OnlyReceived',
				disabled: !editingSettingsState.signedInOnlyTaking,
				onclick: () => (editingSettingsState.resultsVisibility = 'OnlyReceived')
			}
		]}
	/>
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Show results distribution:" />
	<DefaultCheckBox bind:checked={editingSettingsState.showResultsDistribution} />
</p>
{#if savingErrs.length > 0}
	<DefaultErrBlock errList={savingErrs} />
{/if}
<VokiCreationSaveAndCancelButtons
	onCancel={cancelEditing}
	onSave={() => saveChanges()}
	isSaveLoading={isLoading}
/>

<style>
	.field-p {
		display: grid;
		grid-template-columns: max-content 1fr;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		margin-top: 1rem;
	}

	.field-p :global(.results-visibility-input) {
		padding-left: 0.5rem;
	}

	.field-p :global(.results-visibility-input > .option) {
		width: 9.5rem;
	}
</style>
