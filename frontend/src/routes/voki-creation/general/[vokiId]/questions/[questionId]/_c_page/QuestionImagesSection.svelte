<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import FieldNotSetLabel from '../../../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationDefaultButton from '../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import type { GeneralVokiCreationQuestionImageSet } from '../types';
	import QuestionImagesEditingDialog from './_c_images_section/QuestionImagesEditingDialog.svelte';

	interface Props {
		savedImageSet: GeneralVokiCreationQuestionImageSet;
		questionId: string;
		vokiId: string;
		updateSavedImageSet: (newImageSet: GeneralVokiCreationQuestionImageSet) => void;
	}
	let { savedImageSet, questionId, vokiId, updateSavedImageSet }: Props = $props();
	let dialogElement = $state<QuestionImagesEditingDialog>()!;
</script>

<QuestionImagesEditingDialog
	bind:this={dialogElement}
	{questionId}
	{vokiId}
	updateParent={updateSavedImageSet}
/>
{#if savedImageSet.keys.length === 0}
	<div class="field">
		<VokiCreationFieldName fieldName="Images:" />
		<FieldNotSetLabel text="No images selected" />
	</div>
{:else}
	<div class="field"><VokiCreationFieldName fieldName="Images:" /></div>
	<div class="images-container">
		{#each savedImageSet.keys as image}
			<img
				src={StorageBucketMain.fileSrc(image)}
				alt="question-img"
				style="aspect-ratio: {savedImageSet.width} / {savedImageSet.height};"
			/>
		{/each}
	</div>
	<div class="field aspect-ratio">
		<VokiCreationFieldName fieldName="Images aspect ratio:" />
		<label>{savedImageSet.width} : {savedImageSet.height}</label>
	</div>
{/if}
<VokiCreationDefaultButton text="Edit images" onclick={() => dialogElement.open(savedImageSet)} />

<style>
	.field {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin-top: 1.5rem;
	}

	.images-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: start;
		gap: 1rem;
		width: 100%;
		margin-top: 1rem;
	}

	.images-container img {
		height: 14rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.field.aspect-ratio > label {
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 450;
		text-decoration: none;
	}
</style>
