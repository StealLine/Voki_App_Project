<script lang="ts">
	import { VokiTypeUtils, type VokiType } from '$lib/ts/voki-type';
	interface Props {
		type: VokiType;
		isSelected: boolean;
		onclick: () => void;
	}
	let { type, isSelected, onclick }: Props = $props();
</script>

<div class="card" class:selected={isSelected} {onclick}>
	<svg>
		<use href={VokiTypeUtils.icon(type)} />
	</svg>
	<p>{VokiTypeUtils.name(type)}</p>
	<div class="indicator" class:selected={isSelected}>
		<span />
	</div>
</div>

<style>
	.card {
		display: grid;
		justify-content: center;
		width: 11rem;
		height: 11rem;
		padding: 0.5rem 0;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-md);
		transition: all 0.16s ease-in;
		cursor: pointer;
		place-items: center center;
		grid-template-rows: auto 1fr auto;
	}

	.card:hover {
		transform: scale(1.06);
	}

	.card > :global(svg) {
		height: 3.5rem;
		margin-top: 1rem;
		color: var(--primary);
		aspect-ratio: 1/1;
		stroke-width: 1.7;
	}

	.card > p {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 500;
		cursor: inherit;
	}

	.indicator {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 1.675rem;
		border: 0.1875rem solid var(--muted);
		border-radius: 1.5rem;
		background-color: var(--muted);
		transition: inherit;
		aspect-ratio: 1/1;
	}

	.card:hover .indicator:not(.selected) {
		border-color: var(--accent);
		background-color: var(--accent);
	}

	.indicator.selected {
		border-color: var(--primary) !important;
	}

	.indicator span {
		display: block;
		height: 70%;
		aspect-ratio: 1/1;
		transition: inherit;
	}

	.indicator.selected span {
		border-radius: 1.5rem;
		background-color: var(--primary);
	}
</style>
