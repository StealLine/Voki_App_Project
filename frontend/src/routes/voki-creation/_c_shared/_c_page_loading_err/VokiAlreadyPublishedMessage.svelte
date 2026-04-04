<script lang="ts">
	import {
		type VokiItemViewState,
		VokiItemViewUtils
	} from '$lib/components/voki_item/_c_voki_item/voki-item';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';

	interface Props {
		voki: PublishedVokiBriefInfo;
	}
	let { voki }: Props = $props();
	let vokiItemProps: VokiItemViewState = $derived({
		name: 'ok',
		data: VokiItemViewUtils.briefInfoToVokiItemOkStateProps(voki, `/catalog/${voki.id}`, () => {}),
		hide: ['MoreBtn']
	});
</script>

<div class="card">
	<h1 class="title animate-in animate-1">This Voki is already published</h1>
	<p class="subtitle animate-in animate-2">
		This Voki has been published and can no longer be edited here.<br /> You can view the published version
		in the catalog.
	</p>
	<div class="voki-item-wrapper animate-in animate-3">
		<VokiItemView state={vokiItemProps} />
	</div>

	<a class="catalog-btn animate-in animate-4" href={`/catalog/${voki.id}`}>Open in catalog</a>
</div>

<style>
	.card {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 33rem;
		margin: 0 auto;
		text-align: center;
	}

	.title {
		color: var(--text);
		font-size: 1.75rem;
		font-weight: 600;
	}

	.subtitle {
		width: 100%;
		margin-top: 0.25rem;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		line-height: 1.4;
		text-align: center;
	}

	.voki-item-wrapper {
		display: block;
		width: 22rem;
		margin: 1rem auto;
	}

	.catalog-btn {
		width: calc(100% - 2rem);
		padding: 0.5rem 0;
		border-radius: var(--radius);
		background: var(--primary);
		color: var(--primary-foreground);
		font-size: 1rem;
		font-weight: 500;
		letter-spacing: 1px;
		transition: background-color 0.1s ease;
	}

	.catalog-btn:hover {
		background-color: var(--primary-hov);
	}

	.animate-in {
		opacity: 0;
		animation: fade-slide-in 0.35s ease-out forwards;
	}

	.animate-1 {
		animation-delay: 0s;
	}

	.animate-2 {
		animation-delay: 0.1s;
	}

	.animate-3 {
		animation-delay: 0.2s;
	}

	.animate-4 {
		animation-delay: 0.3s;
	}

	@keyframes fade-slide-in {
		from {
			opacity: 0;
			transform: translateY(0.5rem) scale(0.95);
		}

		to {
			opacity: 1;
			transform: translateY(0) scale(1);
		}
	}

	@media (prefers-reduced-motion: reduce) {
		.animate-in {
			opacity: 1;
			transform: none;
			animation: none;
		}
	}
</style>
