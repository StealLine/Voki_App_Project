<script lang="ts">
	import ContextMenuWithActions, {
		type ActionsContextMenuActionsContent,
		type ActionsContextMenuMessageContent
	} from '$lib/components/context_menus/ContextMenuWithActions.svelte';
	import { ApiAlbums, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';
	import { toast } from 'svelte-sonner';

	interface Props {
		removeVokiFromAlbumInParent: (voki: PublishedVokiBriefInfo) => void;
		albumId: string;
	}
	let { removeVokiFromAlbumInParent, albumId }: Props = $props();
	let contextMenu = $state<ContextMenuWithActions>()!;
	export function open(event: MouseEvent, voki: PublishedVokiBriefInfo) {
		currentVoki = voki;
		contextMenu.open(event.x, event.y, 8, -5);
	}
	let currentVoki: PublishedVokiBriefInfo | undefined = $state<PublishedVokiBriefInfo>();
	let menuContent: ActionsContextMenuMessageContent | ActionsContextMenuActionsContent = $derived(
		currentVoki
			? {
					type: 'actions',
					items: [
						{
							label: 'Open',
							iconHref: '#context-menu-open-icon',
							action: {
								isLink: true,
								href: `/catalog/${currentVoki!.id}`
							},
							type: 'default'
						},
						{
							label: 'Copy link',
							iconHref: '#context-menu-copy-link-icon',
							action: {
								isLink: false,
								onclick: () => {
									const link = `${location.origin}/catalog/${currentVoki!.id}`;
									navigator.clipboard.writeText(link).then(
										() => {
											const namePrev =
												currentVoki!.name.length > 17
													? currentVoki!.name.slice(0, 15) + '...'
													: currentVoki!.name;
											toast.success(`Copied link to '${namePrev}' to clipboard`);
										},
										() => toast.error('Could not copy link')
									);
									contextMenu.close();
								}
							},
							type: 'default'
						},
						{
							label: 'Remove from album',
							iconHref: '#context-menu-remove-from-album-icon',
							action: {
								isLink: false,
								onclick: () => removeVokiFromAlbum(currentVoki!)
							},
							type: 'red'
						}
					]
				}
			: {
					type: 'message',
					message: 'No voki selected',
					iconHref: '#common-crossed-circle-icon'
				}
	);
	async function removeVokiFromAlbum(voki: PublishedVokiBriefInfo) {
		const response = await ApiAlbums.fetchVoidResponse(
			`/albums/${albumId}/remove-voki`,
			RJO.PATCH({ vokiId: voki.id })
		);
		if (response.isSuccess) {
			removeVokiFromAlbumInParent(voki);
            contextMenu.close();
		} else {
			toast.error('Could not remove voki from album');
		}
	}
</script>

<ContextMenuWithActions bind:this={contextMenu} content={menuContent} />
