<script lang="ts">
	import type { AuthorBanner } from '../types';
	interface Props {
		banner: AuthorBanner;
	}
	let { banner }: Props = $props();
</script>

<div class="banner-container">
	{#if banner.$type === 'Default'}
		<div class="banner-default"></div>
	{:else if banner.$type === 'FillColor'}
		<div class="banner-fill" style="background-color: {banner.color};"></div>
	{/if}
</div>

<style>
	.banner-container {
		width: 100%;
		min-width: 0;
		height: 14rem;

		position: relative;
		overflow: hidden;
		border-radius: 1rem;
	}
	.banner-default {
		background: linear-gradient(135deg, var(--primary), var(--accent));
		height: 100%;
	}
	.banner-fill {
		height: 100%;
	}
</style>
