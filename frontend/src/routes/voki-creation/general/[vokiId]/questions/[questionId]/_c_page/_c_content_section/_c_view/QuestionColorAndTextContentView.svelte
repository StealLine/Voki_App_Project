<script lang="ts">
	import type { AnswerDataColorAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ColorAndText' }>;
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
{#snippet answerMainContent(answer: AnswerDataColorAndText)}
	<div class="answer-content">
		<AnswersViewTextDisplay text={answer.text} />
		<div class="color-part">
			<div class="color" style="background-color: {answer.color}"></div>
			<label class="color-label">{answer.color}</label>
		</div>
	</div>
{/snippet}

<style>
	.answer-content {
		display: grid;
		gap: 1rem;
		width: 100%;
		height: 100%;
		grid-template-columns: 1fr 10rem;
	}

	.color-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
	}

	.color {
		width: 100%;
		height: 80%;
		min-height: 4rem;
		border-radius: 0.5rem;
	}

	.color-label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
	}
</style>
