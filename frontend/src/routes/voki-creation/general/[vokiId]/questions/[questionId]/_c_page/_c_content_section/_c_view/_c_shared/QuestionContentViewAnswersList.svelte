<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';
	import type { QuestionPageResultsState } from '../../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentNoAnswersMessage from '../../_c_shared/QuestionContentNoAnswersMessage.svelte';

	interface Props {
		answers: T[];
		answerMainContent: Snippet<[T]>;
		resultsIdToNameState: QuestionPageResultsState;
	}
	let { answers, answerMainContent, resultsIdToNameState }: Props = $props();
</script>

{#if answers.length === 0}
	<QuestionContentNoAnswersMessage subtitle="Use the 'Edit content' button to add answers" />
{:else}
	<div class="answer-list">
		{#each answers as answer}
			<SingleAnswerWrapper
				resultsIdToName={resultsIdToNameState}
				answerRelatedResultsCount={answer.relatedResultIds.length}
				order={answer.order}
			>
				{#snippet resultsViewSnippet(idToName)}
					<div class="related-results">
						{#if answer.relatedResultIds.length === 0}
							<FieldNotSetLabel text="no results selected" class="no-related-results" />
						{:else}
							{#each answer.relatedResultIds as result}
								{#if idToName[result]}
									<div class="result">
										{idToName[result]}
									</div>
								{:else}
									<div class="result err">( unknown result )</div>
								{/if}
							{/each}
						{/if}
					</div>
				{/snippet}
				{#snippet answerContentSnippet()}
					{@render answerMainContent(answer)}
				{/snippet}
			</SingleAnswerWrapper>
		{/each}
	</div>
{/if}

<style>
	.answer-list {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}

	.related-results {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.375rem;
		width: 100%;
		height: 100%;
	}

	.related-results > :global(.no-related-results) {
		margin: 0;
		font-weight: 450;
	}

	.result {
		display: block;
		width: 100%;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.result.err {
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--red-1);
		color: var(--red-5);
		box-shadow: var(--err-shadow);
	}
</style>
