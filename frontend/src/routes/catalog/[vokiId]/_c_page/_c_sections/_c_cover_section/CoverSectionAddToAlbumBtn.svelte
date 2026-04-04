<script lang="ts">
	import { getAddVokiToAlbumsOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/add-voki-to-albums-dialog-context';

	let { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let isHovered = $state(false);
	function animateIcon() {
		isHovered = true;
		setTimeout(() => {
			isHovered = false;
		}, 500);
	}
	const openAddVokiToAlbumDialog = getAddVokiToAlbumsOpenFunction();
</script>

<button
	class="add-to-album-btn"
	onmouseenter={() => animateIcon()}
	onfocus={() => animateIcon()}
	onclick={() => openAddVokiToAlbumDialog(vokiId)}
>
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		class="plus icon-item"
		class:animate={isHovered}
		stroke-linecap="round"
		stroke-linejoin="round"
		stroke="currentColor"
	>
		<path d="M5 12h14" class="horizontal" />
		<path d="M12 5v14" class="vertical" />
	</svg>
	<label class="btn-text">Add to album</label>
</button>

<style>
	.add-to-album-btn {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}

	.add-to-album-btn:hover,
	.add-to-album-btn:focus {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.horizontal,
	.vertical {
		stroke-dasharray: 14;
		stroke-dashoffset: 0;
		transition: stroke-dashoffset 0.15s ease-out;
	}

	.plus.animate .horizontal {
		opacity: 0;
		animation: line-animation 0.2s ease-out forwards;
	}

	.plus.animate .vertical {
		opacity: 0;
		animation: line-animation 0.2s ease-out 0.25s forwards;
	}

	@keyframes line-animation {
		0% {
			opacity: 0;
			stroke-dashoffset: 14;
		}

		15% {
			opacity: 1;
			stroke-dashoffset: 14;
		}

		100% {
			opacity: 1;
			stroke-dashoffset: 0;
		}
	}
</style>
