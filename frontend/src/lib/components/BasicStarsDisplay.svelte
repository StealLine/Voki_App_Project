<script lang="ts">
	interface Props {
		value: number;
		class?: string;
	}
	let { value, class: className = '' }: Props = $props();
</script>

<div class="stars-display {className}">
	{#each Array.from({ length: 5 }, (_, i) => i + 1) as i}
		<svg class="star" class:filled={value >= i} viewBox="0 0 24 24">
			<use href="#common-star-icon" />
		</svg>
	{/each}
</div>

<style>
	.stars-display {
		display: flex;
		flex-direction: row;
		gap: 0.125rem;
		margin-left: 0.25rem;
	}

	.star {
		width: 1.5rem;
		height: 1.5rem;
		color: var(--secondary-foreground);
		transition: transform 0.06s ease;
		fill: none;
	}

	.star:not(.filled) {
		stroke-width: 2;
	}

	.filled {
		color: var(--primary);
		fill: var(--primary);
	}
</style>
