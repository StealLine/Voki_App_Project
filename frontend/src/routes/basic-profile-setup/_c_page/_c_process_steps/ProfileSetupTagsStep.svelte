<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import TagsStepChosenList from './_c_tags_step/TagsStepChosenList.svelte';
	import TagsStepPartHeader from './_c_tags_step/TagsStepPartHeader.svelte';
	import TagsStepSearchingColumn from './_c_tags_step/TagsStepSearchingColumn.svelte';
	import TagsStepSuggestionsColumn from './_c_tags_step/TagsStepSuggestionsColumn.svelte';

	interface Props {
		chosenTags: Set<string>;
		tagsSuggestionsState:
			| { name: 'loading' }
			| { name: 'ok'; tags: Iterable<string> }
			| { name: 'errs'; errs: Err[] };
		chooseTag: (tag: string) => void;
		removeTag: (tag: string) => void;
		maxTagLength: number;
	}

	let { chosenTags, tagsSuggestionsState, chooseTag, removeTag, maxTagLength }: Props = $props();
</script>

<div class="tags-step-container">
	<div class="choosing-section-headers">
		<TagsStepPartHeader text="Choose from suggestions" />
		<p class="or-label">Or</p>
		<TagsStepPartHeader text="Find exactly the tags you want" />
	</div>
	<div class="choosing-section-contents">
		<TagsStepSuggestionsColumn
			{tagsSuggestionsState}
			{chooseTag}
			{removeTag}
			isTagChosen={(tag) => chosenTags.has(tag)}
		/>

		<TagsStepSearchingColumn
			isTagChosen={(tag) => chosenTags.has(tag)}
			{chooseTag}
			{maxTagLength}
		/>
	</div>
	<div class="chosen-tags-part">
		{#if chosenTags.size === 0}
			<div class="no-tags-msg">Chosen tags will appear here</div>
		{:else}
			<TagsStepPartHeader text="You have chosen {chosenTags.size} tags" />
			<TagsStepChosenList {chosenTags} {removeTag} />
		{/if}
	</div>
</div>

<style>
	.tags-step-container {
		display: grid;
		grid-template-rows: auto minmax(20rem, 1fr) auto;
	}

	.choosing-section-headers {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
		justify-content: center;
		align-items: center;
		margin-bottom: 0.5rem;
	}

	.or-label {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 600;
	}

	.choosing-section-contents {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 2rem;
	}

	.chosen-tags-part {
		display: flex;
		flex-direction: column;
		min-height: 4.75rem;
	}

	.no-tags-msg {
		width: fit-content;
		height: 100%;
		padding: 0 6rem;
		margin: 2rem auto 0;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		place-content: center;
	}
</style>
