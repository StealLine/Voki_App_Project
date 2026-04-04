<script lang="ts">
	import type { PageProps } from './$types';
	import QuestionInitializingDialog from './_c_questions_page/QuestionInitializingDialog.svelte';
	import VokiTakingProcessSettingsSection from './_c_questions_page/VokiTakingProcessSettingsSection.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import ListEmptyMessage from '../../../_c_shared/ListEmptyMessage.svelte';
	import GeneralVokiCreationQuestionsList from './_c_questions_page/GeneralVokiCreationQuestionsList.svelte';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import { GeneralVokiCreationAllQuestionsPageState } from './general-voki-creation-all-questions-page-state.svelte';
	import { setVokiCreationCurrentPageState } from '../../../voki-creation-page-context';

	let { data }: PageProps = $props();
	let questionInitializingDialog = $state<QuestionInitializingDialog>()!;
	const pageState = new GeneralVokiCreationAllQuestionsPageState(
		data.data?.settings!,
		data.data?.maxVokiQuestionsCount!
	);
	setVokiCreationCurrentPageState(pageState);
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<QuestionInitializingDialog bind:this={questionInitializingDialog} vokiId={data.vokiId!} />
	{#if data.data.questions.length === 0}
		<ListEmptyMessage
			messageText="This voki doesn't have any questions yet"
			btnText="Create first question"
			onBtnClick={() => questionInitializingDialog.open()}
		/>
	{:else}
		<VokiTakingProcessSettingsSection
			vokiId={data.vokiId!}
			savedSettings={pageState.savedSettings}
			updateSavedSettings={(newSettings) => (pageState.savedSettings = newSettings)}
			bind:isEditing={pageState.isEditingVokiTakingProcessSettings}
		/>
		<GeneralVokiCreationQuestionsList questionsProps={data.data.questions} vokiId={data.vokiId!} />
		{#if data.data.questions.length < pageState.maxQuestionsCount}
			<div class="add-new-question-btn-container">
				<PrimaryButton onclick={() => questionInitializingDialog.open()}
					>Add new question</PrimaryButton
				>
			</div>
		{/if}
	{/if}
{/if}

<style>
	.add-new-question-btn-container {
		display: flex;
		justify-content: center;
		margin: 1.25rem auto;
	}
</style>
