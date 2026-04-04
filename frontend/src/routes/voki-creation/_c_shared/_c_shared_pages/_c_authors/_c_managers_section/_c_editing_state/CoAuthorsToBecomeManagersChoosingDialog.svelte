<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';

	interface Props {
		allCoAuthors: string[];
		choseCoAuthors: (coAuthorIds: string[]) => void;
	}
	let { allCoAuthors, choseCoAuthors }: Props = $props();
	let idToIsSelected: Record<string, boolean> = $state({});

	function onSubmit() {
		choseCoAuthors(Object.keys(idToIsSelected).filter((id) => idToIsSelected[id]));
		dialog.close();
	}
	export function open(chosenCoAuthors: string[]) {
		idToIsSelected = Object.fromEntries(
			allCoAuthors.map((id) => [id, chosenCoAuthors.includes(id)])
		);
		dialog.open();
	}
	function toggleCoAuthor(authorId: string) {
		idToIsSelected[authorId] = !idToIsSelected[authorId];
	}
	let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton
	subheading="Choose co-authors to become managers"
	dialogId="co-authors-to-become-managers-dialog"
	bind:this={dialog}
>
	{#if allCoAuthors.length === 0}
		<div class="no-co-authors-message">No co-authors to choose from for now</div>
	{:else}
		<div class="co-authors">
			{#each allCoAuthors as coAuthorId}
				<div class="co-author" onclick={() => toggleCoAuthor(coAuthorId)}>
					<BasicUserDisplay userId={coAuthorId} interactionLevel={'JustDisplay'} />
					<DefaultCheckBox bind:checked={idToIsSelected[coAuthorId]} parentOnlyControl={true} />
				</div>
			{/each}
		</div>
	{/if}
	<PrimaryButton onclick={onSubmit}>Choose</PrimaryButton>
</DialogWithCloseButton>

<style>
	:global(#co-authors-to-become-managers-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	:global(#co-authors-to-become-managers-dialog > .dialog-content > .subheading) {
		width: max-content;
		padding: 0 2rem 1rem;
	}

	.no-co-authors-message {
		padding: 0 2rem 1rem;
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 425;
	}

	.co-authors {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}

	.co-author {
		display: flex;
		justify-content: space-between;
		align-items: center;
		width: 100%;
		height: fit-content;
		padding: 0.125rem 1rem;
		border-radius: 0.5rem;
	}

	.co-author:has(:global(.ok)):hover {
		box-shadow: var(--shadow-xs);
	}
</style>
