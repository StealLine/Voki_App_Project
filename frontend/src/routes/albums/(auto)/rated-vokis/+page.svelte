<script lang="ts">
	import AlbumPageHeader from '../../_c_pages_shared/AlbumPageHeader.svelte';
	import type { PageProps } from './$types';
	import { RatedVokisAlbumPageState } from './rated-vokis-album-page-state.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import AlbumPageFilterAndSort from '../../_c_pages_shared/AlbumPageFilterAndSort.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import AlbumEmptyMessage from '../../_c_pages_shared/AlbumEmptyMessage.svelte';
	import NoVokisInAlbumMatchFilterMessage from '../../_c_pages_shared/NoVokisInAlbumMatchFilterMessage.svelte';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import { relativeTime } from 'svelte-relative-time';
	import BasicStarsDisplay from '$lib/components/BasicStarsDisplay.svelte';

	let { data }: PageProps = $props();
	const pageState = new RatedVokisAlbumPageState(
		data.response.isSuccess ? data.response.data.vokiIdToLastRatingData : {},
		(_, __) => {}
	);
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load rated Vokis auto album"
	/>
{:else}
	<AlbumPageHeader
		content={{
			type: 'auto',
			albumName: 'rated Vokis'
		}}
	/>

	{#if pageState.isInitialListEmpty()}
		<AlbumEmptyMessage
			title="Your rated Vokis auto album is empty"
			subtitle="Vokis will be added automatically to this album after you rate them for the first time"
		/>
	{:else}
		<AlbumPageFilterAndSort
			onVokiTypeClick={(t) => pageState.toggleTypeFilter(t)}
			chooseSortOption={(o) => pageState.chooseSortOption(o)}
			sortOptions={pageState.allSortOptions}
			chosenVokiTypes={pageState.filterAndSort.chosenVokiTypes}
			currentSortOption={pageState.filterAndSort.currentSortOption}
		/>
		{#if pageState.sortedAndFilteredVokis().length === 0}
			<NoVokisInAlbumMatchFilterMessage />
		{:else}
			<div class="vokis-container">
				{#each pageState.sortedAndFilteredVokis() as voki}
					<div class="voki-item">
						<VokiItemView state={{ ...voki, hide: ['Name', 'Authors', 'MoreBtn'] }} />
						<div class="name-and-authors">
							{#if voki.name === 'ok'}
								<p class="voki-name">{voki.data.voki.name}</p>
								<div class="authors">
									<span class="by-label">by:</span>
									<BasicUserDisplay
										userId={voki.data.voki.primaryAuthorId}
										interactionLevel="WholeComponentLink"
									/>
									<div class="co-authors">
										{#each voki.data.voki.coAuthorIds as coAuthorId}
											<BasicUserDisplay userId={coAuthorId} interactionLevel="WholeComponentLink" />
										{/each}
									</div>
								</div>
							{/if}
						</div>
						<BasicStarsDisplay class="stars-display" value={voki.rating.value} />

						<a href="/catalog/{voki.vokiId}?tab=ratings" class="open-voki-page">See Voki ratings</a>
						<span class="date" use:relativeTime={{ date: voki.rating.dateTime }} />
					</div>
				{/each}
			</div>
		{/if}
	{/if}
{/if}

<style>
	.vokis-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}

	.voki-item {
		position: relative;
		display: grid;
		border-radius: calc(var(--voki-cover-border-radius) * 1.25);
		box-shadow: var(--shadow-xs);
		grid-template-columns: 20rem 1fr auto auto;
	}

	.voki-name {
		margin: 0.5rem;
		font-size: 1.25rem;
		font-weight: 500;
		text-indent: 0.125em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.by-label {
		margin-right: 0.5rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.authors {
		display: flex;
		flex-direction: row;
		align-items: center;
	}

	.co-authors {
		margin: 0 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.voki-item > :global(.stars-display) {
		align-self: center;
	}

	.date {
		position: absolute;
		right: 1rem;
		bottom: 1rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		place-self: end right;
	}

	.open-voki-page {
		margin: 0 2rem;
		align-self: center;
	}
</style>
