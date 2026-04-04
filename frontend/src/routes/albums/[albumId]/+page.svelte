<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import AlbumPageFilterAndSort from '../_c_pages_shared/AlbumPageFilterAndSort.svelte';
	import type { PageProps } from './$types';
	import AlbumPageHeader from '../_c_pages_shared/AlbumPageHeader.svelte';
	import { AlbumPageState } from './album-page-state.svelte';
	import { toast } from 'svelte-sonner';
	import AlbumEmptyMessage from '../_c_pages_shared/AlbumEmptyMessage.svelte';
	import NoVokisInAlbumMatchFilterMessage from '../_c_pages_shared/NoVokisInAlbumMatchFilterMessage.svelte';
	import VokiInAlbumItemContextMenu from './_c_page/VokiInAlbumItemContextMenu.svelte';

	let { data }: PageProps = $props();
	const pageState = new AlbumPageState(
		data.response.isSuccess ? data.response.data.vokiIds : [],
		(e, voki) => {
			if (contextMenu) {
				contextMenu.open(e, voki);
			} else {
				toast.error('Could not open context menu');
			}
		}
	);
	let contextMenu: VokiInAlbumItemContextMenu | null = $state(null);
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load album"
		additionalParams={[{ name: 'albumId', value: data.albumId }]}
	/>
{:else}
	<VokiInAlbumItemContextMenu
		bind:this={contextMenu}
		albumId={data.albumId!}
		removeVokiFromAlbumInParent={(v) => pageState.removeVokiFromAlbum(v)}
	/>
	<AlbumPageHeader
		content={{
			type: 'user',
			icon: {
				href: data.response.data.icon,
				mainColor: data.response.data.mainColor,
				secondaryColor: data.response.data.secondaryColor
			},
			albumName: data.response.data.name
		}}
	/>

	{#if pageState.isInitialListEmpty()}
		<AlbumEmptyMessage
			title="This album doesn’t have any Vokis yet"
			subtitle="Add your first Voki to start building your collection"
		/>
	{:else}
		<AlbumPageFilterAndSort
			onVokiTypeClick={(t) => pageState.toggleTypeFilter(t)}
			chooseSortOption={(o) => pageState.chooseSortOption(o)}
			sortOptions={pageState.allSortOptions}
			chosenVokiTypes={pageState.filterAndSort.chosenVokiTypes}
			currentSortOption={pageState.filterAndSort.currentSortOption}
		/>
		{#if pageState.sortedAndFilteredVokis().length === 0}
			<NoVokisInAlbumMatchFilterMessage />
		{:else}
			<VokiItemsGridContainer>
				{#each pageState.sortedAndFilteredVokis() as voki}
					<VokiItemView state={voki} />
				{/each}
			</VokiItemsGridContainer>
		{/if}
	{/if}
{/if}
