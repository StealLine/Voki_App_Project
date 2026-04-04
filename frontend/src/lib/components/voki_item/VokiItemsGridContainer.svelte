<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<div class="vokis-container">
	{@render children()}
</div>

<style>
	.vokis-container {
		display: grid;
		gap: 0.5rem;
		width: 100%;
		grid-template-columns: repeat(auto-fill, minmax(19rem, 1fr));
	}
</style>
