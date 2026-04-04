<script lang="ts">
	import logo from '$lib/logos/main-vokimi-logo.svg';
	import LayoutHeaderRightPart from './_c_header/LayoutHeaderRightPart.svelte';
	import MainLayoutHeaderSearch from './_c_header/MainLayoutHeaderSearch.svelte';
</script>

<div class="main-layout-header">
	<div class="side left unselectable">
		<img src={logo} alt="logo" />
	</div>

	<div class="middle-part">
		<MainLayoutHeaderSearch />
	</div>
	<div class="side right">
		<LayoutHeaderRightPart />
	</div>
</div>

<style>
	.main-layout-header {
		position: fixed;
		top: 0;
		right: 0;
		left: 0;
		z-index: 9999;
		display: grid;
		height: var(--layout-header-height);
		max-height: var(--layout-header-height);
		padding: var(--layout-header-vertical-padding) 0;
		grid-template-columns: var(--side-panel-width) 1fr var(--side-panel-width);
	}

	.side.left {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0 var(--sides-padding);
	}

	.side.left > img {
		width: fit-content;
		height: 2.25rem;
	}

	.middle-part {
		z-index: 9999;
		width: calc(100% - var(--side-panel-width));
		height: 100%;
		margin-left: var(--side-panel-width);
	}

	.side.right {
		max-height: var(--layout-header-height);
		padding: 0 var(--sides-padding);
	}
</style>
