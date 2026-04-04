<script lang="ts">
	import type { VokiAlbumPreviewData } from '../../types';
	let { album, openContextMenu }: Props = $props();
	interface Props {
		album: VokiAlbumPreviewData;
		openContextMenu: (event: MouseEvent, album: VokiAlbumPreviewData) => void;
	}

	const handleMenuClick = (event: MouseEvent) => {
		event.stopPropagation();
		event.preventDefault();
		openContextMenu(event, album);
	};
</script>

<a class="album-card" href={`/albums/${album.id}`}>
	<svg
		class="album-icon"
		style="

		--icon-color-1: {album.mainColor}; 
		--icon-color-2: {album.secondaryColor};"
	>
		<use href={`#${album.icon}`} />
	</svg>

	<div class="album-main">
		<p class="album-name">{album.name}</p>
		<p class="album-meta">
			{album.vokisCount === 1 ? '1 Voki' : `${album.vokisCount} Vokis`}
		</p>
	</div>

	<button
		type="button"
		class="album-menu-button"
		aria-label="Open album menu"
		onclick={(event) => handleMenuClick(event)}
	>
		<span class="menu-dot" />
		<span class="menu-dot" />
		<span class="menu-dot" />
	</button>
</a>

<style>
	.album-card {
		display: grid;
		align-items: center;
		gap: 0.75rem;
		padding: 0.75rem 1rem;
		border-radius: 0.75rem;
		color: var(--text);
		box-shadow: var(--shadow-xs);
		grid-template-columns: auto 1fr 2rem;
		overflow: hidden;
	}

	.album-card:hover {
		background-color: var(--secondary);
	}

	.album-icon {
		width: 2.25rem;
		height: 2.25rem;
		stroke-width: 2;
	}

	.album-main {
		display: flex;
		flex-direction: column;
		gap: 0.125rem;
		min-width: 0;
	}

	.album-name {
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 550;
		white-space: nowrap;
		text-overflow: ellipsis;
		overflow: hidden;
	}

	.album-meta {
		color: var(--muted-foreground);
		font-size: 0.875rem;
	}

	.album-menu-button {
		display: inline-flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 1.875rem;
		height: 1.875rem;
		padding: 0.25rem;
		border: none;
		border-radius: 0.375rem;
		background-color: transparent;
		cursor: pointer;
	}

	.album-menu-button:hover {
		background-color: var(--muted);
	}

	.menu-dot {
		width: 0.125rem;
		height: 0.125rem;
		margin: auto;
		border-radius: 9999rem;
		background-color: var(--muted-foreground);
	}
</style>
