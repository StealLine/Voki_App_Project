<script lang="ts">
	import type { AnswerDataImageAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import CompactQuestionContentMediaInput from './_c_shared/CompactQuestionContentMediaInput.svelte';
	import AnswerEditingTextArea from './_c_answers_content/_c_shared/AnswerEditingTextArea.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageAndText' }>;
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
			image: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(getAnswer: () => AnswerDataImageAndText)}
	{@const answer = getAnswer()}
	<div class="answer-main-edit-grid">
		<AnswerEditingTextArea bind:text={() => answer.text, (v) => (answer.text = v)} />
		<div class="image-part">
			<CompactQuestionContentMediaInput
				type="image"
				mediaUrl={answer.image}
				onUploadSuccess={(newImage) => (answer.image = newImage)}
				mediaDisplay={imageDisplaySnippet}
			/>
		</div>
	</div>
	{#snippet imageDisplaySnippet()}
		<GeneralVokiCreationAnswerDisplayImage src={answer.image} maxWidth={20} />
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

	.image-part {
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
