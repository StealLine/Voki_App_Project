<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type { Err } from '$lib/ts/err';

	interface Props {
		tagsSuggestionsState:
			| { name: 'loading' }
			| { name: 'ok'; tags: Iterable<string> }
			| { name: 'errs'; errs: Err[] };
		isTagChosen: (tag: string) => boolean;
		chooseTag: (tag: string) => void;
		removeTag: (tag: string) => void;
	}

	let { tagsSuggestionsState, isTagChosen, chooseTag, removeTag }: Props = $props();
</script>

{#if tagsSuggestionsState.name === 'loading'}
	<div class="suggestions-not-loaded">
		<CubesLoader sizeRem={2} color= 'var(--primary)'/>
		<h1>Loading suggestions</h1>
	</div>
{:else if tagsSuggestionsState.name === 'errs'}
	<div class="suggestions-not-loaded">
		<h1>Couldn't load tag suggestions</h1>
		<DefaultErrBlock errList={tagsSuggestionsState.errs} />
	</div>
{:else if tagsSuggestionsState.name === 'ok'}
	<div class="suggestion-list">
		{#each tagsSuggestionsState.tags as tag}
			{#if isTagChosen(tag)}
				<div class="tag chosen" onclick={() => removeTag(tag)}>#{tag}</div>
			{:else}
				<div class="tag" onclick={() => chooseTag(tag)}>#{tag}</div>
			{/if}
		{/each}
	</div>
{:else}{/if}

<style>
	.suggestions-not-loaded {
		display: flex;
		flex-direction: column;
		justify-content: start;
		align-items: center;
		gap: 1rem;
		padding-top: 2rem;
	}

	.suggestions-not-loaded h1 {
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
	}
	
	.suggestion-list {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		align-items: center;
		gap: 0.75rem 1rem;
		height: fit-content;
	}

	.tag {
		display: grid;
		gap: 0.125rem;
		padding: 0.125rem 0.675rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		letter-spacing: 0.1px;
		box-shadow: var(--shadow-xs);
		cursor: pointer;
		grid-template-columns: 1fr auto;
	}

	.tag:not(.chosen):hover {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.tag.chosen {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: none;
	}
</style>
