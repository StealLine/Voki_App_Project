<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../_c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../_c_shared/MyVokisPageInitialLoading.svelte';
	import { MyDraftVokisPageState } from './my-draft-vokis-page-state.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';

	import { page } from '$app/state';
	import { replaceState } from '$app/navigation';

	const pageState = new MyDraftVokisPageState();
	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => pageState.forceRefetch(),
			invitesPage: {
				exists: true,
				path: 'co-author-invites'
			}
		});
	});

	$effect(() => {
		if (page.data.vokiHighlight) {
			console.log('page.data.vokiHighlight', page.data.vokiHighlight);
			const cleanUrl = new URL(page.url);
			cleanUrl.searchParams.delete('vokiHighlight');
			replaceState(cleanUrl, {});
		}
	});
</script>

{#if pageState.draftVokiIds.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your draft vokis" />
{:else if pageState.draftVokiIds.state === 'errs'}
	<DefaultErrBlock errList={pageState.draftVokiIds.errs} />
{:else if pageState.draftVokiIds.state === 'loaded'}
	{#if pageState.draftVokiIds.vokiIds.length === 0}
		<h1>You don't have any draft vokis</h1>
	{:else}
		<VokiItemsGridContainer>
			{#each pageState.draftVokiIds.vokiIds as vokiId}
				<VokiItemView state={pageState.getVokiViewItemState(vokiId)} />
			{/each}
		</VokiItemsGridContainer>
	{/if}
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}
