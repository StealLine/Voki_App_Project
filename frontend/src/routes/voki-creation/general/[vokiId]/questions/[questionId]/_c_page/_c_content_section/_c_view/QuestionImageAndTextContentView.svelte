<script lang="ts">
	import type { AnswerDataImageAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageAndText' }>;
		resultsIdToName: QuestionPageResultsState;
	}
	let { content, resultsIdToName }: Props = $props();
</script>

<div class="question-content">
	<QuestionContentViewAnswersList
		answers={content.answers}
		resultsIdToNameState={resultsIdToName}
		{answerMainContent}
	/>
</div>
{#snippet answerMainContent(answer: AnswerDataImageAndText)}
	<div class="answer-content">
		<AnswersViewTextDisplay text={answer.text} />
		<GeneralVokiCreationAnswerDisplayImage src={answer.image} maxWidth={24} maxHeight={14} />
	</div>
{/snippet}

<style>
	.answer-content {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 0.5rem;
		margin-inline: auto;
		width: 100%;
	}
</style>
