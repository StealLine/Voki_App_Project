<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<h1 class="results-header">{@render children()}</h1>

<style>
	.results-header {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.75rem 2rem;
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 1.5rem;
		font-weight: 550;
		letter-spacing: 0.25px;
	}
</style>
