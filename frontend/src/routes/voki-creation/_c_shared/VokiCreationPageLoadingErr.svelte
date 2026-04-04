<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ErrUtils, type Err } from '$lib/ts/err';
	import { PublishedVokisStore } from '$lib/ts/stores/published-vokis-store.svelte';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';
	import VokiAlreadyPublishedMessage from './_c_page_loading_err/VokiAlreadyPublishedMessage.svelte';
	import { onMount } from 'svelte';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
	let componentState:
		| { type: 'additional-checks' }
		| { type: 'voki-published'; voki: PublishedVokiBriefInfo }
		| { type: 'unable-to-load' } = $state({ type: 'unable-to-load' });

	onMount(() => {
		const isVokiNotFoundAsDraft = errs.some((err) => ErrUtils.isWithVokiNotFoundCode(err));
		if (isVokiNotFoundAsDraft) {
			componentState = { type: 'additional-checks' };
			PublishedVokisStore.FetchOne(vokiId)
				.then((response) => {
					componentState = response.isSuccess
						? { type: 'voki-published', voki: response.data }
						: { type: 'unable-to-load' };
				})
				.catch(() => (componentState = { type: 'unable-to-load' }));
		} else {
			componentState = { type: 'unable-to-load' };
		}
	});
</script>

<div class="container">
	{#if componentState.type === 'voki-published'}
		<VokiAlreadyPublishedMessage voki={componentState.voki} />
	{:else if componentState.type === 'additional-checks'}
		<div class="is-published-check-loading">Something went wrong. We make additional checks</div>
	{:else if componentState.type === 'unable-to-load'}
		<h1>Unable to load page data</h1>
		<DefaultErrBlock errList={errs} />
	{:else}
		<div>Something went wrong</div>
	{/if}
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		width: 60rem;
		margin: 4rem auto;
	}

	.is-published-check-loading {
		animation: fade-in-from-zero-with-delay 1s ease-in;
	}
</style>
