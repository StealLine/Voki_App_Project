<script lang="ts">
	import type { Snippet } from 'svelte';

	const { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<div class="field-value">
	{@render children()}
</div>

<style>
	.field-value {
		display: flex;
		flex-direction: row;
		align-items: center;
		padding: 0.25rem 0.75rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
        font-size: 1.125rem;
        font-weight: 420;
		box-shadow: var(--shadow-xs);
	}
</style>
