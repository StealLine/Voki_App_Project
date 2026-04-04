<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		userIds: string[];
	}
	let { userIds }: Props = $props();
</script>

<div class="count">
	{userIds.length}
	<div class="hint">
		{#each userIds as userId}
			<BasicUserDisplay
				{userId}
				interactionLevel={'UniqueNameGotoOnClick'}
				class="co-author-display"
			/>
		{/each}
	</div>
</div>

<style>
	.count {
		position: relative;
		display: inline-flex;
		justify-content: center;
		align-items: center;
		min-width: 1.5rem;
		padding: 0 0.35rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 500;
		transition:
			background-color 0.2s,
			color 0.2s;
		cursor: help;
	}

	.count:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.hint {
		position: absolute;
		top: 100%;
		left: 50%;
		z-index: 50;
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: max-content;
		max-width: 20rem;
		padding: 0.75rem;
		margin-top: 0.25rem;
		border: 1px solid var(--muted);
		border-radius: 0.75rem;
		background-color: var(--back);
		color: var(--text);
		opacity: 0;
		box-shadow: var(--shadow-lg);
		transition: all 0.2s cubic-bezier(0.16, 1, 0.3, 1);
		transform: translateX(-50%) translateY(0.5rem);
		flex-flow: row wrap;
		visibility: hidden;
		pointer-events: none;
		transition-delay: 0.25s;
	}

	.count:hover .hint,
	.hint:hover {
		opacity: 1;
		visibility: visible;
		pointer-events: auto;
		transform: translateX(-50%) translateY(0);
		transition-delay: 0s;
	}

	.hint::before {
		content: '';
		position: absolute;
		top: -1rem;
		left: 0;
		width: 100%;
		height: 1rem;
	}

	.hint > :global(.co-author-display) {
		--profile-pic-width: 2.5rem !important;
	}
</style>
