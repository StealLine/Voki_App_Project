<script lang="ts">
	import AlbumPageHeader from '../../_c_pages_shared/AlbumPageHeader.svelte';
	import type { PageProps } from './$types';
	import { TakenVokisAlbumPageState } from './taken-vokis-album-page-state.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import AlbumPageFilterAndSort from '../../_c_pages_shared/AlbumPageFilterAndSort.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import AlbumEmptyMessage from '../../_c_pages_shared/AlbumEmptyMessage.svelte';
	import NoVokisInAlbumMatchFilterMessage from '../../_c_pages_shared/NoVokisInAlbumMatchFilterMessage.svelte';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import { relativeTime } from 'svelte-relative-time';

	let { data }: PageProps = $props();

	const pageState = new TakenVokisAlbumPageState(
		data.response.isSuccess ? data.response.data.vokis : {},
		(_, __) => {}
	);
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load taken Vokis auto album"
	/>
{:else}
	<AlbumPageHeader
		content={{
			type: 'auto',
			albumName: 'taken Vokis'
		}}
	/>

	{#if pageState.isInitialListEmpty()}
		<AlbumEmptyMessage
			title="Your taken Vokis auto album is empty"
			subtitle="Vokis will be added automatically to this album after you complete them for the first time"
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

						<div class="times-taken">
							<span class="times-count">{voki.taken.timesTaken}</span>
							<span class="times-label">
								{voki.taken.timesTaken === 1 ? 'time taken' : 'times taken'}
							</span>
						</div>

						<a href="/catalog/{voki.vokiId}" class="open-voki-page">
							Open Vokis page
						</a>
						<label class="date">
							Last time taken: <span use:relativeTime={{ date: voki.taken.lastTimeTaken }} />
						</label>
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
		width: 100%;
		border-radius: calc(var(--voki-cover-border-radius) * 1.25);
		background-color: var(--back);
		box-shadow: var(--shadow-xs);
		grid-template-columns: 20rem 1fr auto auto;
	}

	.voki-name {
		margin: 0.5rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 500;
		text-indent: 0.125em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.name-and-authors {
		display: flex;
		flex-direction: column;
		padding-right: 1rem;
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
		display: flex;
		flex-wrap: wrap;
		gap: 0.25rem;
		margin: 0 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.times-taken {
		place-self: center center;
		display: flex;
		flex-direction: column;
		align-items: center;
		padding: 0.5rem 1rem;
	}

	.times-count {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 600;
	}

	.times-label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
	}

	.date {
		position: absolute;
		right: 1rem;
		bottom: 1rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.open-voki-page {
		margin: 0 2rem;
		color: var(--primary);
		font-size: 0.95rem;
		text-decoration: none;
		place-self: center center;
	}
</style>
