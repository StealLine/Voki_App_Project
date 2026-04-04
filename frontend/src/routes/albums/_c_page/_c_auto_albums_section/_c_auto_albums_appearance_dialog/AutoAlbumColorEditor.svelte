<script lang="ts">
	import { watch } from 'runed';
	import type { AutoAlbumsColorsPair } from '../../../types';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';

	interface Props {
		title: string;
		descriptionVokiAction: string;
		iconId: string;
		colors: AutoAlbumsColorsPair;
	}

	let { title, descriptionVokiAction, iconId, colors = $bindable() }: Props = $props();

	let useTwoColors = $state(colors.mainColor !== colors.secondaryColor);

	watch(
		() => colors.mainColor,
		() => {
			if (!useTwoColors) {
				colors.secondaryColor = colors.mainColor;
			}
		}
	);

	watch(
		() => useTwoColors,
		() => {
			if (!useTwoColors) {
				colors.secondaryColor = colors.mainColor;
			}
		}
	);
</script>

<article class="album-editor">
	<header class="header">
		<svg
			class="album-icon"
			aria-hidden="true"
			style={`--icon-color-1: ${colors.mainColor}; --icon-color-2: ${colors.secondaryColor};`}
		>
			<use href={iconId} />
		</svg>

		<div class="title-block">
			<h3 class="title">{title}</h3>
			<p class="subtitle">Albums with Voki you <br />{descriptionVokiAction}</p>
		</div>
	</header>

	<div class="fields">
		<label class="field unselectable">
			<span class="label">Main color</span>
			<div class="input-row">
				<input type="color" bind:value={colors.mainColor} />
				<input class="hex-input" type="text" bind:value={colors.mainColor} spellcheck="false" />
			</div>
		</label>
		<label class="use-two-colors">
			<DefaultCheckBox bind:checked={useTwoColors} />
			<span class="use-two-colors-label">Use secondary color</span>
		</label>
		<label class="field" class:hide={!useTwoColors}>
			<span class="label unselectable">Secondary color</span>
			<div class="input-row">
				<input type="color" bind:value={colors.secondaryColor} disabled={!useTwoColors} />
				<input
					class="hex-input"
					type="text"
					bind:value={colors.secondaryColor}
					spellcheck="false"
					disabled={!useTwoColors}
				/>
			</div>
		</label>
	</div>
</article>

<style>
	.album-editor {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
		width: 15rem;
	}

	.header {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.5rem;
	}

	.album-icon {
		width: 4rem;
		height: 4rem;
		stroke-width: 1.675;
	}

	.title-block {
		display: flex;
		flex-direction: column;
		gap: 0.125rem;
	}

	.title {
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 550;
		white-space: nowrap;
	}

	.subtitle {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		white-space: nowrap;
	}

	.use-two-colors {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		color: var(--secondary-foreground);
		font-size: 0.9rem;
	}

	.use-two-colors-label {
		user-select: none;
	}

	.fields {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.field {
		display: flex;
		flex-direction: column;
		gap: 0.375rem;
		transition: opacity 0.08s ease;
	}

	.field.hide {
		opacity: 0;
	}

	.label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
	}

	.input-row {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.5rem;
	}

	input[type='color'] {
		width: 2rem;
		height: 2rem;
		padding: 0;
		border: none;
		background-color: var(--back);
		transition: transform 0.08s ease-in-out;
		cursor: pointer;
		-webkit-appearance: none;
		appearance: none;
		overflow: hidden;
	}

	input[type='color']::-webkit-color-swatch-wrapper {
		padding: 0;
		border: none;
		border-radius: 0.5rem;
	}

	input[type='color']::-webkit-color-swatch {
		border: none;
		border-radius: 0.5rem;
	}

	input[type='color']::-moz-color-swatch {
		border: none;
		border-radius: 0.5rem;
	}

	input[type='color']:disabled {
		opacity: 0.6;
		cursor: default;
	}

	.hex-input {
		min-width: 0;
		padding: 0.3rem 0.75rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.5rem;
		background-color: var(--back);
		color: var(--text);
		font-size: 1rem;
		font-family: monospace;
		outline: none;
	}

	.hex-input:focus {
		border-color: var(--primary);
		box-shadow: 0 0 0 0.08rem var(--accent);
	}

	.hex-input:disabled {
		opacity: 0.6;
		cursor: default;
	}
</style>
