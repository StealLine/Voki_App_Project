<script lang="ts">
	interface Props {
		mainColor: string;
		secondaryColor: string;
		icons: string[];
		value: string;
	}
	let { mainColor, secondaryColor, icons, value = $bindable() }: Props = $props();
	function select(id: string) {
		value = id;
	}
</script>

<div class="grid">
	{#each icons as id}
		<button
			type="button"
			class="tile"
			class:active={id === value}
			onclick={() => select(id)}
			aria-label={`Choose icon ${id}`}
		>
			<svg
				viewBox="0 0 24 24"
				aria-hidden="true"
				style={`--icon-color-1:${mainColor}; --icon-color-2:${secondaryColor};`}
			>
				<use href={`#${id}`} />
			</svg>
		</button>
	{/each}
</div>

<style>
	.grid {
		display: grid;
		grid-template-columns: repeat(6, 1fr);
		gap: 0.5rem;
	}

	.tile {
		padding: 0.5rem;
		border: 0.125rem solid var(--secondary);
		border-radius: var(--radius);
		background: var(--back);
		box-shadow: var(--shadow-xs);
		transition:
			border-color 120ms ease,
			box-shadow 120ms ease,
			transform 80ms ease;
		cursor: pointer;
	}

	.tile:hover {
		border-color: var(--primary);
	}

	.tile.active {
		border-color: var(--primary);
		box-shadow: 0 0 0 0.15rem color-mix(in srgb, var(--primary) 28%, transparent);
	}

	svg {
		width: 2rem;
		height: 2rem;
		stroke-width: 1.9;
	}
</style>
