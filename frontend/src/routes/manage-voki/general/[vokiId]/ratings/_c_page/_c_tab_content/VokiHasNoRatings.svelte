<script lang="ts">
	import { browser } from '$app/environment';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import TakeAndRetrieveNewSnapshotButton from './_c_shared/TakeAndRetrieveNewSnapshotButton.svelte';
	import { DateUtils } from '$lib/ts/utils/date-utils';

	interface Props {
		vokiId: string;
		lastSnapshotDate: Date | null;
		onTakeAndRetrieveRatingsSnapshotBtnClicked: () => void;
		snapshotsRetrievingState: { name: 'ok' } | { name: 'loading' } | { name: 'errs'; errs: Err[] };
	}
	let {
		vokiId,
		lastSnapshotDate,
		onTakeAndRetrieveRatingsSnapshotBtnClicked,
		snapshotsRetrievingState
	}: Props = $props();

	const rateVokiPath = $derived(`/catalog/${vokiId}?tab=ratings`);
	const catalogPath = $derived(`/catalog/${vokiId}`);
	const shareUrl = $derived(browser ? `${location.origin}${rateVokiPath}` : rateVokiPath);

	async function onCopyLinkClick() {
		try {
			if (browser && navigator?.clipboard?.writeText) {
				await navigator.clipboard.writeText(shareUrl);
				toast.success('Link copied to clipboard');
			} else {
				toast.error('Failed to copy link to clipboard');
			}
		} catch {
			toast.error('Failed to copy link to clipboard');
		}
	}

	function onShareInputClick(e: MouseEvent) {
		const el = e.currentTarget as HTMLInputElement;
		el.focus();
		el.select();
	}
</script>

<div class="no-ratings-message">
	<h1 class="title">
		Last time we checked this Voki didn’t have any ratings yet
		<TakeAndRetrieveNewSnapshotButton
			{onTakeAndRetrieveRatingsSnapshotBtnClicked}
			{snapshotsRetrievingState}
		/>
	</h1>
	{#if lastSnapshotDate}
		<p class="subtitle last-check">Last check: {DateUtils.toLocale(lastSnapshotDate)}</p>
	{/if}
	<p class="subtitle">You can invite your friends to take the Voki and leave the first ratings</p>

	<div class="share">
		<div class="share-label">Share the ratings link</div>

		<div class="share-row">
			<input
				class="share-input share-row-el"
				readonly
				value={shareUrl}
				onclick={onShareInputClick}
				aria-label="Share link"
			/>

			<button
				class="icon-btn share-row-el"
				type="button"
				onclick={onCopyLinkClick}
				aria-label="Copy link"
			>
				<svg class="icon"><use href="#context-menu-copy-link-icon" /></svg>
			</button>
		</div>
	</div>

	<div class="actions">
		<a class="btn primary" href={rateVokiPath}>Open ratings tab</a>
		<a class="btn secondary" href={catalogPath}>Open in catalog</a>
	</div>
</div>

<style>
	.no-ratings-message {
		display: flex;
		flex-direction: column;
		min-height: 100%;
		padding: 2rem 4rem;
	}
	.title {
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.875rem;
		font-weight: 700;
		line-height: 1;
		display: flex;
		align-items: center;
		gap: 1rem;
	}
	.last-check {
		margin-top: 0.25rem;
		margin-bottom: 2rem;
	}
	.subtitle {
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 425;
	}

	.share {
		padding: 1rem;
		margin-top: 0.25rem;
		border-radius: 1rem;
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.share-label {
		margin-bottom: 0.75rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
	}

	.share-row {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		height: 2.75rem;
	}

	.share-row-el {
		height: 100%;
		border-radius: 0.75rem;
		box-shadow: var(--shadow-xs);
	}

	.share-row-el:hover {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.share-input {
		width: 100%;
		padding: 0 1rem;
		border: none;
		background: var(--back);
		color: var(--text);
		font-size: 1rem;
		cursor: default;
		outline: none;
	}

	.icon-btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		height: 100%;
		padding: 0.5rem;
		border: none;
		background: var(--secondary);
		color: var(--muted-foreground);
		cursor: default;
		flex: 0 0 auto;
		aspect-ratio: 1/1;
	}

	.icon-btn:active {
		transform: scale(0.97);
	}

	.icon {
		height: 100%;
		aspect-ratio: 1/1;
		stroke-width: 2;
	}

	.actions {
		display: flex;
		flex-wrap: wrap;
		gap: 0.75rem;
		margin-top: 1.6rem;
	}

	.btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		padding: 0.75rem 1.25rem;
		border-radius: 0.75rem;
		font-size: 1rem;
		font-weight: 550;
	}

	.btn.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover {
		background: var(--primary-hov);
	}

	.btn.secondary {
		background: var(--muted);
		color: var(--muted-foreground);
	}

	.btn.secondary:hover {
		background: var(--accent);
		color: var(--accent-foreground);
	}
</style>
