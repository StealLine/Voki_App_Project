<script lang="ts">
	import type { MyVokisPageTabMarker } from '../tab-marker';

	interface Props {
		text: string;
		tab: MyVokisPageTabMarker.Tab;
		currentTab: string;
	}
	let { text, tab, currentTab }: Props = $props();
</script>

<a
	data-sveltekit-preload-data="off"
	href="/my-vokis/{tab}"
	class="unselectable"
	class:current={currentTab === tab}>{text}</a
>

<style>
	a {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 12rem;
		height: 100%;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 450;
		text-decoration: none;
		letter-spacing: 0.25px;
		box-shadow: var(--shadow-md), var(--shadow);
		transition: transform 0.12s ease;
		cursor: pointer;
	}

	a:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	a:active {
		letter-spacing: 0;
	}

	a.current {
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-weight: 475;
	}
</style>
