<script lang="ts">
	import type { AnswerDataColorOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ColorOnly' }>;
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
{#snippet answerMainContent(answer: AnswerDataColorOnly)}
	<div class="answer-content">
		<div class="color" style="background-color: {answer.color}"></div>
		<label>{answer.color}</label>
	</div>
{/snippet}

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
	}

	.color {
		width: 90%;
		height: 60%;
		min-height: 4rem;
		border-radius: 0.5rem;
	}

	label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
	}
</style>
