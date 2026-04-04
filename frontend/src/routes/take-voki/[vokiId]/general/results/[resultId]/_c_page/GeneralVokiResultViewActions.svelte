<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import type { GeneralVokiResultsVisibility } from '$lib/ts/voki';
	import ViewAllResultsActionButton from './_c_result_view_actions/ViewAllResultsActionButton.svelte';

	interface Props {
		resultsCount: number;
		resultsVisibility: GeneralVokiResultsVisibility;
		vokiId: string;
		vokiResultIdsReceivedByUser: string[];
		allVokiResultIds: string[];
	}
	let {
		resultsCount,
		resultsVisibility,
		vokiId,
		vokiResultIdsReceivedByUser,
		allVokiResultIds
	}: Props = $props();
</script>

<AuthView>
	{#snippet children(authState)}
		<ViewAllResultsActionButton
			{authState}
			{resultsCount}
			{resultsVisibility}
			{vokiId}
			{vokiResultIdsReceivedByUser}
			{allVokiResultIds}
		/>
		{#if authState.name === 'authenticated'}
			<a class="see-received-btn" href={`/take-voki/${vokiId}/general/results/received`}
				>See my received results</a
			>
		{/if}
	{/snippet}
</AuthView>

<style>
	.see-received-btn {
		align-self: center;
		width: fit-content;
		padding: 0.125rem 1rem;
		border-radius: 0.375rem;
		background-color: var(--back-main);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		text-decoration: underline;
		transition: letter-spacing 0.2s ease;
		cursor: pointer;
	}

	.see-received-btn:hover {
		background-color: var(--muted);
		letter-spacing: 0.5px;
	}
</style>
