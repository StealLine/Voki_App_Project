<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		icon: Snippet;
		text: string;
	}
	let { icon, text }: Props = $props();
</script>

<div class="msg-container">
	<div class="icon-wrapper">
		{@render icon()}
	</div>
	<p>{text}</p>
</div>

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 1rem;
		color: var(--secondary-foreground);
		text-align: center;
	}

	.icon-wrapper {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.75rem;
		margin-bottom: 0.75rem;
		border-radius: 50%;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.icon-wrapper > :global(svg) {
		width: 2rem;
		height: 2rem;
		stroke-width: 1.5;
		animation: icon-fade-in 0.2s ease;
	}

	p {
		margin: 0;
		font-size: 1rem;
		font-weight: 450;
		animation: text-fade-in 0.2s ease;
		cursor: default;
	}

	@keyframes icon-fade-in {
		from {
			opacity: 0.8;
			transform: scale(0.75);
		}

		to {
			opacity: 1;
			transform: scale(1);
		}
	}

	@keyframes text-fade-in {
		from {
			opacity: 0.8;
			transform: scale(0.9);
		}

		to {
			opacity: 1;
			transform: scale(1);
		}
	}

	@media (prefers-reduced-motion: reduce) {
		.icon-wrapper > :global(svg) {
			animation: none;
		}
	}
</style>
