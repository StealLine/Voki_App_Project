<script lang="ts">
	import { VokiTypeUtils } from '$lib/ts/voki-type';
	import type { VokiTypeWithSpecificData } from '../../../types';
	import VokiPageTabSectionLabel from '../_c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import GeneralVokiTypeSpecificDataContent from './_c_type_section_specific_data/GeneralVokiTypeSpecificDataContent.svelte';
	interface Props {
		typeWithData: VokiTypeWithSpecificData;
	}
	let { typeWithData }: Props = $props();
	let hideTypeSpecificData = $state(true);

	let iconElement = $state<SVGSVGElement>()!;

	function toggleDetails() {
		hideTypeSpecificData = !hideTypeSpecificData;
		if (iconElement) {
			iconElement.classList.remove('rotate-down', 'rotate-up');
			iconElement.classList.add(hideTypeSpecificData ? 'rotate-down' : 'rotate-up');
		}
	}
</script>

<div class="container">
	<div class="field-line">
		<VokiPageTabSectionLabel fieldName="Type:" />
		<div class="type-value">
			<span>{VokiTypeUtils.name(typeWithData.type)}</span>
		</div>
		<div class="show-more-btn" onclick={toggleDetails}>
			<svg bind:this={iconElement}><use href="#common-toggle-content-arrow" /></svg>
		</div>
	</div>
	<div class="type-specific-data" class:hidden={hideTypeSpecificData}>
		{#if typeWithData.type === 'General'}
			<GeneralVokiTypeSpecificDataContent data={typeWithData.typeSpecificData} />
		{:else}
			<p>Unsupported type</p>
		{/if}
	</div>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
	}

	.field-line {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin: 0;
		cursor: default;
	}

	.field-line .type-value {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 500;
	}

	.show-more-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.125rem 0.375rem;
		margin-left: 0.375rem;
		border-radius: 100vw;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.show-more-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.show-more-btn > svg {
		width: 1rem;
		height: 1rem;
		transition: transform 0.17s ease-in;
		transform-origin: center;
		cursor: pointer;
		stroke-width: 2.5;
	}

	.show-more-btn > :global(svg.rotate-down) {
		animation: rotate-down 0.3s ease-in-out forwards;
	}

	.show-more-btn > :global(svg.rotate-up) {
		animation: rotate-up 0.3s ease-in-out forwards;
	}

	.type-specific-data {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		height: max-content;
		margin: 0 0 0 1rem;
		opacity: 1;
		transition:
			all 0.4s ease,
			opacity 0.4s ease-out;
		interpolate-size: allow-keywords;
		overflow: hidden;
	}

	.type-specific-data.hidden {
		height: 0;
		opacity: 0;
	}

	@keyframes rotate-down {
		from {
			transform: rotate(0deg);
		}

		to {
			transform: rotate(-180deg);
		}
	}

	@keyframes rotate-up {
		from {
			transform: rotate(-180deg);
		}

		to {
			transform: rotate(0deg);
		}
	}
</style>
