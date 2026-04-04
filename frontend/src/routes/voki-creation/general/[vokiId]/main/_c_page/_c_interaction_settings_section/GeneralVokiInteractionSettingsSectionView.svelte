<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import type { GeneralVokiInteractionSettings } from '../../types';

	interface Props {
		settings: GeneralVokiInteractionSettings;
		startEditing: () => void;
	}
	let { settings, startEditing }: Props = $props();
</script>

<p class="field-p">
	<VokiCreationFieldName fieldName="Voki taking:" />
	{settings.signedInOnlyTaking ? 'Signed in only' : 'No authentication required'}
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Results visibility:" />
	{#if settings.resultsVisibility === 'Anyone'}
		Anyone
	{:else if settings.resultsVisibility === 'AfterTaking'}
		Only after Voki taking
	{:else if settings.resultsVisibility === 'OnlyReceived'}
		Only received results
	{:else}
		Unknown value
	{/if}
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Results distribution:" />
	{settings.showResultsDistribution ? 'Show' : 'Hide'}
</p>
<VokiCreationDefaultButton text="Edit settings" onclick={() => startEditing()} />

<style>
	.field-p {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		width: max-content;
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 500;
	}
</style>
