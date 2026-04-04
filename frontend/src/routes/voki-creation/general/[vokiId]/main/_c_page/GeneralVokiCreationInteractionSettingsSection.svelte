<script lang="ts">
	import type { GeneralVokiInteractionSettings } from '../types';
	import GeneralVokiInteractionSettingsSectionEditing from './_c_interaction_settings_section/GeneralVokiInteractionSettingsSectionEditing.svelte';
	import GeneralVokiInteractionSettingsSectionView from './_c_interaction_settings_section/GeneralVokiInteractionSettingsSectionView.svelte';

	interface Props {
		vokiId: string;
		isEditing: boolean;
		updateSavedInteractionSettings: (newSettings: GeneralVokiInteractionSettings) => void;
		savedInteractionSettings: GeneralVokiInteractionSettings;
	}
	let {
		savedInteractionSettings,
		vokiId,
		isEditing = $bindable(),
		updateSavedInteractionSettings
	}: Props = $props();

	let editingInteractionSettings = $state(savedInteractionSettings);

	function startEditing() {
		editingInteractionSettings = savedInteractionSettings;
		isEditing = true;
	}
</script>

<div class="interaction-settings">
	{#if isEditing}
		<GeneralVokiInteractionSettingsSectionEditing
			{vokiId}
			savedInteractionSettings={editingInteractionSettings}
			cancelEditing={() => {
				isEditing = false;
			}}
			updateParent={(newSettings) => {
				updateSavedInteractionSettings(newSettings);
				isEditing = false;
			}}
		/>
	{:else}
		<GeneralVokiInteractionSettingsSectionView
			settings={editingInteractionSettings}
			startEditing={() => startEditing()}
		/>
	{/if}
</div>

<style>
	.interaction-settings {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.interaction-settings > :global(.err-block) {
		margin: 1rem 0;
	}
</style>
