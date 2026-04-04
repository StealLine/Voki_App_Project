<script lang="ts">
	import { navigating, page } from '$app/state';
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './_c_layout/GeneralVokiCreationLayoutNavBar.svelte';
	import { type VokiCreationHeaderVokiName } from '../../_c_layout/VokiCreationVokiNameHeader.svelte';
	import { initVokiCreationPageContext } from '../../voki-creation-page-context';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import vokiAnswerTypesIconsSprite from '$lib/icons/general-voki-answer-types-icons.svg?raw';
	import generalVokiCreationIconsSprite from '$lib/icons/general-voki-creation-icons.svg?raw';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiCreationVokiNameHeader from '../../_c_layout/VokiCreationVokiNameHeader.svelte';

	const { children }: { children: Snippet } = $props();

	let vokiId = page.params.vokiId;
	let vokiName: VokiCreationHeaderVokiName = $state()!;
	async function fetchAndSetVokiName() {
		if (!vokiId) {
			vokiName = {
				state: 'errs',
				errs: [{ message: 'Voki id is not specified' }],
				reload: () => fetchAndSetVokiName()
			};
			return;
		}
		vokiName = { state: 'loading' };
		const response = await ApiVokiCreationGeneral.getVokiName(vokiId);
		if (response.isSuccess) {
			vokiName = { state: 'ok', value: response.data.vokiName };
		} else {
			vokiName = {
				state: 'errs',
				errs: response.errs,
				reload: () => fetchAndSetVokiName()
			};
		}
	}
	fetchAndSetVokiName();
	initVokiCreationPageContext(ApiVokiCreationGeneral, {
		get value() {
			return vokiName.state === 'ok' ? vokiName.value : undefined;
		},
		invalidate: fetchAndSetVokiName
	});
</script>

<div class="sprites">
	{@html vokiAnswerTypesIconsSprite}
	{@html generalVokiCreationIconsSprite}
</div>

<VokiCreationVokiNameHeader {vokiName} vokiType="General" />
<GeneralVokiCreationLayoutNavBar {vokiId} />

{#if navigating.type}
	<div class="loading fade-in-animation">
		<h1>Loading tab data</h1>
		<CubesLoader sizeRem={5} color="var(--primary)" />
	</div>
{:else}
	<div class="fade-in-animation">
		{@render children()}
	</div>
{/if}

<style>
	.sprites {
		display: none;
		width: 0;
		height: 0;
	}

	.loading {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
	}

	.loading h1 {
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 2px;
	}

	.fade-in-animation {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
</style>
