<script lang="ts">
	import { goto } from '$app/navigation';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { getCreateNewAlbumOpenFunction } from '../../_c_layout/_ts_layout_contexts/album-creation-dialog-context';
	import type { VokiAlbumPreviewData } from '../types';
	import AlbumsPageSectionHeader from './AlbumsPageSectionHeader.svelte';
	import ConfirmAlbumDeletionDIalog from './_c_user_albums_section/ConfirmAlbumDeletionDIalog.svelte';
	import CopyVokisFromAnotherAlbumDialog from './_c_user_albums_section/CopyVokisFromAnotherAlbumDialog.svelte';
	import EditAlbumDialog from './_c_user_albums_section/EditAlbumDialog.svelte';
	import UserAlbumsContextMenu from './_c_user_albums_section/UserAlbumsContextMenu.svelte';
	import UserAlbumView from './_c_user_albums_section/UserAlbumView.svelte';

	interface Props {
		albums: VokiAlbumPreviewData[];
	}
	let { albums }: Props = $props();
	const openCreateNewAlbumDialog = getCreateNewAlbumOpenFunction();

	function openNewAlbumDialog(): void {
		const onAfterCreated = (newAlbumId: string) => {
			goto(`/albums/${newAlbumId}`);
		};
		openCreateNewAlbumDialog(onAfterCreated);
	}
	let userAlbumsContextMenu = $state<UserAlbumsContextMenu>()!;

	let editAlbumDialog = $state<EditAlbumDialog>()!;
	let confirmAlbumDeletionDialog = $state<ConfirmAlbumDeletionDIalog>()!;
	let copyVokisFromAnotherAlbumDialog = $state<CopyVokisFromAnotherAlbumDialog>()!;
	function updateAlbum(newAlbum: VokiAlbumPreviewData) {
		let albumToUpdateInd = albums.findIndex((a) => a.id === newAlbum.id);
		if (albumToUpdateInd != -1) {
			albums[albumToUpdateInd] = newAlbum;
		}
	}

	function removeAlbum(albumId: string) {
		albums = albums.filter((a) => a.id !== albumId);
	}
</script>

{#snippet headerIcon()}
	<svg><use href="#common-plus-icon" /></svg>
{/snippet}

<AlbumsPageSectionHeader
	headerText="User albums"
	rightButton={{
		text: 'Create new album',
		onclick: openNewAlbumDialog,
		icon: headerIcon
	}}
/>
<ConfirmAlbumDeletionDIalog
	bind:this={confirmAlbumDeletionDialog}
	removeAlbum={(aId) => removeAlbum(aId)}
/>
<EditAlbumDialog bind:this={editAlbumDialog} updateParent={(a) => updateAlbum(a)} />
<CopyVokisFromAnotherAlbumDialog
	bind:this={copyVokisFromAnotherAlbumDialog}
	userAlbums={albums}
	updateParent={(a) => updateAlbum(a)}
/>

<UserAlbumsContextMenu
	bind:this={userAlbumsContextMenu}
	openDeleteAlbumDialog={(a) => confirmAlbumDeletionDialog.open(a)}
	openEditAlbumDialog={(a) => editAlbumDialog.open(a)}
	openCopyFromAnotherAlbumDialog={(a) => copyVokisFromAnotherAlbumDialog.open(a)}
/>
{#if albums.length === 0}
	{#if albums.length === 0}
		<div class="no-albums">
			<h2>You don't have any Albums</h2>
			<p>
				Albums help you organize Vokis into meaningful collections — by topic, mood, or purpose.
				<br />
				Use button above to create your first album
			</p>
		</div>
	{/if}
{:else}
	<div class="albums-list">
		{#each albums as album}
			<UserAlbumView
				{album}
				openContextMenu={(event, album) => userAlbumsContextMenu.open(event, album)}
			/>
		{/each}
	</div>
{/if}

<style>
	.albums-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.no-albums {
		position: relative;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		margin-top: 2rem;
		text-align: center;
	}

	.no-albums h2 {
		margin-bottom: 0.5rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 600;
	}

	.no-albums p {
		margin-bottom: 1.25rem;
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 450;
		line-height: 1.4;
		text-wrap: pretty;
	}
</style>
