<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import { onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../_c_shared/MyVokisPageInitialLoading.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../_c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';
	import { MyPublishedVokisPageState } from './my-published-vokis-page-state.svelte';
	import { toast } from 'svelte-sonner';
	import BaseContextMenu from '$lib/components/context_menus/BaseContextMenu.svelte';

	const pageState = new MyPublishedVokisPageState((e, voki) => {
		// if (contextMenu) {
		// 	contextMenu.open(e, voki);
		// } else {
		// 	toast.error('Could not open context menu');
		// }
	});

	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => pageState.forceRefetch(),
			invitesPage: {
				exists: false
			}
		});
	});
</script>

{#if pageState.publishedVokiIds.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your published vokis" />
{:else if pageState.publishedVokiIds.state === 'errs'}
	<DefaultErrBlock errList={pageState.publishedVokiIds.errs} />
{:else if pageState.publishedVokiIds.state === 'loaded'}
	{#if pageState.publishedVokiIds.vokiIds.length === 0}
		<h1>You don't have any published vokis</h1>
	{:else}
		<VokiItemsGridContainer>
			{#each pageState.publishedVokiIds.vokiIds as vokiId}
				<VokiItemView state={pageState.getVokiViewItemState(vokiId)} />
			{/each}
		</VokiItemsGridContainer>
	{/if}
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}
