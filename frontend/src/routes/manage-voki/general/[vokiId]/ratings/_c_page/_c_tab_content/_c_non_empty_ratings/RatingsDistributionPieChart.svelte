<script lang="ts">
	import type { VokiRatingValue } from '$lib/ts/voki';
	import type { RatingValueToCountType } from '../../../types';

	interface Props {
		distribution: RatingValueToCountType;
	}

	let { distribution }: Props = $props();
	const ORDER: VokiRatingValue[] = [1, 2, 3, 4, 5];

	const total = $derived.by(() => {
		let sum = 0;
		for (const v of ORDER) {
			sum += distribution[v];
		}
		return sum;
	});

	function polarToCartesian(cx: number, cy: number, r: number, angleDeg: number) {
		const a = ((angleDeg - 90) * Math.PI) / 180;
		return { x: cx + r * Math.cos(a), y: cy + r * Math.sin(a) };
	}

	function describeWedge(cx: number, cy: number, r: number, startDeg: number, endDeg: number) {
		const sweep = endDeg - startDeg;
		if (sweep >= 360) {
			return `M ${cx} ${cy - r} a ${r} ${r} 0 1 1 0 ${2 * r} a ${r} ${r} 0 1 1 0 ${-2 * r}`;
		}
		const start = polarToCartesian(cx, cy, r, startDeg);
		const end = polarToCartesian(cx, cy, r, endDeg);
		const largeArc = sweep > 180 ? 1 : 0;

		return `M ${cx} ${cy} L ${start.x} ${start.y} A ${r} ${r} 0 ${largeArc} 1 ${end.x} ${end.y} Z`;
	}

	const slices = $derived.by(() => {
		const cx = 50;
		const cy = 50;
		const r = 46;

		let acc = -90;

		return ORDER.map((value) => {
			const count = distribution[value];
			const pct = count / total;
			const span = pct * 360;

			const start = acc;
			const end = acc + span;
			acc = end;

			return {
				value,
				count,
				pct,
				path: describeWedge(cx, cy, r, start, end)
			};
		});
	});
	const highlightOnHover = $derived(slices.filter((s) => s.count > 0).length > 2);
</script>

<div class="root" class:hov-highlight={highlightOnHover}>
	<svg class="chart" viewBox="0 0 100 100" role="img" aria-label="Rating distribution">
		<g class="slices">
			{#each slices as s (s.value)}
				{#if s.count > 0}
					<path class={`slice highlight-val-${s.value} fill-color-${s.value}`} d={s.path} />
				{/if}
			{/each}
		</g>
	</svg>

	<div class="legend">
		{#each slices as s (s.value)}
			<div class="legend-row highlight-val-{s.value}">
				<div class="stars" title={`${s.value} star${s.value === 1 ? '' : 's'}`}>
					{#each Array.from({ length: s.value }) as _}
						<svg class="fill-color-{s.value}"><use href="#common-star-icon" /></svg>
					{/each}
				</div>
				<span class="legend-value">{s.count}</span>
				<span class="legend-pct">({Math.round(s.pct * 100)}%)</span>
			</div>
		{/each}
	</div>
</div>

<style>
	.root {
		display: flex;
		display: grid;
		align-items: center;
		gap: 2rem;
		padding: 0.5rem 2rem;
		border-radius: 1rem;
		background-color: var(--back);
		box-shadow: var(--shadow-xs);
		grid-template-columns: 1fr auto;
		--slice-5: var(--primary-hov);
		--slice-4: color-mix(in srgb, var(--primary-hov) 80%, var(--back));
		--slice-3: color-mix(in srgb, var(--primary-hov) 60%, var(--back));
		--slice-2: color-mix(in srgb, var(--primary-hov) 40%, var(--back));
		--slice-1: color-mix(in srgb, var(--primary-hov) 20%, var(--back));
	}

	.chart {
		display: block;
		width: 100%;
		aspect-ratio: 1/1;
	}

	.slice {
		transition: transform 0.12s ease-in-out;
		transform-origin: center;
	}

	.fill-color-1 {
		fill: var(--slice-1);
	}

	.fill-color-2 {
		fill: var(--slice-2);
	}

	.fill-color-3 {
		fill: var(--slice-3);
	}

	.fill-color-4 {
		fill: var(--slice-4);
	}

	.fill-color-5 {
		fill: var(--slice-5);
	}

	.legend {
		display: flex;
		flex-direction: column-reverse;
		gap: 0.75rem;
		width: fit-content;
	}

	.legend-row {
		display: grid;
		grid-template-columns: auto 1fr auto;
		align-items: center;
		column-gap: 0.5rem;
		padding: 0.125rem 0.5rem;
		text-align: right;
		border-radius: 0.5rem;
	}

	.stars {
		display: grid;
		--star-size: 1.5rem;
		grid-template-columns: repeat(5, var(--star-size));
		margin-right: 0.5rem;
	}
	.stars > svg {
		width: 100%;
		aspect-ratio: 1/1;
		stroke-width: 0;
	}

	.legend-value {
		color: var(--muted-foreground);
		font-variant-numeric: tabular-nums;
		font-size: 1.25rem;
		cursor: default;
	}

	.legend-pct {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-style: italic;
		cursor: default;
		text-align: left;
		width: 2.5rem;
	}
	.hov-highlight:has(.highlight-val-5:hover) .slice.highlight-val-5 {
		transform: scale(1.04);
	}
	.hov-highlight:has(.highlight-val-5:hover) .legend-row.highlight-val-5 {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.hov-highlight:has(.highlight-val-4:hover) .slice.highlight-val-4 {
		transform: scale(1.04);
	}
	.hov-highlight:has(.highlight-val-4:hover) .legend-row.highlight-val-4 {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.hov-highlight:has(.highlight-val-3:hover) .slice.highlight-val-3 {
		transform: scale(1.04);
	}
	.hov-highlight:has(.highlight-val-3:hover) .legend-row.highlight-val-3 {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.hov-highlight:has(.highlight-val-2:hover) .slice.highlight-val-2 {
		transform: scale(1.04);
	}
	.hov-highlight:has(.highlight-val-2:hover) .legend-row.highlight-val-2 {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.hov-highlight:has(.highlight-val-1:hover) .slice.highlight-val-1 {
		transform: scale(1.04);
	}
	.hov-highlight:has(.highlight-val-1:hover) .legend-row.highlight-val-1 {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow);
	}
</style>
