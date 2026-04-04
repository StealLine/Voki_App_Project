<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import VokiPageTabSectionLabel from '../_c_tabs_shared/VokiPageTabSectionLabel.svelte';

	interface Props {
		averageRating: number;
		count: number;
		isOutdated: boolean;
		reloadOutdated: () => Promise<void>;
	}
	let { averageRating, count, isOutdated, reloadOutdated }: Props = $props();
	async function onReloadButtonClick() {
		loading = true;
		await reloadOutdated();
		loading = false;
	}
	let loading = $state(false);
</script>

<div class="avg-container">
	<VokiPageTabSectionLabel fieldName="Average rating:" />
	<div class="value-container">
		{Math.round(averageRating * 100) / 100}<svg class="star"><use href="#common-star-icon" /></svg
		>/5
	</div>
	{#if isOutdated}
		<svg
			class="reload-btn"
			onclick={() => {
				onReloadButtonClick();
			}}><use href="#common-reload-icon" /></svg
		>
	{/if}
	<VokiPageTabSectionLabel
		fieldName="({count} total rating{count === 1 ? '' : 's'})"
		class="ratings-count"
	/>
	{#if loading}
		<div class="loader-backdrop">
			<LinesLoader sizeRem={1.6} strokePx={2} color="var(--text)" />
		</div>
	{/if}
</div>

<style>
	.avg-container {
		position: relative;
		z-index: 1;
		display: flex;
		flex-direction: row;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 500;
		cursor: default;
	}

	.value-container {
		display: flex;
		align-items: center;
		margin: 0 0.5rem;
	}

	.reload-btn {
		width: 1.5rem;
		height: 1.5rem;
		padding: 0.25rem;
		margin-left: 1rem;
		border-radius: 1rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		transition: all 0.2s ease-out;
		transform: rotate(60deg);
		cursor: pointer;
		stroke-width: 2.23;
	}

	.reload-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		transform: rotate(120deg);
	}

	.reload-btn:active {
		transform: scale(0.96);
		transform: rotate(360deg);
	}

	.star {
		width: 1.5rem;
		height: 1.5rem;
		margin-bottom: 0.125rem;
		margin-left: 0.125rem;
		stroke-width: 0;
		fill: var(--primary);
	}

	.avg-container > :global(.ratings-count) {
		margin-left: auto;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.loader-backdrop {
		position: absolute;
		top: 50%;
		left: 50%;
		z-index: 2;
		display: flex;
		justify-content: center;
		align-items: center;
		width: calc(100% + 1rem);
		height: calc(100% + 0.5rem);
		border-radius: 0.5rem;
		background-color: var(--secondary);
		opacity: 0.7;
		box-shadow: var(--shadow-xs);
		transform: translate(-50%, -50%);
		animation:fade-in-from-zero-with-delay 1s ease;
	}

	.loader-backdrop > :global(.container) {
		opacity: inherit;
	}

	
</style>
