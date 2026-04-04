<script lang="ts">
	import SideBarLink from '../SideBarLink.svelte';
	let { isCollapsed }: { isCollapsed: boolean } = $props<{ isCollapsed: boolean }>();

	let isHovered = $state(false);
	function animateIcon() {
		isHovered = true;
		setTimeout(() => {
			isHovered = false;
		}, 600);
	}
</script>

<SideBarLink href="/profile" text="Profile" {isCollapsed} {animateIcon}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M15 10C15 8.34315 13.6569 7 12 7C10.3431 7 9 8.34315 9 10C9 11.6569 10.3431 13 12 13C13.6569 13 15 11.6569 15 10Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="0"
			class:hov={isHovered}
		></path>
		<path
			d="M17 18C17 15.2386 14.7614 13 12 13C9.23858 13 7 15.2386 7 18"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="1"
			class:hov={isHovered}
		></path>
		<path
			d="M21 13V11C21 7.22876 21 5.34315 19.8284 4.17157C18.6569 3 16.7712 3 13 3H11C7.22876 3 5.34315 3 4.17157 4.17157C3 5.34315 3 7.22876 3 11V13C3 16.7712 3 18.6569 4.17157 19.8284C5.34315 21 7.22876 21 11 21H13C16.7712 21 18.6569 21 19.8284 19.8284C21 18.6569 21 16.7712 21 13Z"
			stroke="currentColor"
			stroke-linecap="square"
			stroke-linejoin="round"
		></path>
	</svg>
</SideBarLink>

<style>
	svg {
		stroke-width: 1.85;
	}

	path {
		transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
		transform-origin: center;
		transform: none;
	}

	path[data-custom='0'].hov {
		animation: a0 0.6s forwards;
	}

	path[data-custom='1'].hov {
		animation: a1 0.6s forwards;
	}

	@keyframes a0 {
		0% {
			opacity: 0.2;
			transform: scale(0);
		}

		40% {
			opacity: 0.4;
			transform: scale(0.2);
		}

		100% {
			opacity: 1;
			transform: scale(1);
		}
	}

	@keyframes a1 {
		0%,
		20% {
			opacity: 0.2;
			transform: translateY(2px) scale(0.2);
		}

		100% {
			opacity: 1;
			transform: translateY(0) scale(1);
		}
	}
</style>
