<script lang="ts">
	interface Props {
		current: number;
		onClick: (newVal: number) => void;
	}
	const { current, onClick }: Props = $props();
	const stars = Array.from({ length: 5 }, (_, i) => i + 1);
	let starHovered: number | null = $state(null);
</script>

<div class="stars-input">
	{#each stars as i}
		<button
			type="button"
			class="star-btn"
			aria-label={`Set rating ${i} of ${5}`}
			onmouseenter={() => (starHovered = i)}
			onmouseleave={() => (starHovered = null)}
			onclick={() => onClick(i)}
		>
			<svg
				class="star"
				class:filled={current >= i}
				class:highlight={starHovered !== null && starHovered >= i}
				viewBox="0 0 24 24"
			>
				<use href="#common-star-icon" />
			</svg>
		</button>
	{/each}
</div>

<style>
	.stars-input {
		display: flex;
		flex-direction: row;
		gap: 0;
	}

	.star {
		width: 1.75rem;
		height: 1.75rem;
		padding: 0.125rem;
		color: var(--secondary-foreground);
		transition: transform 0.06s ease;
		fill: none;
	}

	.star:not(.filled) {
		stroke-width: 2;
	}

	.star:hover {
		transform: scale(1.1);
	}

	.highlight {
		color: var(--primary);
	}

	.filled {
		color: var(--primary);
		fill: var(--primary);
	}

	.star-btn {
		padding: 0;
		border: none;
		background: transparent;
		cursor: pointer;
		appearance: none;
	}
</style>
