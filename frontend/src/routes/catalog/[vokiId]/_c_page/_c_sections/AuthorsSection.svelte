<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	interface Props {
		primaryAuthorId: string;
		coAuthorIds: string[];
	}
	let { primaryAuthorId, coAuthorIds }: Props = $props();
</script>

<div class="participants-section">
	<label class="by-label">by: </label>
	<BasicUserDisplay userId={primaryAuthorId} interactionLevel="WholeComponentLink" />
	{#if coAuthorIds.length > 0}
		<svg class="plus-icon"><use href="#common-plus-icon" /></svg>
		{#each coAuthorIds as coAuthorId}
			<BasicUserDisplay userId={coAuthorId} interactionLevel="WholeComponentLink" />
		{/each}
	{/if}
</div>

<style>
	.by-label {
		margin-right: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		font-style: italic;
	}

	.participants-section {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		gap: 0.375rem;
	}

	.plus-icon {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 1rem;
		height: 1rem;
		padding: 0.125rem;
		margin: 0 0.25rem;
		border-radius: 99vw;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
		stroke-width: 3.25;
	}
</style>
