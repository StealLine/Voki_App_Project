<script lang="ts">
	import type { AnswerDataColorAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import ColorAndTextAnswerEditing from './_c_answers_content/ColorAndTextAnswerEditing.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ColorAndText' }>;
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
			color: '#000000',
			text: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(getAnswer: () => AnswerDataColorAndText)}
	<ColorAndTextAnswerEditing
		answer={getAnswer()}
		onTextChange={(newText) => (getAnswer().text = newText)}
		onColorChange={(newColor) => (getAnswer().color = newColor)}
	/>
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
