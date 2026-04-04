<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		mainText: string;
		children?: Snippet;
	}
	let { mainText, children }: Props = $props();
</script>

<div class="cannot-rate-message">
	<h1>{mainText}</h1>
	{@render children?.()}
</div>

<style>
	.cannot-rate-message {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		width: calc(100% - 2rem);
		padding: 1rem;
		margin: 0.25rem 1rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.cannot-rate-message > h1 {
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 1.5rem;
		font-weight: 500;
	}
</style>
