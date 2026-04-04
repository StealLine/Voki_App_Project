<script lang="ts">
	import type { Language } from '$lib/ts/language';
	import { ProfileSetupProcessState } from './profile-setup-process-state.svelte';
	import ProfileSetupLanguagesStep from './_c_process_steps/ProfileSetupLanguagesStep.svelte';
	import ProfileSetupTagsStep from './_c_process_steps/ProfileSetupTagsStep.svelte';
	import ProfileSetupDisplayNameStep from './_c_process_steps/ProfileSetupDisplayNameStep.svelte';
	import ProfileSetupProfilePictureStep from './_c_process_steps/ProfileSetupProfilePictureStep.svelte';
	import SetupProcessStepHeader from './_c_setup_process/SetupProcessStepHeader.svelte';
	import SetupProcessFlowButtons from './_c_setup_process/SetupProcessFlowButtons.svelte';
	import ProfileSetupConfirmationStep from './_c_process_steps/ProfileSetupConfirmationStep.svelte';
	interface Props {
		uniqueName: string;
		initialLangs: Language[];
		initialTags: string[];
		initialProfilePic: string;
		initialDisplayName: string;
		maxDisplayNameLength: number;
		maxTagLength: number;
		changeStateToSaved: () => void;
	}
	let {
		uniqueName,
		initialLangs,
		initialTags,
		initialProfilePic,
		initialDisplayName,
		maxDisplayNameLength,
		maxTagLength,
		changeStateToSaved
	}: Props = $props();

	let setupProcessState = new ProfileSetupProcessState(
		uniqueName,
		initialLangs,
		initialTags,
		initialProfilePic,
		initialDisplayName
	);
	type Step = 'languages' | 'tags' | 'display-name' | 'profile-pic' | 'confirmation';
	let currentStep = $state<Step>('languages');
	type ProcessButton = {
		appearance: () => 'primary' | 'secondary' | 'hidden';
		onClick: () => void;
	};
	let nextButton: ProcessButton = $derived({
		appearance: () => {
			if (currentStep === 'confirmation') {
				return 'hidden';
			}
			if (currentStep === 'languages') {
				if (setupProcessState.anyLanguagesChosen()) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'tags') {
				if (setupProcessState.chosenFavoriteTags.size > 0) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'display-name') {
				if (setupProcessState.displayNameInputValue.length > 0) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'profile-pic') {
				return 'primary';
			}
			return 'secondary';
		},
		onClick: () => {
			if (currentStep === 'languages') {
				currentStep = 'tags';
			} else if (currentStep === 'tags') {
				currentStep = 'display-name';
			} else if (currentStep === 'display-name') {
				currentStep = 'profile-pic';
			} else if (currentStep === 'profile-pic') {
				currentStep = 'confirmation';
			}
		}
	});
	let prevButton: ProcessButton = $derived({
		appearance: () => {
			if (currentStep === 'languages') {
				return 'hidden';
			}
			return 'secondary';
		},
		onClick: () => {
			if (currentStep === 'confirmation') {
				currentStep = 'profile-pic';
			} else if (currentStep === 'profile-pic') {
				currentStep = 'display-name';
			} else if (currentStep === 'display-name') {
				currentStep = 'tags';
			} else if (currentStep === 'tags') {
				currentStep = 'languages';
			}
		}
	});
</script>

<div class="setup-process-container" aria-label="Profile setup">
	<div class="step-content">
		{#if currentStep === 'languages'}
			<SetupProcessStepHeader text="Choose languages you are comfortable with" />
			<ProfileSetupLanguagesStep
				isLanguageChosen={(l) => setupProcessState.isLanguageChosen(l)}
				toggleLanguage={(l) => setupProcessState.toggleLanguage(l)}
			/>
		{:else if currentStep === 'tags'}
			<SetupProcessStepHeader text="Choose tags you are interested in" />
			<ProfileSetupTagsStep
				chosenTags={setupProcessState.chosenFavoriteTags}
				tagsSuggestionsState={setupProcessState.suggestedTagsState()}
				chooseTag={(tag) => setupProcessState.chooseTag(tag)}
				removeTag={(tag) => setupProcessState.removeChosenTag(tag)}
				{maxTagLength}
			/>
		{:else if currentStep === 'display-name'}
			<SetupProcessStepHeader text="Input name that you want to be known by" />
			<ProfileSetupDisplayNameStep
				bind:displayName={setupProcessState.displayNameInputValue}
				maxLength={maxDisplayNameLength}
			/>
		{:else if currentStep === 'profile-pic'}
			<SetupProcessStepHeader text="Choose your profile picture" />
			<ProfileSetupProfilePictureStep bind:profilePic={setupProcessState.profilePicInputValue} />
		{:else if currentStep === 'confirmation'}
			<SetupProcessStepHeader text="Save chosen settings" />
			<ProfileSetupConfirmationStep
				uniqueName={setupProcessState.initialUniqueName}
				languages={setupProcessState.chosenLanguages}
				goToLanguagesStep={() => (currentStep = 'languages')}
				chosenTags={setupProcessState.chosenFavoriteTags}
				goToTagsStep={() => (currentStep = 'tags')}
				profilePic={setupProcessState.profilePicInputValue}
				goToPicStep={() => (currentStep = 'profile-pic')}
				displayName={setupProcessState.displayNameToSave}
				goToNameStep={() => (currentStep = 'display-name')}
				{changeStateToSaved}
			/>
		{:else}
			<h1>Something went wrong, reload the page</h1>
		{/if}
	</div>

	<div class="footer">
		<SetupProcessFlowButtons {nextButton} {prevButton} />
	</div>
</div>

<style>
	.setup-process-container {
		display: grid;
		gap: 1rem;
		width: 100%;
		max-width: 66rem;
	}

	.step-content {
		display: grid;
		align-items: start;
		gap: 1rem;
		min-height: 34rem;
		animation: fade-in 220ms ease-out both;
	}

	.footer {
		padding-top: 0.75rem;
		margin-top: 0.25rem;
	}
</style>
