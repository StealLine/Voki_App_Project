<script lang="ts">
	let {
		min,
		max,
		step,
		value = $bindable<number>(min),
		marks = []
	}: {
		min: number;
		max: number;
		step: number;
		value: number;
		marks?: number[];
	} = $props<{
		min: number;
		max: number;
		step: number;
		value: number;
		marks?: number[];
	}>();
</script>

<div class="slider-container unselectable">
	<input class="slider" type="range" bind:value {min} {max} {step} />

	{#if marks.length > 0}
		<div class="slider-track">
			{#each marks as mark}
				<div
					onclick={() => (value = mark)}
					class="mark"
					class:active={mark == value}
					style="left: {((mark - min) / (max - min)) * 100}%"
				>
					<label class="mark-label">{mark}</label>
				</div>
			{/each}
		</div>
	{/if}
</div>

<style>
	.slider-container {
		position: relative;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		width: 100%;
	}

	.slider {
		z-index: 6;
		width: 100%;
		height: 0.375rem;
		box-sizing: border-box;
		margin: 0;
		border: none;
		border-radius: 0.125rem;
		background: var(--secondary);
		appearance: none;
		outline: none;
	}

	.slider::-webkit-slider-thumb {
		z-index: 15;
		width: 1rem;
		height: 1rem;
		border-radius: 50%;
		background: var(--primary);
		transition: background 0.2s ease;
		cursor: pointer;
		appearance: none;
	}

	.slider::-moz-range-thumb {
		z-index: 15;
		width: 1rem;
		height: 1rem;
		border-radius: 50%;
		background: var(--primary);
		cursor: pointer;
	}

	.slider-track {
		position: relative;
		z-index: 3;
		display: flex;
		justify-content: space-between;
		width: calc(100% - 0.75rem);
		height: 1rem;
		color: var(--secondary-foreground);
		font-size: 0.8rem;
	}

	.mark {
		position: absolute;
		top: 0.125rem;
		z-index: 1;
		align-items: center;
		width: 0.75rem;
		color: var(--secondary-foreground);
		font-weight: 450;
		transition: all 0.2s ease;
		cursor: pointer;
		flex: 0 0 auto;
	}

	.mark-label {
		position: absolute;
		left: 50%;
		display: block;
		color: inherit;
		font-weight: inherit;
		transform: translateX(-50%);
		cursor: inherit;
	}

	.mark:hover:not(.active) {
		color: var(--text-main);
		font-weight: 550;
		transform: scale(1.08) translateY(-0.125rem);
	}

	.mark.active {
		color: var(--primary);
		font-weight: 600;
	}
</style>
