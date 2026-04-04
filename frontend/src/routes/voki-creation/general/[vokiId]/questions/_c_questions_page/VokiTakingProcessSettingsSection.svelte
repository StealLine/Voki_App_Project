<script lang="ts">
	import VokiCreationBasicHeader from '../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import VokiCreationDefaultButton from '../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../_c_shared/VokiCreationFieldName.svelte';
	import type { GeneralVokiTakingProcessSettings } from '../types';
	import TakingProcessSettingsEditingState from './_c_settings_section/TakingProcessSettingsEditingState.svelte';
	import TakingProcessSettingsFieldValue from './_c_settings_section/TakingProcessSettingsFieldValue.svelte';
	interface Props {
		savedSettings: GeneralVokiTakingProcessSettings;
		vokiId: string;
		isEditing: boolean;
		updateSavedSettings: (newSettings: GeneralVokiTakingProcessSettings) => void;
	}
	let { savedSettings, vokiId, isEditing = $bindable(), updateSavedSettings }: Props = $props();
</script>

<VokiCreationBasicHeader header="Voki taking process settings" />
{#if isEditing}
	<TakingProcessSettingsEditingState
		{vokiId}
		settings={savedSettings}
		updateParent={(newSettings) => {
			updateSavedSettings(newSettings);
			isEditing = false;
		}}
		cancelEditing={() => {
			isEditing = false;
		}}
	/>
{:else}
	<div class="settings-section">
		<div class="field first-filed">
			<VokiCreationFieldName fieldName="Questions order:" />
			{#if savedSettings.shuffleQuestions}
				<TakingProcessSettingsFieldValue text="Shuffled" iconId="#common-shuffle-icon" />
			{:else}
				<TakingProcessSettingsFieldValue text="Ordered" iconId="#common-order-icon" />
			{/if}
		</div>
		<div class="field">
			<VokiCreationFieldName fieldName="Answering flow:" />
			{#if savedSettings.forceSequentialAnswering}
				<TakingProcessSettingsFieldValue
					text="Sequential"
					iconId="#general-voki-taking-process-settings-force-sequential-flow-icon"
				/>
			{:else}
				<TakingProcessSettingsFieldValue
					text="Free"
					iconId="#general-voki-taking-process-settings-free-flow-icon"
				/>
			{/if}
		</div>
		<VokiCreationDefaultButton
			text="Edit settings"
			onclick={() => {
				isEditing = true;
			}}
		/>
	</div>
{/if}

<style>
	.settings-section {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.field {
		display: grid;
		align-items: center;
		margin: 0;
		grid-template-columns: 12.5rem 1fr;
	}

	.field:not(.first-filed) {
		margin-top: 1.5rem;
	}
</style>
