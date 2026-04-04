<script lang="ts">
	interface Props {
		answerResults: string[];
		setAnswerResults: (results: string[]) => void;
		removeResultFromAnswer: (resultId: string) => void;
		resultsIdToName: Record<string, string>;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		answerResults,
		setAnswerResults,
		removeResultFromAnswer,
		resultsIdToName,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();
</script>

<div class="related-results">
	{#each answerResults as id}
		{#if resultsIdToName[id]}
			<div class="result">
				<label>
					{resultsIdToName[id]}
				</label>
				<svg class="remove-result-btn" onclick={() => removeResultFromAnswer(id)}
					><use href="#common-minus-icon" /></svg
				>
			</div>
		{:else}
			<div class="result err">
				<label>Result not found</label>
			</div>
		{/if}
	{/each}
	{#if answerResults.length < maxResultsForAnswerCount}
		<button
			class="add-btn unselectable"
			onclick={() =>
				openRelatedResultsSelectingDialog(answerResults, (selected) => setAnswerResults(selected))}
		>
			<svg><use href="#common-plus-icon" /></svg>
			related results
		</button>
	{/if}
</div>

<style>
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
		display: grid;
		gap: 0.25rem;
		width: 100%;
		padding: 0 0.25rem;
		grid-template-columns: 1fr auto;
	}

	.result.err {
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--red-1);
		color: var(--red-5);
		box-shadow: var(--err-shadow);
	}

	.result > label {
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 450;
	}

	.remove-result-btn {
		width: 1.375rem;
		height: 1.375rem;
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		stroke-width: 3;
	}

	.remove-result-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.add-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.125rem 1rem;
		margin-top: 0.25rem;
		border: none;
		border-radius: 4rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 400;
		cursor: pointer;
		align-self: center;
	}

	.add-btn > svg {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}
</style>
