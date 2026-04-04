<script lang="ts">
	import type { PageProps } from './$types';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';
	import {
		VokiItemViewUtils,
		type VokiItemViewOkStateProps
	} from '$lib/components/voki_item/_c_voki_item/voki-item';
	import CatalogVokiItemContextMenu from './catalog/_c_page/CatalogVokiItemContextMenu.svelte';
	import { toast } from 'svelte-sonner';

	let { data }: PageProps = $props();
	let contextMenu = $state<CatalogVokiItemContextMenu>()!;
	function assembleVokiItemStateData(voki: PublishedVokiBriefInfo): VokiItemViewOkStateProps {
		return VokiItemViewUtils.briefInfoToVokiItemOkStateProps(
			voki,
			`/catalog/${voki.id}`,
			(mEvent: MouseEvent) => contextMenu.open(mEvent, voki)
		);
	}
	let vokisToView = $state<PublishedVokiBriefInfo[]>(data.isSuccess ? data.data.vokis : []);
	function removeVokiFromList(voki: PublishedVokiBriefInfo) {
		vokisToView = vokisToView.filter((v) => v.id !== voki.id);
	}
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load vokis catalog" />
{:else if data.data.vokis.length === 0}
	<h1>Voki catalog is empty</h1>
{:else}
	<CatalogVokiItemContextMenu
		bind:this={contextMenu}
		removeVokiFromListInParent={removeVokiFromList}
		openReportVokiDialog={() => toast.error('Not implemented')}
	/>
	<VokiItemsGridContainer>
		{#each vokisToView as voki}
			<VokiItemView
				state={{
					name: 'ok',
					data: assembleVokiItemStateData(voki)
				}}
			/>
		{/each}
	</VokiItemsGridContainer>
{/if}
