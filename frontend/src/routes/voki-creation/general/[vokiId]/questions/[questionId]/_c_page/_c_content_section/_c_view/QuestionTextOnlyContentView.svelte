<script lang="ts">
	import type { AnswerDataTextOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'TextOnly' }>;
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
{#snippet answerMainContent(answer: AnswerDataTextOnly)}
	<div class="answer-content">
		<AnswersViewTextDisplay text={answer.text} />
	</div>
{/snippet}

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}
</style>
