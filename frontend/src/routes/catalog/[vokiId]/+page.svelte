<script lang="ts">
	import { browser } from '$app/environment';
	import { onDestroy, onMount } from 'svelte';
	import type { PageProps } from './$types';
	import CoverSection from './_c_page/_c_sections/CoverSection.svelte';
	import NameSection from './_c_page/_c_sections/NameSection.svelte';
	import VokiPageAboutTab from './_c_page/_c_tabs/VokiPageAboutTab.svelte';
	import VokiPageCommentsTab from './_c_page/_c_tabs/VokiPageCommentsTab.svelte';
	import VokiPageRatingsTab from './_c_page/_c_tabs/VokiPageRatingsTab.svelte';
	import TabLinksContainer from './_c_page/TabLinksContainer.svelte';
	import VokiNotLoaded from './_c_page/VokiNotLoaded.svelte';
	import { VokiCatalogVisitMarkerCookie } from '$lib/ts/cookies/voki-catalog-visit-marker-cookie';
	import { page } from '$app/state';
	import { VokiPageState } from './voki-page-state.svelte';
	import { goto } from '$app/navigation';
	import { VokiUtils } from '$lib/ts/voki';
	import AuthorsSection from './_c_page/_c_sections/AuthorsSection.svelte';
	import type { VokiTypeWithSpecificData } from './types';

	let { data }: PageProps = $props();
	let pageState = new VokiPageState(
		data.vokiId,
		page.url.searchParams,
		data.response.isSuccess ? data.response.data.ratingsCount : 0,
		data.response.isSuccess ? data.response.data.commentsCount : 0
	);

	function setTab(tab: VokiPageState['currentTab']) {
		if (tab !== pageState.currentTab) {
			pageState.currentTab = tab;
			const url = new URL(page.url);
			url.searchParams.set('tab', tab);

			goto(url.toString(), { replaceState: true, keepFocus: true, noScroll: true });
		}
	}
	let refreshTimer: number | undefined;
	onMount(() => {
		if (browser) {
			VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);

			refreshTimer = window.setInterval(() => {
				VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);
			}, 60_000);
		}
	});
	onDestroy(() => {
		if (refreshTimer) {
			clearInterval(refreshTimer);
		}
	});
	function canUserManageVoki(uId: string) {
		if (!data.response.isSuccess) {
			return false;
		}
		return VokiUtils.canUserManageVoki(data.response.data, uId);
	}
</script>

{#if !data.response.isSuccess}
	<VokiNotLoaded errs={data.response.errs} vokiId={data.vokiId!} />
{:else}
	<div class="voki-page-container">
		<div class="main-content">
			<NameSection name={data.response.data.name} />
			<AuthorsSection
				primaryAuthorId={data.response.data.primaryAuthorId}
				coAuthorIds={data.response.data.coAuthorIds}
			/>

			<div class="tabs-section">
				<TabLinksContainer
					currentTab={pageState.currentTab}
					changeTab={setTab}
					commentsCount={pageState.commentsCount}
					ratingsCount={pageState.ratingsCount}
				/>
				<div class="current-tab">
					{#if pageState.currentTab === 'about'}
						<VokiPageAboutTab
							vokiId={data.vokiId}
							{...data.response.data}
							typeWithData={{
								type: data.response.data.type,
								typeSpecificData: data.response.data.typeSpecificData
							} as VokiTypeWithSpecificData}
						/>
					{:else if pageState.currentTab === 'comments'}
						<VokiPageCommentsTab />
					{:else if pageState.currentTab === 'ratings'}
						<VokiPageRatingsTab
							tabData={pageState.ratingsTabData}
							fetchTabData={async () => await pageState.fetchRatingsTabData()}
							saveNewUserRating={async (value) => await pageState.saveNewUserRating(value)}
							reloadOutdatedRatings={async () => await pageState.reloadOutdatedRatings()}
						/>
					{:else}
						<h1>Something went wrong</h1>
						<button onclick={() => (pageState.currentTab = 'about')}>Go back</button>
					{/if}
				</div>
			</div>
		</div>
		<CoverSection
			vokiId={data.response.data.id}
			cover={data.response.data.cover}
			canUserManageVoki={(uId) => canUserManageVoki(uId)}
			vokiType={data.response.data.type}
			signedInOnlyTaking={data.response.data.signedInOnlyTaking}
		/>
	</div>
{/if}

<style>
	.voki-page-container {
		display: grid;
		gap: var(--sides-padding);
		width: 100%;
		min-width: 0;
		grid-template-columns: 1fr auto;
	}

	.main-content {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.tabs-section {
		display: flex;
		flex-direction: column;
		padding: 0 0.5rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs);
	}

	.current-tab {
		display: flex;
		flex-direction: column;
		margin: 1rem 0.25rem 0.75rem;
	}

	.current-tab > :global(*) {
		animation: 0.25s tab-fade-in;
	}

	@keyframes tab-fade-in {
		from {
			opacity: 0.2;
		}

		to {
			opacity: 1;
		}
	}
</style>
