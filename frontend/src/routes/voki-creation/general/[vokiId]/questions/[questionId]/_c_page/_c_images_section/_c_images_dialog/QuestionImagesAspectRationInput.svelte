<script lang="ts">
	import NumberSliderInput from '$lib/components/inputs/NumberSliderInput.svelte';

	let { aspectRatio = $bindable() }: { aspectRatio: { width: number; height: number } } = $props<{
		aspectRatio: {
			width: number;
			height: number;
		};
	}>();
	let defaultOptions = [
		{ width: 16, height: 9 },
		{ width: 4, height: 3 },
		{ width: 1, height: 1 },
		{ width: 3, height: 4 },
		{ width: 9, height: 16 }
	];
	let isCustom = $state<boolean>(
		!defaultOptions.some((o) => o.width === aspectRatio.width && o.height === aspectRatio.height)
	);
	function selectDefault(option: { width: number; height: number }) {
		isCustom = false;
		aspectRatio = option;
	}
	function onCustomClick() {
		if (isCustom) return;

		isCustom = true;

		const { width, height } = aspectRatio;

		if (width >= height) {
			const newWidth = Number((width / height).toFixed(2));
			aspectRatio = { width: newWidth, height: 1 };
		} else {
			const newHeight = Number((height / width).toFixed(2));
			aspectRatio = { width: 1, height: newHeight };
		}
	}
</script>

<div class="aspect-ratio-inputs-container">
	<div class="main-options unselectable">
		{#each defaultOptions as option}
			<button
				class="aspect-ratio-option"
				class:chosen={option.width === aspectRatio.width && option.height === aspectRatio.height}
				onclick={() => selectDefault(option)}
			>
				{option.width} : {option.height}
			</button>
		{/each}
		<button
			class="aspect-ratio-option custom-btn"
			class:chosen={isCustom}
			onclick={() => onCustomClick()}>Custom</button
		>
	</div>
	<div class="custom-inputs" class:open={isCustom}>
		<NumberSliderInput
			bind:value={aspectRatio.width}
			min={1}
			max={3}
			step={0.01}
			marks={[1, 2, 3]}
		/>
		<label>
			{aspectRatio.width} : {aspectRatio.height}
		</label>
		<NumberSliderInput
			bind:value={aspectRatio.height}
			min={1}
			max={3}
			step={0.01}
			marks={[1, 2, 3]}
		/>
	</div>
</div>

<style>
	.aspect-ratio-inputs-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.main-options {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: fit-content;
	}

	.aspect-ratio-option {
		width: 5rem;
		height: 2rem;
		border: none;
		border-radius: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		box-shadow: var(--shadow);
	}

	.aspect-ratio-option:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.aspect-ratio-option.chosen {
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-weight: 500;
	}

	.custom-btn {
		width: 8rem;
		margin-left: 1rem;
	}

	.custom-inputs {
		display: grid;
		align-items: center;
		gap: 1rem;
		height: 0;
		font-size: 0;
		opacity: 0;
		transition: all 0.2s ease-in;
		interpolate-size: allow-keywords;
		grid-template-columns: 1fr 8rem 1fr;
	}

	.custom-inputs:not(.open) > :global(*) {
		display: none;
	}

	.custom-inputs.open {
		height: auto;
		font-size: 1.25rem;
		opacity: 1;
	}

	.custom-inputs label {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.375rem;
		width: 100%;
		height: 2rem;
		margin-bottom: 1rem;
		border-radius: 2rem;
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 450;
	}
</style>
