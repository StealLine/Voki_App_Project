<script lang="ts">
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import type { Err } from '$lib/ts/err';
	import { VokiTypeUtils, type VokiType } from '$lib/ts/voki-type';
	import { getErrsViewDialogOpenFunction } from '../../_c_layout/_ts_layout_contexts/errs-view-dialog-context';

	export type VokiCreationHeaderVokiName =
		| { state: 'ok'; value: string }
		| { state: 'loading' }
		| { state: 'errs'; errs: Err[]; reload: () => void };
	interface Props {
		vokiName: VokiCreationHeaderVokiName;
		vokiType: VokiType;
	}

	let { vokiName, vokiType }: Props = $props();
	const openErrsViewDialog = getErrsViewDialogOpenFunction();
</script>

<div class="creation-header-container">
	{#if vokiName.state === 'errs'}
		<label class="could-not-load-label" onclick={() => openErrsViewDialog(vokiName.errs)}
			>Could not load voki name
			<svg><use href="#common-info-icon" /> </svg>
		</label>
		<ReloadButton onclick={() => vokiName.reload()} />
	{:else}
		Creation of the
		{#if vokiName.state === 'ok'}
			<label class="voki-name">{vokiName.value}</label>
		{:else if vokiName.state === 'loading'}
			<div class="loader"></div>
		{/if}
		{VokiTypeUtils.name(vokiType).toLocaleLowerCase()} voki
	{/if}
</div>

<style>
	.creation-header-container {
		display: flex;
		justify-content: center;
		align-items: center;
		margin-bottom: 0.75rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 440;
	}

	.creation-header-container:has(.could-not-load-label) {
		gap: 2rem;
		padding-right: 1rem;
	}

	.creation-header-container > :global(.reload-btn) {
		margin: 0;
	}

	.could-not-load-label > svg {
		width: 1.25rem;
		height: 1.25rem;
		padding: 0;
		margin-bottom: 0.125rem;
		margin-left: 0.25rem;
		color: inherit;
		stroke-width: 2.25;
		vertical-align: middle;
	}

	.could-not-load-label {
		padding: 0.125rem 1rem;
		border-radius: 100vw;
		background-color: var(--muted);
	}

	.could-not-load-label:hover,
	.could-not-load-label:active,
	.could-not-load-label:focus {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.creation-header-container .voki-name {
		display: inline-block;
		max-width: calc(50vw - 4rem);
		margin: 0 0.25rem;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--primary);
		font-weight: 500;
	}

	.loader {
		position: relative;
		width: 14rem;
		height: 1.25rem;
		margin: 0 0.5rem;
		border-radius: 100vw;
		background: var(--secondary);
	}

	.loader::after {
		position: absolute;
		background: linear-gradient(
			-40deg,
			transparent 0%,
			transparent 40%,
			color-mix(in srgb, var(--secondary-foreground) 5%, var(--muted) 15%) 50%,
			transparent 60%,
			transparent 100%
		);
		animation: shimmer 1s infinite;
		content: '';
		inset: 0;
	}

	@keyframes shimmer {
		0% {
			transform: translateX(-35%);
		}

		100% {
			transform: translateX(35%);
		}
	}
</style>
