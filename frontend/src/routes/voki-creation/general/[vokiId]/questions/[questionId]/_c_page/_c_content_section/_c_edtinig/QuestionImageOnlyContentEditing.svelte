<script lang="ts">
	import type { AnswerDataImageOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import FullWidthQuestionContentMediaInput from './_c_shared/FullWidthQuestionContentMediaInput.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageOnly' }>;
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
			image: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(getAnswer: () => AnswerDataImageOnly)}
	{@const answer = getAnswer()}
	<FullWidthQuestionContentMediaInput
		type="image"
		mediaUrl={answer.image}
		onUploadSuccess={(newImage) => (answer.image = newImage)}
		mediaDisplay={imageDisplaySnippet}
	/>
	{#snippet imageDisplaySnippet()}
		<GeneralVokiCreationAnswerDisplayImage src={answer.image} />
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
</style>
