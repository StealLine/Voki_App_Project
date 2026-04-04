<script lang="ts">
	import type { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { VokiUtils, type GeneralVokiResultsVisibility } from '$lib/ts/voki';

	interface Props {
		authState: AuthStore.AuthState;
		resultsVisibility: GeneralVokiResultsVisibility;
		resultsCount: number;
		vokiId: string;
		vokiResultIdsReceivedByUser: string[];
		allVokiResultIds: string[];
	}
	let {
		authState,
		resultsVisibility,
		resultsCount,
		vokiId,
		vokiResultIdsReceivedByUser,
		allVokiResultIds
	}: Props = $props();
</script>

{#if VokiUtils.canUserSeeAllGeneralVokiResults(resultsVisibility, vokiResultIdsReceivedByUser, allVokiResultIds)}
	<a class="see-all-btn" href={`/take-voki/${vokiId}/general/results/all`}
		>View all ({resultsCount}) results</a
	>
{:else if authState.isAuthenticated && resultsVisibility === 'OnlyReceived'}
	<p class="not-all-results-received-msg">
		Voki author decided that only received results should be visible to users. You can try to take
		the Voki again to get a new result or use the link below to see your received results.
	</p>
{:else if resultsVisibility === 'AfterTaking'}
	<p class="not-all-results-received-msg">
		Voki author decided that only received results should be visible only after taking the Voki.
		Please take the Voki to see its results
	</p>
{/if}

<style>
	.see-all-btn {
		width: fit-content;
		padding: 0.25rem 2rem;
		margin-top: 1rem;
		border-radius: 0.375rem;
		background: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 400;
		letter-spacing: 0.5px;
		transition: all 0.12s ease;
		cursor: pointer;
		align-self: center;
	}

	.see-all-btn:hover {
		background: var(--primary-hov);
	}
</style>
