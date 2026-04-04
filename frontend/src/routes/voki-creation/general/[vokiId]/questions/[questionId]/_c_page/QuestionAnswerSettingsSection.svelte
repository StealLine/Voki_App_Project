<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import QuestionSettingsEditingState from './_c_settings_section/QuestionSettingsEditingState.svelte';
	import QuestionSettingsFieldValue from './_c_settings_section/QuestionSettingsFieldValue.svelte';
	import type { QuestionAnswersSettings } from '../types';

	interface Props {
		isEditing: boolean;
		savedAnswerSettings: QuestionAnswersSettings;
		questionId: string;
		vokiId: string;
		updateSavedAnswerSettings: (newSettings: QuestionAnswersSettings) => void;
	}
	let {
		isEditing = $bindable(),
		savedAnswerSettings,
		questionId,
		vokiId,
		updateSavedAnswerSettings
	}: Props = $props();
	let isSingleChoice = $derived(
		savedAnswerSettings.minAnswersCount === 1 && savedAnswerSettings.maxAnswersCount === 1
	);
</script>

{#if isEditing}
	<QuestionSettingsEditingState
		savedSettings={savedAnswerSettings}
		{questionId}
		{vokiId}
		cancelEditing={() => (isEditing = false)}
		updateParent={(newSettings) => {
			updateSavedAnswerSettings(newSettings);
			isEditing = false;
		}}
	/>
{:else}
	<div class="field">
		<VokiCreationFieldName fieldName="Answers order:" />
		<QuestionSettingsFieldValue>
			{#if savedAnswerSettings.shuffleAnswers}
				Shuffled
			{:else}
				Ordered
			{/if}
		</QuestionSettingsFieldValue>
	</div>
	<div class="field">
		<VokiCreationFieldName fieldName="Selection type:" />
		<QuestionSettingsFieldValue>
			{#if isSingleChoice}
				Single choice
			{:else}
				Multiple choice (from {savedAnswerSettings.minAnswersCount} to {savedAnswerSettings.maxAnswersCount})
			{/if}
		</QuestionSettingsFieldValue>
	</div>
	<VokiCreationDefaultButton
		text="Edit answer settings"
		class="edit-settings-button"
		onclick={() => (isEditing = true)}
	/>
{/if}

<style>
	.field {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
		margin: 1.5rem 0 0;
	}

	:global(.edit-settings-button) {
		padding: 0 1rem;
	}
</style>
