<script lang="ts">
	import type { AnswerDataAudioAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import CompactQuestionContentMediaInput from './_c_shared/CompactQuestionContentMediaInput.svelte';
	import AnswerEditingTextArea from './_c_answers_content/_c_shared/AnswerEditingTextArea.svelte';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'AudioAndText' }>;
		maxAnswersForQuestionCount: number;
		resultsIdToNameState: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		content = $bindable(),
		maxAnswersForQuestionCount,
		resultsIdToNameState,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();

	function addNewAnswer() {
		content.answers.push({
			text: '',
			audio: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(getAnswer: () => AnswerDataAudioAndText)}
	{@const answer = getAnswer()}
	<div class="answer-main-edit-grid">
		<AnswerEditingTextArea bind:text={() => answer.text, (v) => (answer.text = v)} />
		<div class="audio-part">
			<CompactQuestionContentMediaInput
				type="audio"
				mediaUrl={answer.audio}
				onUploadSuccess={(newAudio) => (answer.audio = newAudio)}
				mediaDisplay={audioDisplaySnippet}
			/>
		</div>
	</div>
	{#snippet audioDisplaySnippet()}
		<BasicAudio src={answer.audio} />
	{/snippet}
{/snippet}

<div class="question-content">
	<QuestionContentEditingAnswersList
		bind:answers={content.answers}
		{resultsIdToNameState}
		{answerMainContent}
		{maxAnswersForQuestionCount}
		{maxResultsForAnswerCount}
		{openRelatedResultsSelectingDialog}
		{addNewAnswer}
	/>
</div>

<style>
	.question-content {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.answer-main-edit-grid {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.audio-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		min-width: 12rem;
		transition:
			height 0.12s ease,
			width 0.12s ease;
	}
</style>
