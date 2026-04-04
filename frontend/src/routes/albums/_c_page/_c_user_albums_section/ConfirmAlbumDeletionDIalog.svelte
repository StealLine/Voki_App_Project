<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiAlbums, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { VokiAlbumPreviewData } from '../../types';

	interface Props {
		removeAlbum: (albumId: string) => void;
	}
	let { removeAlbum }: Props = $props();
	let album: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
	let dialog = $state<DialogWithCloseButton>()!;

	async function confirmDeletion() {
		if (!album) {
			errs = [{ message: 'No album selected' }];
			return;
		}
		errs = [];
		const response = await ApiAlbums.fetchVoidResponse(
			`/albums/${album!.id}/delete`,
			RJO.DELETE({})
		);
		if (response.isSuccess) {
			removeAlbum(album.id);
			dialog.close();
		} else {
			errs = response.errs;
		}
	}
	let errs: Err[] = $state([]);
	export function open(newAlbum: VokiAlbumPreviewData) {
		errs = [];
		album = newAlbum;
		dialog.open();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="confirm-album-deletion-dialog">
	{#if album}
		<h1 class="title">
			Delete <span class="album-name">{album.name}</span>?
		</h1>
		<p class="description">
			This action is <span>irreversible</span><br /> The album contains
			<strong>{album.vokisCount}</strong> Vokis
		</p>
		<DefaultErrBlock errList={errs} />
		<div class="buttons">
			<button class="cancel-btn" onclick={() => dialog.close()}> Cancel </button>
			<button class="delete-btn" onclick={() => confirmDeletion()}> Delete album </button>
		</div>
	{:else}
		<p class="no-album">No album selected</p>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#confirm-album-deletion-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		text-align: center;
	}

	.title {
		margin: 1rem 1rem 0;
		color: var(--text);
		font-size: 1.75rem;
		font-weight: 550;
	}

	.album-name {
		display: inline-block;
		max-width: 26rem;
		padding-bottom: 0.25rem;
		color: var(--primary);
		white-space: nowrap;
		text-overflow: ellipsis;
		overflow: hidden;
		vertical-align: middle;
	}

	.description {
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		line-height: 1.4;
	}

	.buttons {
		display: grid;
		grid-template-columns: 1fr 1fr;
		flex-direction: column;
		gap: 1rem;
		width: 100%;
		margin-top: 1rem;
	}

	.buttons > * {
		padding: 0.675rem 1rem;
		border: none;
		border-radius: var(--radius);
		font-size: 1rem;
		font-weight: 450;
		cursor: pointer;
	}

	.delete-btn {
		background-color: var(--red-5);
		color: var(--primary-foreground);
	}

	.buttons > *:hover {
		font-weight: 520;
	}

	.cancel-btn {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}

	.cancel-btn:hover {
		background-color: var(--muted);
	}

	.no-album {
		padding: 2rem;
		color: var(--muted-foreground);
		text-align: center;
	}
</style>
