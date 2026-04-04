<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import { toast } from 'svelte-sonner';
	import VokiPageTabSectionLabel from '../_c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import TagItemChip from '$lib/components/TagItemChip.svelte';

	let { tags, vokiId }: { tags: string[]; vokiId: string } = $props<{
		tags: string[];
		vokiId: string;
	}>();
</script>

<div class="voki-tags-section">
	<VokiPageTabSectionLabel fieldName="Tags:" />
	{#if tags.length === 0}
		<FieldNotSetLabel text="This voki has no tags" />
	{:else}
		{#each tags as tag}
			<TagItemChip {tag} className="tag-box" />
		{/each}
	{/if}
	<button
		class="tag-box suggest-tag-btn"
		class:right={tags.length === 0}
		onclick={() => toast.error('Tags suggestion is not implemented yet')}>+ Suggest a tag</button
	>
</div>

<style>
	.voki-tags-section {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		width: 100%;
		row-gap: 0.375rem;
	}

	.voki-tags-section :global(.tag-box) {
		margin-left: 0.375rem;
		border-radius: 0.25rem;
		font-size: 1rem;
	}

	.suggest-tag-btn {
		padding: 0.125rem 0.675rem;
		margin-left: auto;
		border: none;
		background-color: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
	}

	.suggest-tag-btn.right {
		margin-left: auto;
	}

	.suggest-tag-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
