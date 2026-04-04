<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { LanguageUtils } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import FieldNotSetLabel from '../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import DetailsSectionEditState from './_c_details_section/DetailsSectionEditState.svelte';
	import VokiCreationDefaultButton from '../../VokiCreationDefaultButton.svelte';

	interface Props {
		savedDetails: VokiDetails;
		vokiId: string;
		isEditing: boolean;
		updateSavedVokiDetails: (newDetails: VokiDetails) => void;
	}
	let { savedDetails, vokiId, isEditing = $bindable(), updateSavedVokiDetails }: Props = $props();
</script>

<div class="voki-details-section">
	{#if isEditing}
		<DetailsSectionEditState
			{vokiId}
			{savedDetails}
			updateSavedDetails={(newDetails) => (savedDetails = newDetails)}
			cancelEditing={() => (isEditing = false)}
		/>
	{:else}
		<p class="field">
			<VokiCreationFieldName fieldName="Description:" />
			{#if StringUtils.isNullOrWhiteSpace(savedDetails.description)}
				<FieldNotSetLabel text="No description" class="no-description" />
			{:else}
				{savedDetails.description}
			{/if}
		</p>
		<p class="field">
			<VokiCreationFieldName fieldName="Language:" />
			{LanguageUtils.name(savedDetails.language)}
		</p>
		<p class="field">
			<VokiCreationFieldName fieldName="Mature content:" />
			{#if savedDetails.hasMatureContent}
				Contains mature content
			{:else}
				No mature content
			{/if}
		</p>
		<VokiCreationDefaultButton text="Edit details" onclick={() => (isEditing = true)} />
	{/if}
</div>

<style>
	.voki-details-section {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.field:not(:has(:global(.no-description))) {
		width: 100%;
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 500;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.field:has(:global(.no-description)) {
		display: flex;
		flex-direction: row;
		align-items: center;
	}
</style>
