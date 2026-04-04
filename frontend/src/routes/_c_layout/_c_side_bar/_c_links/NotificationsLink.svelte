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

<SideBarLink
	href="/notifications"
	text="Notifications"
	{isCollapsed}
	animateIcon={animateIcon}
>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" class:hov={isHovered}>
		<path
			d="M15.5 18C15.5 19.933 13.933 21.5 12 21.5C10.067 21.5 8.5 19.933 8.5 18"
			stroke="currentColor"
			stroke-width="1.85"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M19.2311 18H4.76887C3.79195 18 3 17.208 3 16.2311C3 15.762 3.18636 15.3121 3.51809 14.9803L4.12132 14.3771C4.68393 13.8145 5 13.0514 5 12.2558V9.5C5 5.63401 8.13401 2.5 12 2.5C15.866 2.5 19 5.634 19 9.5V12.2558C19 13.0514 19.3161 13.8145 19.8787 14.3771L20.4819 14.9803C20.8136 15.3121 21 15.762 21 16.2311C21 17.208 20.208 18 19.2311 18Z"
			stroke="currentColor"
			stroke-width="1.85"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
</SideBarLink>

<style>
	svg.hov {
		transform-origin: top;
		animation: rotate-animation 0.6s ease-in-out;
	}

	@keyframes rotate-animation {
		0% {
			transform: rotate(0deg);
		}

		20% {
			transform: rotate(-10deg);
		}

		40% {
			transform: rotate(10deg);
		}

		60% {
			transform: rotate(-10deg);
		}

		100% {
			transform: rotate(0deg);
		}
	}
</style>
