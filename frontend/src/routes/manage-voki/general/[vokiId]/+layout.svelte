<script lang="ts">
	import { navigating, page } from '$app/state';
	import type { Snippet } from 'svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import ManageVokiLayoutHeader from '../../_c_shared/ManageVokiLayoutNavBar.svelte';

	const { children }: { children: Snippet } = $props();

	const allLinks: Record<(typeof MANAGE_TABS)[number], string> = {
		main: 'Main information',
		comments: 'Comments',
		ratings: 'Ratings',
		'voki-takings': 'Voki Takings'
	};

	const MANAGE_TABS = ['main', 'comments', 'ratings', 'voki-takings'] as const;
	const activeLinkKey = $derived.by(() => {
		const segments = page.url.pathname.split('/').filter(Boolean);
		return MANAGE_TABS.find((key) => segments.includes(key));
	});
	const linksForLayout = MANAGE_TABS.map((key) => ({
		name: allLinks[key],
		key,
		href: `/manage-voki/general/${page.params.vokiId}/${key}`
	}));
</script>

<ManageVokiLayoutHeader links={linksForLayout} {activeLinkKey} />
{#if navigating.type}
	<div class="loading fade-in-animation">
		<h1>Loading tab data</h1>
		<CubesLoader sizeRem={5} color="var(--primary)" />
	</div>
{:else}
	<div class="manage-page-content fade-in-animation">
		{@render children()}
	</div>
{/if}

<style>
	.loading {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
	}

	.loading h1 {
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 2px;
	}

	.fade-in-animation {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
</style>
