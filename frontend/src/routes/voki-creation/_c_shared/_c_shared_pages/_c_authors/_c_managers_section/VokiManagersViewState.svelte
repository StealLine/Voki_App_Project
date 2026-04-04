<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import VokiCreationDefaultButton from '../../../VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import type { VokiExpectedManagersSetting } from '../types';

	interface Props {
		setting: VokiExpectedManagersSetting;
		viewerId: string;
		isViewerPrimaryAuthor: boolean;
		startEditing: () => void;
	}
	let { setting, viewerId, isViewerPrimaryAuthor, startEditing }: Props = $props();
	const policyKind = $derived.by(() => {
		if (setting.makeAllCoAuthorsManagers) {
			return 'all' as const;
		}
		if (setting.userIdsToBecomeManagers.length === 0) {
			return 'none' as const;
		}
		return 'selected' as const;
	});

	const selectedCount = $derived(setting.userIdsToBecomeManagers.length);

	const viewerWillBeManager = $derived.by(() => {
		if (setting.makeAllCoAuthorsManagers) {
			return true;
		}
		return setting.userIdsToBecomeManagers.includes(viewerId);
	});
</script>

<div class="top">
	<div class="title">
		<VokiCreationFieldName fieldName="Voki managers:" />
		<span class="chip">
			{#if policyKind === 'all'}
				All co-authors
			{:else if policyKind === 'none'}
				None
			{:else}
				Selected ({selectedCount})
			{/if}
		</span>
	</div>

	{#if isViewerPrimaryAuthor}
		<VokiCreationDefaultButton text="Edit" onclick={startEditing} />
	{/if}
</div>

{#if policyKind === 'all'}
	<p class="policy-msg">
		When voki will be published, every co-author will automatically become a manager
	</p>
{:else if policyKind === 'none'}
	<p class="policy-msg">No co-authors will become managers after publishing</p>
{:else}
	<p class="policy-msg">Only selected co-authors will become managers after publishing</p>
	{#if isViewerPrimaryAuthor}
		<div class="selected-co-authors-container">
			{#each setting.userIdsToBecomeManagers as managerId (managerId)}
				<BasicUserDisplay userId={managerId} interactionLevel={'WholeComponentLink'} />
			{/each}
		</div>
	{/if}
{/if}

{#if !isViewerPrimaryAuthor}
	<div class="you-row">
		<span class="you-label">You:</span>
		<span class="you-value">
			{#if viewerWillBeManager}
				Will become a manager
			{:else}
				Will not become a manager
			{/if}
		</span>
	</div>
{/if}

<style>
	.top {
		display: flex;
		justify-content: space-between;
		align-items: center;
		gap: 0.75rem;
	}

	.top > :global(.btn) {
		margin: 0;
	}

	.title {
		display: flex;
		align-items: center;
		gap: 0.5rem;
	}

	.chip {
		padding: 0.25rem 0.875rem;
		border-radius: 999rem;
		background: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		box-shadow: var(--shadow-xs);
		white-space: nowrap;
	}

	.policy-msg {
		margin: 0.5rem 0;
		color: var(--secondary-foreground);
	}

	.selected-co-authors-container {
		display: flex;
		flex-flow: row wrap;
		gap: 2rem;
		margin-top: 0;
	}

	.you-row {
		display: flex;
		justify-content: space-between;
		align-items: center;
		gap: 0.75rem;
		padding: 0.75rem;
		border-radius: 0.75rem;
		background: var(--muted);
	}

	.you-label {
		color: var(--muted-foreground);
		font-weight: 450;
	}

	.you-value {
		color: var(--text);
		font-weight: 600;
		letter-spacing: 0.125px;
	}
</style>
