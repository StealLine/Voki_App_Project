<script lang="ts">
	import type { VokiItemHidableElements } from './voki-item';

	let { hide = [] }: { hide?: VokiItemHidableElements[] } = $props();
</script>

<div class="skeleton-item">
	<div class="skeleton-cover shimmer"></div>
	{#if !hide.includes('Name')}
		<div class="skeleton-name shimmer"></div>
	{/if}
</div>

<style>
	.skeleton-item {
		display: flex;
		flex-direction: column;
		gap: var(--voki-cover-name-gap);
		width: 100%;
	}

	.skeleton-cover,
	.skeleton-name {
		position: relative;
		width: 100%;
		border-radius: var(--voki-cover-border-radius);
		background-color: var(--secondary);
		color: var(--primary);
		overflow: hidden;
	}

	.skeleton-cover {
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.skeleton-name {
		height: var(--voki-name-max-height);
	}

	.shimmer::before {
		position: absolute;
		background: linear-gradient(
			to right,
			transparent 0%,
			color-mix(in srgb, var(--secondary-foreground) 10%, var(--secondary) 10%) 50%,
			transparent 100%
		);
		transform: translateX(-90%);
		animation: shimmer 2s infinite;
		content: '';
		inset: 0;
	}

	@keyframes shimmer {
		100% {
			transform: translateX(90%);
		}
	}
</style>
