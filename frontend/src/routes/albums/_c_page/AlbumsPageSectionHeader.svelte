<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		headerText: string;
		rightButton: {
			icon: Snippet;
			text: string;
			onclick: () => void;
		};
	}
	let { headerText, rightButton }: Props = $props();
</script>

<div class="header-container">
	<h1>{headerText}</h1>
	<button class="right-btn" onclick={() => rightButton.onclick()}
		>{@render rightButton.icon()} {rightButton.text}</button
	>
</div>

<style>
	.header-container {
		display: grid;
		grid-template-columns: 1fr auto;
		padding: 0 2rem;
		margin: 2rem 0 0.5rem;
	}

	.right-btn {
		display: grid;
		align-items: center;
		gap: 0.25rem;
		width: 13.5rem;
		height: fit-content;
		padding: 0.375rem 1rem;
		border: none;
		border-radius: 0.75rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		grid-template-columns: auto 1fr;
	}

	.right-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.right-btn > :global(svg) {
		height: 1.375rem;
		aspect-ratio: 1/1;
		color: inherit;
		stroke-width: 2.2;
	}

	.right-btn:focus {
		outline: 0.125rem solid var(--muted-foreground);
		box-shadow: var(--shadow);
	}

	.right-btn:focus:hover {
		outline-color: var(--accent-foreground);
	}
</style>
