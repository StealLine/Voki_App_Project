<script lang="ts">
	import type { Err } from '$lib/ts/err';

	interface Props {
		onTakeAndRetrieveRatingsSnapshotBtnClicked: () => void;
		snapshotsRetrievingState: { name: 'ok' } | { name: 'loading' } | { name: 'errs'; errs: Err[] };
	}
	let { onTakeAndRetrieveRatingsSnapshotBtnClicked, snapshotsRetrievingState }: Props = $props();
	function onBtnClick() {
		if (snapshotsRetrievingState.name === 'ok') {
			onTakeAndRetrieveRatingsSnapshotBtnClicked();
		}
	}
</script>

<button
	class="btn"
	type="button"
	onclick={onBtnClick}
	disabled={snapshotsRetrievingState.name === 'loading'}
	title="Reload"
>
	{#if snapshotsRetrievingState.name === 'loading'}
		<div class="spinner" />
	{:else}
		<svg class="reload-arrow"><use href="#common-reload-icon" /></svg>
	{/if}
</button>

<style>
	.btn {
		height: 1.875rem;
		width: 1.875rem;
		display: inline-flex;
		justify-content: center;
		align-items: center;
		border-radius: 0.5rem;
		border: none;
		background: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
	}

	.reload-arrow {
		width: 1.125rem;
		height: 1.125rem;
		stroke-width: 2.25;
		transition: transform 0.3s ease-out;
	}
	.btn:has(.reload-arrow):hover .reload-arrow {
		transform: rotate(60deg);
		background-color: var(--primary-hov);
	}
	.btn:has(.reload-arrow):active .reload-arrow {
		transform: rotate(120deg) scale(0.96);
	}

	.spinner {
		width: 1.25rem;
		height: 1.25rem;
		border-radius: 50%;
		border: 0.125rem solid var(--primary-foreground);
		border-top: 0.1875rem solid transparent;
		animation: spin 0.6s linear infinite;
	}

	@keyframes spin {
		from {
			transform: rotate(0deg);
		}
		to {
			transform: rotate(360deg);
		}
	}
</style>
