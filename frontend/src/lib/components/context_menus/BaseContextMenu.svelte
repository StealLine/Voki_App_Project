<script lang="ts">
	import { onMount, onDestroy, type Snippet } from 'svelte';
	import { onClickOutside } from 'runed';

	type XY = { x: number; y: number };

	let containerRef = $state<HTMLElement | null>(null);

	let menuState: { isOpen: boolean; initialClick: XY; position: XY; offset: XY } = $state({
		isOpen: false,
		initialClick: { x: 0, y: 0 },
		position: { x: 0, y: 0 },
		offset: { x: 0, y: 0 }
	});

	let scrollPos = $state({ x: 0, y: 0 });

	interface Props {
		children: Snippet;
		onAfterClose?: () => void;
		class?: string;
		id?: string;
	}

	let { children, class: className = '', onAfterClose, id = '' }: Props = $props();

	export function open(x: number, y: number, offsetX = 0, offsetY = 0) {
		menuState.initialClick = { x, y };
		menuState.offset = { x: offsetX, y: offsetY };
		menuState.isOpen = true;

		lockScroll();
		requestAnimationFrame(fixPosition);
	}

	function fixPosition() {
		if (!containerRef) return;

		const box = containerRef.getBoundingClientRect();
		const vw = window.innerWidth;
		const vh = window.innerHeight;

		let left = menuState.initialClick.x;
		let top = menuState.initialClick.y;

		let flippedX = false;
		let flippedY = false;

		if (left + box.width > vw) {
			left -= box.width;
			flippedX = true;
		}

		if (top + box.height > vh) {
			top -= box.height;
			flippedY = true;
		}

		const offsetX = flippedX ? -menuState.offset.x : menuState.offset.x;
		const offsetY = flippedY ? -menuState.offset.y : menuState.offset.y;

		menuState.position = {
			x: left + offsetX,
			y: top + offsetY
		};
	}

	function preventScroll(e: Event) {
		e.preventDefault();
	}

	function lockScroll() {
		scrollPos = { x: window.scrollX, y: window.scrollY };
		window.addEventListener('wheel', preventScroll, { passive: false });
		window.addEventListener('touchmove', preventScroll, { passive: false });
	}

	export function close() {
		menuState.isOpen = false;
		unlockScroll();
		onAfterClose?.();
	}

	function unlockScroll() {
		window.removeEventListener('wheel', preventScroll);
		window.removeEventListener('touchmove', preventScroll);
	}

	onMount(() => {
		const observer = new MutationObserver(() => {
			if (window.scrollX !== scrollPos.x || window.scrollY !== scrollPos.y) {
				close();
			}
		});

		observer.observe(document.body, { attributes: true, subtree: true });
		window.addEventListener('scroll', close, { passive: true });

		onDestroy(() => {
			observer.disconnect();
			window.removeEventListener('scroll', close);
		});
	});

	onClickOutside(() => containerRef, close);
</script>

{#if menuState.isOpen}
	<div
		bind:this={containerRef}
		class="context-menu {className}"
		style="top:{menuState.position.y}px; left:{menuState.position.x}px;"
		{id}
	>
		{@render children()}
	</div>
{/if}

<style>
	.context-menu {
		position: absolute;
		z-index: 9999;
		animation: fade-in 0.04s ease-out;
	}

	@keyframes fade-in {
		from {
			opacity: 0;
			transform: scale(0.6);
		}

		to {
			opacity: 1;
			transform: scale(1);
		}
	}
</style>
