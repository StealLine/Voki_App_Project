<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import type { AlbumViewData } from './add-voki-to-albums-dialog-state.svelte';

	interface Props {
		albumsViewData: AlbumViewData[];
		albumIdToIsChosen: Record<string, boolean>;
		isAlbumChosenChanged: (albumId: string) => boolean;
		changeToCreateNewAlbum: () => void;
		updateVokiPresenceInAlbums: () => void;
	}
	let {
		albumsViewData,
		isAlbumChosenChanged,
		albumIdToIsChosen,
		changeToCreateNewAlbum,
		updateVokiPresenceInAlbums
	}: Props = $props();
</script>

<div class="all-albums-view">
	<div class="list">
		{#each albumsViewData as album}
			<label class="album unselectable">
				<svg
					class="album-icon"
					style="

					--icon-color-1: {album.mainColor};
					--icon-color-2: {album.secondaryColor};"><use href="#{album.icon}" /></svg
				>
				{album.name}
				<DefaultCheckBox bind:checked={albumIdToIsChosen[album.id]} />
				<span class="changed-indicator" class:active={isAlbumChosenChanged(album.id)}></span>
			</label>
		{/each}
		<button class="create-new-btn" onclick={() => changeToCreateNewAlbum()}>
			Create new albums button
		</button>
	</div>
	<PrimaryButton onclick={() => updateVokiPresenceInAlbums()}>Save</PrimaryButton>
</div>

<style>
	.all-albums-view {
		display: grid;
		width: 100%;
		height: 100%;
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.list {
		display: flex;
		flex-direction: column;
		gap: 0.125rem;
		width: 100%;
		height: 100%;
	}

	.album {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 1rem;
		border-radius: 0.375rem;
		font-size: 1.125rem;
		font-weight: 450;
		grid-template-columns: auto 1fr auto auto;
	}

	.album:hover {
		background-color: var(--secondary);
	}

	.album-icon {
		width: 1.675rem;
		height: 1.675rem;
		stroke-width: 1.9;
	}

	.changed-indicator {
		width: 0.375rem;
		height: 0.375rem;
		border-radius: 50%;
		transition: all 0.15s ease-out;
		transform: scale(0);
	}

	.changed-indicator.active {
		background-color: var(--primary);
		transform: scale(1);
	}

	.create-new-btn {
		width: fit-content;
		padding: 0.25rem 1rem;
		margin-top: 1rem;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-weight: 500;
		letter-spacing: 0.1px;
		transition: all 0.1s ease-out;
		align-self: center;
	}

	.create-new-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		letter-spacing: 0.25px;
	}
</style>
