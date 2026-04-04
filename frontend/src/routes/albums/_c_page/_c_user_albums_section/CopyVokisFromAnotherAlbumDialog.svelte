<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiAlbumPreviewData } from '../../types';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import { ApiAlbums, RJO } from '$lib/ts/backend-communication/backend-services';
	import { toast } from 'svelte-sonner';
	interface Props {
		userAlbums: VokiAlbumPreviewData[];
		updateParent: (newData: VokiAlbumPreviewData) => void;
	}
	let { userAlbums, updateParent }: Props = $props();
	let destination: VokiAlbumPreviewData | undefined = $state(undefined)!;
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = $state(false)!;
	let savingErrs: Err[] = $state([]);

	export function open(a: VokiAlbumPreviewData) {
		destination = a;
		dialog.open();
	}
	async function save() {
		if (!destination) {
			savingErrs = [{ message: 'No destination album selected' }];
			return;
		}
		savingErrs = [];
		isLoading = true;

		const response = await ApiAlbums.fetchJsonResponse<{
			newVokisCount: number;
			vokisAdded: number;
		}>(
			`/albums/${destination.id}/copy-vokis-from-albums`,
			RJO.PATCH({
				albumIds: Object.keys(chosenAlbums).filter((k) => chosenAlbums[k])
			})
		);
		if (response.isSuccess) {
			updateParent({ ...destination, vokisCount: response.data.newVokisCount });
			isLoading = false;
			dialog.close();
			const toastMsg =
				response.data.vokisAdded === 0
					? 'All Vokis were already in this album'
					: `Copied ${response.data.vokisAdded} Voki${response.data.vokisAdded === 1 ? '' : 's'}`;
			toast.success(toastMsg);
		} else {
			savingErrs = response.errs;
			isLoading = false;
		}
	}
	let chosenAlbums = $state(Object.fromEntries(userAlbums.map((a) => [a.id, false])));
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="copy-vokis-from-another-album-dialog"
	onBeforeClose={() => (destination = undefined)}
>
	{#if destination}
		<h1 class="dialog-heading">Choose albums you want to copy Vokis from</h1>
		<div class="list">
			{#each userAlbums as album}
				<label class="album unselectable" class:destination={album.id === destination.id}>
					<svg
						class="album-icon"
						style="

						--icon-color-1: {album.mainColor};
						--icon-color-2: {album.secondaryColor};
 						"><use href="#{album.icon}" /></svg
					>
					<div class="album-name-voki-count">
						<span class="album-name">{album.name}</span>
						<span class="vokis-count"
							>{album.vokisCount} Voki{album.vokisCount === 1 ? '' : 's'}</span
						>
					</div>

					{#if album.id === destination.id}
						<span class="destination-chip">Destination</span>
					{:else}
						<DefaultCheckBox bind:checked={chosenAlbums[album.id]} />
					{/if}
				</label>
			{/each}
		</div>
		<DefaultErrBlock errList={savingErrs} class="saving-err-block" />
		<PrimaryButton onclick={() => save()} class={isLoading ? 'submit-btn loading' : 'submit-btn'}>
			{#if isLoading}
				<LinesLoader color="var(--primary-foreground);" sizeRem={1.4} strokePx={2} /> Saving...
			{:else}
				Save
			{/if}
		</PrimaryButton>
	{:else}
		<p class="no-album-selected">No destination album selected</p>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#copy-vokis-from-another-album-dialog > .dialog-content) {
		width: 46rem;
		padding: 2rem;
	}

	.dialog-heading {
		color: var(--muted-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		text-align: center;
	}

	.list {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0;
		overflow-y: auto;
	}

	.album {
		display: grid;
		align-items: center;
		gap: 0.75rem;
		padding: 0.25rem 0.75rem;
		border-radius: var(--radius);
		color: var(--text);
		grid-template-columns: auto 1fr auto auto;
	}

	.album:not(.destination) {
		transition: background-color 0.08s ease-in-out;
		cursor: pointer;
	}

	.album:not(.destination):hover {
		background-color: var(--secondary);
	}

	.album.destination {
		background-color: var(--accent);
		color: var(--accent-foreground);
		cursor: default;
	}

	.album-icon {
		width: 1.75rem;
		height: 1.75rem;
		stroke-width: 1.9;
		flex-shrink: 0;
	}

	.album-name-voki-count {
		display: grid;
		grid-template-rows: auto auto;
	}

	.album-name {
		font-size: 1.125rem;
		font-weight: 500;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
		line-height: 1.25;
	}

	.vokis-count {
		color: var(--muted-foreground);
		font-size: 0.875rem;
	}

	.album.destination .vokis-count {
		color: var(--accent-foreground);
	}

	.destination-chip {
		padding: 0.125rem 0.5rem;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 0.75rem;
	}

	.no-album-selected {
		padding: 1rem 0;
		color: var(--secondary-foreground);
		font-size: 1rem;
		text-align: center;
	}

	:global(#copy-vokis-from-another-album-dialog .dialog-content > .submit-btn) {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 14rem;
		height: 2.5rem;
		margin-top: 0.5rem;
	}

	:global(#copy-vokis-from-another-album-dialog .dialog-content > .submit-btn.loading) {
		gap: 0.5rem;
		font-size: 1.25rem;
		opacity: 0.8;
		pointer-events: none;
	}
</style>
