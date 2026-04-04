<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import SignInRequiredToUseAlbums from '$lib/components/SignInRequiredToUseAlbums.svelte';
	import { getCreateNewAlbumOpenFunction } from '../_ts_layout_contexts/album-creation-dialog-context';
	import { AddVokiToAlbumsDialogState } from './_c_add_voki_to_album/add-voki-to-albums-dialog-state.svelte';
	import AlbumsDialogAlbumsChoosing from './_c_add_voki_to_album/AlbumsDialogAlbumsChoosing.svelte';
	import AlbumsDialogNoAlbumsState from './_c_add_voki_to_album/AlbumsDialogNoAlbumsState.svelte';

	let dialogState = new AddVokiToAlbumsDialogState();
	let dialog = $state<DialogWithCloseButton>()!;
	let currentVokiId: string | null = $state(null);
	export function open(vokiId: string) {
		currentVokiId = vokiId;
		dialogState.setVokiAndUpdate(vokiId);
		dialog.open();
	}
	const openCreateNewAlbumDialog = getCreateNewAlbumOpenFunction();
	function changeToCreateNewAlbum() {
		openCreateNewAlbumDialog(() => {
			if (currentVokiId === null) {
				return;
			}
			dialogState.setVokiAndUpdate(currentVokiId);

			open(currentVokiId);
		});
		dialog.close();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="user-voki-albums-dialog">
	<AuthView>
		{#snippet children(authState)}
			{#if authState.name === 'loading' || dialogState.albumsState.name === 'loading'}
				<div class="loading-container">
					<CubesLoader sizeRem={6} color="var(--primary)" />
					<h1>Loading your albums</h1>
				</div>
			{:else if authState.name === 'error'}
				<div class="err-view">
					<h1>Error in the authentication part</h1>
					<DefaultErrBlock errList={authState.errs} />
				</div>
			{:else if !authState.isAuthenticated}
				<SignInRequiredToUseAlbums closeDialog={dialog.close} />
			{:else if dialogState.albumsState.name === 'errs'}
				<div class="err-view">
					<h1>Error in the albums fetching part</h1>
					<DefaultErrBlock errList={dialogState.albumsState.errs} />
				</div>
			{:else if dialogState.albumsState.name === 'ok' && currentVokiId !== null}
				{#if dialogState.albumsState.albums.length === 0}
					<AlbumsDialogNoAlbumsState {changeToCreateNewAlbum} />
				{:else}
					<AlbumsDialogAlbumsChoosing
						{changeToCreateNewAlbum}
						albumsViewData={dialogState.albumsState.albums}
						albumIdToIsChosen={dialogState.albumToIsChosen}
						isAlbumChosenChanged={(id) => dialogState.isAlbumChosenChanged(id)}
						updateVokiPresenceInAlbums={() => dialogState.updateVokiPresenceInAlbums(currentVokiId!)}
					/>
				{/if}
			{:else}
				<h1>Something went wrong</h1>
				{#if currentVokiId}
					<ReloadButton onclick={() => dialogState.setVokiAndUpdate(currentVokiId!)} />
				{:else}
					<button onclick={() => dialog.close()}>Close</button>
				{/if}
			{/if}
		{/snippet}
	</AuthView>
</DialogWithCloseButton>

<style>
	:global(#user-voki-albums-dialog > .dialog-content) {
		width: 48rem;
		height: 32rem;
	}

	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 2rem;
		width: 100%;
		height: 100%;
		padding-bottom: 2rem;
	}

	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 2rem;
		font-weight: 550;
		letter-spacing: 0.5px;
	}

	.err-view {
		width: 100%;
		height: 100%;
	}
</style>
