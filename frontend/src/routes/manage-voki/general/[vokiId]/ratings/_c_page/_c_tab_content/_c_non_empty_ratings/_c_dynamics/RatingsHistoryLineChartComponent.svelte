<script lang="ts">
	interface Point {
		xLabel: string;
		yValue: number;
	}

	interface Props {
		data: Point[];
		title: string;
		color?: string;
		yMin?: number;
		yMax?: number;
		anyInputError: boolean;
	}

	let { data, title, color = 'var(--primary)', yMin, yMax, anyInputError }: Props = $props();

	const effectiveYMin = $derived(yMin ?? Math.min(...data.map((d) => d.yValue), 0));
	const effectiveYMax = $derived(
		yMax ?? Math.max(...data.map((d) => d.yValue), Math.max(1, effectiveYMin + 1))
	);

	const width = 1000;
	const height = 300;
	const paddingX = 70;
	const paddingY = 40;

	const graphWidth = width - 2 * paddingX;
	const graphHeight = height - 2 * paddingY;

	const points = $derived.by(() => {
		if (data.length === 0) return [];
		if (data.length === 1) {
			return [
				{
					...data[0],
					cx: paddingX + graphWidth / 2,
					cy:
						paddingY +
						graphHeight -
						((data[0].yValue - effectiveYMin) / (effectiveYMax - effectiveYMin || 1)) * graphHeight
				}
			];
		}

		return data.map((d, i) => {
			const x = paddingX + (i / (data.length - 1)) * graphWidth;
			const yRange = effectiveYMax - effectiveYMin || 1;
			const yPercent = (d.yValue - effectiveYMin) / yRange;
			const y = paddingY + graphHeight - yPercent * graphHeight;
			return { ...d, cx: x, cy: y };
		});
	});

	const polylinePoints = $derived(points.map((p) => `${p.cx},${p.cy}`).join(' '));

	const yTicks = $derived.by(() => {
		const count = 5;
		const ticks = [];
		const range = effectiveYMax - effectiveYMin || 1;
		for (let i = 0; i <= count; i++) {
			const val = effectiveYMin + (range * i) / count;
			const y = paddingY + graphHeight - (i / count) * graphHeight;
			ticks.push({ value: Math.round(val * 10) / 10, y });
		}
		return ticks;
	});

	const xTicks = $derived.by(() => {
		if (points.length <= 10) return points;
		const step = Math.ceil(points.length / 10);
		return points.filter((_, i) => i % step === 0 || i === points.length - 1);
	});

	let hoveredPoint = $state<(Point & { cx: number; cy: number }) | null>(null);
</script>

<div class="chart-container">
	<h3 class="chart-title">{title}</h3>
	<div class="svg-wrapper">
		<svg viewBox="0 0 {width} {height}" class="line-chart" role="img" aria-label={title}>
			{#if anyInputError}
				<text
					x={width / 2}
					y={height / 2}
					class="svg-error-message"
					text-anchor="middle"
					alignment-baseline="middle"
				>
					Invalid input values
				</text>
			{:else}
				<g class="grid">
					{#each yTicks as tick}
						<line x1={paddingX} y1={tick.y} x2={width - paddingX} y2={tick.y} class="grid-line" />
						<text
							x={paddingX - 10}
							y={tick.y}
							class="y-label"
							alignment-baseline="middle"
							text-anchor="end">{tick.value}</text
						>
					{/each}
				</g>

				{#if points.length > 1}
					<polyline points={polylinePoints} class="data-line" style="stroke: {color};" />
				{/if}

				{#each points as p}
					<circle
						cx={p.cx}
						cy={p.cy}
						r={hoveredPoint === p ? 6 : 4}
						class="data-point"
						style="fill: {color};"
						onmouseenter={() => (hoveredPoint = p)}
						onmouseleave={() => (hoveredPoint = null)}
						role="graphics-symbol"
						tabindex="0"
					/>
				{/each}

				<g class="x-axis">
					<line
						x1={paddingX}
						y1={height - paddingY}
						x2={width - paddingX}
						y2={height - paddingY}
						class="axis-line"
					/>
					{#each xTicks as p}
						<text x={p.cx} y={height - paddingY + 20} class="x-label" text-anchor="middle"
							>{p.xLabel}</text
						>
					{/each}
				</g>

				{#if hoveredPoint}
					<line
						x1={hoveredPoint.cx}
						y1={paddingY}
						x2={hoveredPoint.cx}
						y2={height - paddingY}
						class="hover-line"
					/>
				{/if}
			{/if}
		</svg>

		{#if hoveredPoint && !anyInputError}
			<div
				class="tooltip"
				style="left: {(hoveredPoint.cx / width) * 100}%; top: {(hoveredPoint.cy / height) * 100}%;"
			>
				<div><strong>{hoveredPoint.xLabel}</strong></div>
				<div>{hoveredPoint.yValue}</div>
			</div>
		{/if}
	</div>
</div>

<style>
	.chart-container {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 1rem;
		position: relative;
	}

	.chart-title {
		margin: 0;
		font-size: 1.25rem;
		font-weight: 600;
		color: var(--text);
		text-align: left;
	}

	.svg-wrapper {
		width: 100%;
		position: relative;
	}

	.line-chart {
		width: 100%;
		height: auto;
		display: block;
		overflow: visible;
	}

	.data-line {
		fill: none;
		stroke-width: 3;
		stroke-linejoin: round;
		stroke-linecap: round;
	}

	.data-point {
		stroke: var(--back);
		stroke-width: 2;
		cursor: pointer;
		transition: r 0.15s ease-out;
	}

	.data-point:focus {
		outline: none;
	}

	.grid-line {
		stroke: var(--border);
		stroke-width: 1;
		stroke-dasharray: 4 4;
	}

	.axis-line {
		stroke: var(--border);
		stroke-width: 2;
	}

	.y-label,
	.x-label {
		fill: var(--muted-foreground);
		font-size: 14px;
		font-family: inherit;
	}

	.hover-line {
		stroke: var(--muted-foreground);
		stroke-width: 1;
		stroke-dasharray: 4 4;
		pointer-events: none;
	}

	.tooltip {
		position: absolute;
		transform: translate(-50%, -120%);
		background: var(--popover);
		color: var(--popover-foreground);
		padding: 0.5rem 0.75rem;
		border-radius: var(--radius);
		box-shadow: var(--shadow-sm);
		pointer-events: none;
		font-size: 0.875rem;
		white-space: nowrap;
		z-index: 10;
		border: 1px solid var(--border);
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.25rem;
	}

	.svg-error-message {
		fill: var(--destructive);
		font-size: 1.15rem;
		font-weight: 600;
		font-family: inherit;
	}
</style>
