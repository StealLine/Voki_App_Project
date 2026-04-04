<script lang="ts">
	import { watch } from 'runed';
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import type { AnswerDataColorOnly } from '../../../../types';
	import AnswerEditingBasicColorInput from './_c_shared/AnswerEditingBasicColorInput.svelte';
	interface Props {
		answer: AnswerDataColorOnly;
		onColorChange: (newColor: string) => void;
	}
	let { answer, onColorChange }: Props = $props();

	function normalizeForShades(v: string): string {
		let t = (v ?? '').trim();
		if (!t.startsWith('#')) t = '#' + t;
		return ColorUtils.isHex6(t) ? t.toUpperCase() : '#000000';
	}
	let base = $derived(normalizeForShades(answer.color));
	let shades = $derived([
		ColorUtils.adjustLightness(base, +21),
		ColorUtils.adjustLightness(base, +14),
		ColorUtils.adjustLightness(base, +7),
		base.toUpperCase(),
		ColorUtils.adjustLightness(base, -7),
		ColorUtils.adjustLightness(base, -14),
		ColorUtils.adjustLightness(base, -21)
	]);
</script>

<div class="answer-content">
	<AnswerEditingBasicColorInput bind:color={() => answer.color, onColorChange} />
	<div class="shades">
		{#each shades as shade}
			<div class="shade" style="

--shade:{shade}" onclick={() => onColorChange(shade)}></div>
		{/each}
	</div>
	<div class="divider"></div>
	<div class="presets">
		Choose from presets
		<div class="presets-list">
			{#each ColorUtils.colorPresets as p}
				<div class="preset" style="

--preset:{p}" onclick={() => onColorChange(p)}></div>
			{/each}
		</div>
	</div>
</div>

<style>
	.answer-content {
		display: flex;
		justify-content: space-between;
		align-items: center;
		width: 100%;
		height: 100%;
		padding: 0.5rem 2rem 0;
		overflow: hidden;
	}

	.divider {
		width: 0.125rem;
		height: calc(100% - 1rem);
		border-radius: 0.25rem;
		background-color: var(--muted);
	}

	.shades {
		display: flex;
		flex-direction: row;
		justify-content: center;
		height: fit-content;
		border-radius: 0.875rem;

		/* border: 0.125rem solid var(--secondary-foreground); */

		/* background-color: var(--secondary-foreground);
		 */
	}

	.shade {
		z-index: 1;
		width: 2.5rem;
		height: 7rem;
		background: var(--shade);
		transition: all 0.2s ease-out;
		cursor: pointer;
	}

	.shade:hover {
		z-index: 2;
		border-radius: 0.25rem;
		box-shadow: var(--shadow-md);
		transform: scaleY(1.14) scaleX(1.02);
	}

	.shade:first-child {
		border-radius: 0.75rem 0 0 0.75rem;
	}

	.shade:first-child:hover {
		border-radius: 0.75rem 0.25rem 0.25rem 0.75rem;
	}

	.shade:last-child {
		border-radius: 0 0.75rem 0.75rem 0;
	}

	.shade:last-child:hover {
		border-radius: 0.25rem 0.75rem 0.75rem 0.25rem;
	}

	.presets {
		display: flex;
		flex-direction: column;
		justify-content: center;
		gap: 0.5rem;
		width: fit-content;
		margin-bottom: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		text-align: center;
	}

	.presets-list {
		display: grid;
		gap: 0.5rem;
		grid-template-columns: 1fr 1fr 1fr 1fr;
		grid-template-rows: 1fr 1fr;
	}

	.preset {
		height: 2.5rem;
		border-radius: 0.5rem;
		background: var(--preset);
		box-shadow: var(--shadow-md), var(--shadow-xs);
		transition: all 0.12s ease-out;
		cursor: pointer;
		aspect-ratio: 1/1;
	}

	.preset:hover {
		transform: scale(1.1);
	}
</style>
