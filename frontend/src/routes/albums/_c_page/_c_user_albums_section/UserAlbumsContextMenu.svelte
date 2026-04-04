<script lang="ts">
	import ContextMenuWithActions, {
		type ActionsContextMenuActionsContent,
		type ActionsContextMenuMessageContent,
	} from '$lib/components/context_menus/ContextMenuWithActions.svelte';
	import type { VokiAlbumPreviewData } from '../../types';

	interface Props {
		openEditAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openDeleteAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openCopyFromAnotherAlbumDialog: (album: VokiAlbumPreviewData) => void;
	}

	let { openEditAlbumDialog, openDeleteAlbumDialog, openCopyFromAnotherAlbumDialog }: Props =
		$props();
	let contextMenu = $state<ContextMenuWithActions>()!;

	export function open(event: MouseEvent, album: VokiAlbumPreviewData) {
		currentAlbum = album;
		contextMenu.open(event.x, event.y, 8, -5);
	}
	let currentAlbum: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
	let menuContent: ActionsContextMenuMessageContent | ActionsContextMenuActionsContent = $derived(
		currentAlbum
			? {
					type: 'actions',
					items: [
						{
							label: 'Open',
							iconHref: '#context-menu-open-icon',
							action: {
								isLink: true,
								href: `/albums/${currentAlbum!.id}`
							},
							type: 'default'
						},
						{
							label: 'Edit',
							iconHref: '#context-menu-edit-icon',
							action: {
								isLink: false,
								onclick: () => openEditAlbumDialog(currentAlbum!)
							},
							type: 'default'
						},
						{
							label: 'Copy from another album',
							iconHref: '#context-menu-copy-from-album-icon',
							action: {
								isLink: false,
								onclick: () => openCopyFromAnotherAlbumDialog(currentAlbum!)
							},
							type: 'default'
						},
						{
							label: 'Delete',
							iconHref: '#context-menu-delete-icon',
							action: {
								isLink: false,
								onclick: () => openDeleteAlbumDialog(currentAlbum!)
							},
							type: 'red'
						}
					]
				}
			: {
					type: 'message',
					message: 'No album selected',
					iconHref: '#common-crossed-circle-icon'
				}
	);
</script>

<ContextMenuWithActions bind:this={contextMenu} content={menuContent} />
