<script lang="ts">
	import { type Snippet } from 'svelte';
	import AppToaster from './_c_layout/AppToaster.svelte';
	import LayoutSprites from './_c_layout/LayoutSprites.svelte';
	import ConfirmActionDialog from './_c_layout/_c_dialogs/ConfirmActionDialog.svelte';
	import SignInDialog from './_c_layout/_c_dialogs/SignInDialog.svelte';
	import { registerConfirmActionDialogOpenFunction } from './_c_layout/_ts_layout_contexts/confirm-action-dialog-context';
	import { registerSignInDialogOpenFunction } from './_c_layout/_ts_layout_contexts/sign-in-dialog-context';
	import VokiItemFlagsInfoDialog from './_c_layout/_c_dialogs/VokiItemFlagsInfoDialog.svelte';
	import { registerVokiFlagsInfoDialogOpenFunction } from './_c_layout/_ts_layout_contexts/voki-flags-info-dialog-context';
	import CreateNewAlbumDialog from './_c_layout/_c_dialogs/CreateNewAlbumDialog.svelte';
	import { registerCreateNewAlbumOpenFunction } from './_c_layout/_ts_layout_contexts/album-creation-dialog-context';
	import ErrsViewDialog from './_c_layout/_c_dialogs/ErrsViewDialog.svelte';
	import { registerErrsViewDialogOpenFunction } from './_c_layout/_ts_layout_contexts/errs-view-dialog-context';
	import AddVokiToAlbumsDialog from './_c_layout/_c_dialogs/AddVokiToAlbumsDialog.svelte';
	import { registerAddVokiToAlbumsOpenFunction } from './_c_layout/_ts_layout_contexts/add-voki-to-albums-dialog-context';
	import MainLayoutHeader from './_c_layout/MainLayoutHeader.svelte';
	import MainLayoutLeftSideBar from './_c_layout/MainLayoutLeftSideBar.svelte';
	let isFullWidthMode = $state(false);
	let { children }: { children: Snippet } = $props<{ children: Snippet }>();

	let signInDialog = $state<SignInDialog>()!;
	registerSignInDialogOpenFunction((state) => signInDialog.open(state));

	let confirmActionDialog = $state<ConfirmActionDialog>()!;
	registerConfirmActionDialogOpenFunction({
		open: (content) => confirmActionDialog.open(content),
		close: () => confirmActionDialog.close()
	});

	let createNewAlbumDialog = $state<CreateNewAlbumDialog>()!;
	registerCreateNewAlbumOpenFunction((onAfterNewAlbumCreated) =>
		createNewAlbumDialog.open(onAfterNewAlbumCreated)
	);

	let vokiItemFlagsInfoDialog = $state<VokiItemFlagsInfoDialog>()!;
	registerVokiFlagsInfoDialogOpenFunction(() => vokiItemFlagsInfoDialog.open());

	let errsViewDialog = $state<ErrsViewDialog>()!;
	registerErrsViewDialogOpenFunction((errs) => errsViewDialog.open(errs));

	let addVokiToAlbumsDialog = $state<AddVokiToAlbumsDialog>()!;
	registerAddVokiToAlbumsOpenFunction((vokiId) => addVokiToAlbumsDialog.open(vokiId));
</script>

<SignInDialog bind:this={signInDialog} />
<ConfirmActionDialog bind:this={confirmActionDialog} />
<CreateNewAlbumDialog bind:this={createNewAlbumDialog} />
<VokiItemFlagsInfoDialog bind:this={vokiItemFlagsInfoDialog} />
<ErrsViewDialog bind:this={errsViewDialog} />
<AddVokiToAlbumsDialog bind:this={addVokiToAlbumsDialog} />

<LayoutSprites />
<AppToaster />

<div id="vokimi-app" class:full-width={isFullWidthMode}>
	<MainLayoutHeader />
	<div class="sidebar">
		<MainLayoutLeftSideBar />
	</div>
	<div id="page-content">
		{@render children()}
	</div>
</div>

<style>
	#vokimi-app {
		display: flex;
		flex-direction: column;
		width: 100%;
		box-sizing: border-box;
		padding-top: var(--layout-header-height);
		padding-left: var(--side-panel-width);
	}

	.sidebar {
		position: fixed;
		top: var(--layout-header-height);
		left: 0;
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		width: var(--side-panel-width);
		height: calc(100vh - var(--layout-header-height));
		padding: 0 var(--sides-padding);
	}

	#page-content {
		padding-right: var(--sides-padding);
	}

	#page-content > :global(*) {
		animation: 0.2s page-fade-in;
	}

	@keyframes page-fade-in {
		from {
			opacity: 0.2;
		}

		to {
			opacity: 1;
		}
	}
</style>
