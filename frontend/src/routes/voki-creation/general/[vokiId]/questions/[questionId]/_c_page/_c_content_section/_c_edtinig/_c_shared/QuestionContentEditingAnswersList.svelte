<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import type { QuestionPageResultsState } from '../../../../general-voki-creation-specific-question-page-state.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';
	import VokiCreationDefaultButton from '../../../../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import QuestionContentNoAnswersMessage from '../../_c_shared/QuestionContentNoAnswersMessage.svelte';
	import QuestionContentEditingAnswerResults from './_c_answers_list/QuestionContentEditingAnswerResults.svelte';

	interface Props {
		answers: T[];
		answerMainContent: Snippet<[() => T]>;
		maxAnswersForQuestionCount: number;
		addNewAnswer: () => void;
		resultsIdToNameState: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		answers = $bindable(),
		answerMainContent,
		maxAnswersForQuestionCount,
		addNewAnswer,
		resultsIdToNameState,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();

	function removeResultFromAnswer(answer: T, resultId: string) {
		answer.relatedResultIds = answer.relatedResultIds.filter((rId) => rId !== resultId);
	}

	function removeAnswer(answer: T) {
		answers = answers.filter((a) => a !== answer);
		answers = answers.map((a, i) => ({
			...a,
			order: i + 1
		}));
	}
</script>

{#if answers.length === 0}
	<QuestionContentNoAnswersMessage
		subtitle="Add some answers to this question using the button below"
	/>
	<button onclick={addNewAnswer} class="add-new-answer-btn">Add first answer</button>
{:else}
	<div class="answer-list">
		{#each answers as answer}
			<SingleAnswerWrapper
				resultsIdToName={resultsIdToNameState}
				answerRelatedResultsCount={answer.relatedResultIds.length}
				order={answer.order}
			>
				{#snippet resultsViewSnippet(idToName)}
					<QuestionContentEditingAnswerResults
						answerResults={answer.relatedResultIds}
						setAnswerResults={(results) => (answer.relatedResultIds = results)}
						removeResultFromAnswer={(resId) => removeResultFromAnswer(answer, resId)}
						resultsIdToName={idToName}
						{maxResultsForAnswerCount}
						{openRelatedResultsSelectingDialog}
					/>
				{/snippet}
				{#snippet answerContentSnippet()}
					{#key answer.order}
						<div class="answer-content-grid">
							{@render answerMainContent(() => answer)}
							<button class="delete-answer-btn" onclick={() => removeAnswer(answer)}>
								<div class="hint">Delete this answer</div>
								<svg><use href="#common-trash-can-icon" /></svg>
							</button>
						</div>
					{/key}
				{/snippet}
			</SingleAnswerWrapper>
		{/each}
		{#if answers.length < maxAnswersForQuestionCount}
			<VokiCreationDefaultButton
				class="add-new-answer-btn"
				text="Add new answer"
				onclick={addNewAnswer}
			/>
		{:else}
			Answers count limit for question reached ({answers.length}/{maxAnswersForQuestionCount})
		{/if}
	</div>
{/if}

<style>
	.add-new-answer-btn {
		padding: 0.25rem 1rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 425;
		letter-spacing: 0.15px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
	}

	.add-new-answer-btn:hover {
		background-color: var(--primary-hov);
	}

	.answer-content-grid {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
	}

	.delete-answer-btn {
		position: relative;
		display: flex;
		justify-content: center;
		align-items: center;
		width: 1.75rem;
		height: 1.75rem;
		padding: 0.25rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		transition: all 0.1s ease;
		cursor: pointer;
	}

	.delete-answer-btn:hover {
		background-color: var(--red-1);
		color: var(--red-4);
	}

	.delete-answer-btn:hover .hint {
		opacity: 1;
		visibility: visible;
		transform: translateY(-50%) translateX(-0.25rem);
	}

	.delete-answer-btn svg {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 2;
	}

	.hint {
		position: absolute;
		top: 50%;
		right: 100%;
		z-index: 10;
		display: flex;
		align-items: center;
		width: max-content;
		padding: 0.25rem 0.375rem 0.25rem 0.5rem;
		margin-right: 0.25rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 450;
		opacity: 0;
		box-shadow: var(--shadow), var(--shadow-md);
		transition: all 0.15s ease-in-out;
		transform: translateY(-50%);
		visibility: hidden;
		pointer-events: none;
		overflow: visible;
	}

	.hint::after {
		position: absolute;
		top: 50%;
		left: 100%;
		width: 0.5rem;
		height: 0.5rem;
		background-color: var(--muted);
		transform: translate(-50%, -50%) rotate(45deg);
		content: '';
	}

	:global(.add-new-answer-btn) {
		width: 100%;
		margin: 1rem 0 0 !important;
	}
</style>
