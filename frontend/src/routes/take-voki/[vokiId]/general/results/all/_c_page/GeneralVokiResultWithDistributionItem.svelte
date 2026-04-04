<script lang="ts">
	import GeneralVokiResultPreviewImage from '../../_c_shared/GeneralVokiResultPreviewImage.svelte';
	import type { VokiResultWithDistributionPercent } from '../types';

	interface Props {
		result: VokiResultWithDistributionPercent;
		showDistribution: boolean;
	}
	let { result, showDistribution }: Props = $props();
</script>

<div class="result-item">
	<GeneralVokiResultPreviewImage resultImage={result.image} resultName={result.name} />
	<div class="item-body">
		<h3 class="result-name">{result.name}</h3>
		<div class="distribution">
			{#if showDistribution}
				<div
					class="bar"
					aria-valuemin="0"
					aria-valuemax="100"
					aria-valuenow={Math.round(result.distributionPercent)}
				>
					<div
						class="bar-fill"
						style={`width:${Math.max(0, Math.min(100, result.distributionPercent))}%`}
					/>
				</div>
				<label class="percent-label">{Math.round(result.distributionPercent * 10) / 10}%</label>
			{:else}
				<div class="distribution-hidden">Author decided to hide results distribution</div>
			{/if}
		</div>
	</div>
</div>

<style>
	.result-item {
		display: grid;
		grid-template-columns: 8rem 1fr;
		gap: 1rem;
		padding: 1rem;
		border-radius: calc(var(--radius) + 0.25rem);
		box-shadow: var(--shadow-md);
	}

	.item-body {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.result-name {
		margin: 0;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 600;
		text-indent: 0.5em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.distribution {
		display: grid;
		grid-template-columns: 1fr auto;
		align-items: center;
		gap: 0.125rem;
	}

	.bar {
		position: relative;
		width: 100%;
		height: 0.8rem;
		border-radius: 999rem;
		background: var(--secondary);
		box-shadow: inset var(--shadow-xs);
		overflow: hidden;
	}

	.bar-fill {
		height: 100%;
		border-radius: inherit;
		background: linear-gradient(
			90deg,
			color-mix(in srgb, var(--primary) 70%, var(--back) 30%),
			var(--primary)
		);
		box-shadow: var(--shadow);
		transition: width 220ms ease;
	}

	.percent-label {
		display: inline-block;
		min-width: 4.5rem;
		border-radius: 0.5rem;
		font-size: 1rem;
		font-weight: 550;
		text-align: center;
	}

	.distribution-hidden {
		width: fit-content;
		padding: 0.125rem 0.75rem;
		border-radius: 8rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 440;
		box-shadow: var(--shadow-xs);
	}
</style>
