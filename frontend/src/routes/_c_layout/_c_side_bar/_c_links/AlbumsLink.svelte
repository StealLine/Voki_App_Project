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

<SideBarLink href="/albums" text="Albums" {isCollapsed} animateIcon={animateIcon}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" color="currentColor" fill="none">
		<path
			d="M16.2627 10.5H7.73725C5.15571 10.5 3.86494 10.5 3.27143 11.3526C2.67793 12.2052 3.11904 13.4258 4.00126 15.867L5.08545 18.867C5.54545 20.1398 5.77545 20.7763 6.2889 21.1381C6.80235 21.5 7.47538 21.5 8.82143 21.5H15.1786C16.5246 21.5 17.1976 21.5 17.7111 21.1381C18.2245 20.7763 18.4545 20.1398 18.9146 18.867L19.9987 15.867C20.881 13.4258 21.3221 12.2052 20.7286 11.3526C20.1351 10.5 18.8443 10.5 16.2627 10.5Z"
			stroke="currentColor"
			stroke-width="1.85"
			stroke-linecap="square"
			data-custom="0"
			class:hov={isHovered}
		/>
		<path
			d="M19 8C19 7.53406 19 7.30109 18.9239 7.11732C18.8224 6.87229 18.6277 6.67761 18.3827 6.57612C18.1989 6.5 17.9659 6.5 17.5 6.5H6.5C6.03406 6.5 5.80109 6.5 5.61732 6.57612C5.37229 6.67761 5.17761 6.87229 5.07612 7.11732C5 7.30109 5 7.53406 5 8"
			stroke="currentColor"
			stroke-width="1.85"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="1"
			class:hov={isHovered}
		/>
		<path
			d="M16.5 4C16.5 3.53406 16.5 3.30109 16.4239 3.11732C16.3224 2.87229 16.1277 2.67761 15.8827 2.57612C15.6989 2.5 15.4659 2.5 15 2.5H9C8.53406 2.5 8.30109 2.5 8.11732 2.57612C7.87229 2.67761 7.67761 2.87229 7.57612 3.11732C7.5 3.30109 7.5 3.53406 7.5 4"
			stroke="currentColor"
			stroke-width="1.85"
			stroke-linecap="round"
			stroke-linejoin="round"
			class:hov={isHovered}
			data-custom="2"
		/>
	</svg>
</SideBarLink>

<style>
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

	path[data-custom='2'].hov {
		animation: a2 0.6s forwards;
	}

	@keyframes a0 {
		0%{
            transform: translateY(0) scale(1);
        }

		40% {
			transform: translateY(-5px) scale(1.1);
		}

		100% {
			transform: translate(0) scale(1);
		}
	}

	@keyframes a1 {
		10%,
		50% {
			opacity: 0;
			transform: translateY(3px);
		}

		60%,
		100% {
			opacity: 1;
			transform: translate(0);
		}
	}

	@keyframes a2 {
		00%,
		40% {
			opacity: 0;
			transform: translateY(3px);
		}

		80%,
		100% {
			opacity: 1;
			transform: translateY(0);
		}
	}
</style>
