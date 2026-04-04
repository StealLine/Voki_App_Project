<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import QuestionHasNoImages from './_c_images_dialog/QuestionHasNoImages.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiCreationQuestionImageSet } from '../../types';
	import QuestionImagesAspectRationInput from './_c_images_dialog/QuestionImagesAspectRationInput.svelte';
	import AddedImagesView from './_c_images_dialog/QuestionAddedImagesView.svelte';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	interface Props {
		questionId: string;
		vokiId: string;
		updateParent: (imageSet: GeneralVokiCreationQuestionImageSet) => void;
	}
	let { questionId, vokiId, updateParent }: Props = $props();

	let images: string[] = $state<string[]>([]);
	let aspectRatio: { width: number; height: number } = $state({ width: 1, height: 1 });

	let dialogElement = $state<DialogWithCloseButton>()!;
	let errs = $state<Err[]>([]);
	async function saveData() {
		errs = [];
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			newKeys: string[];
			newWidth: number;
			newHeight: number;
		}>(
			`/vokis/${vokiId}/questions/${questionId}/update-images`,
			RJO.PATCH({
				newImages: images,
				width: aspectRatio.width,
				height: aspectRatio.height
			})
		);
		if (response.isSuccess) {
			updateParent({
				keys: response.data.newKeys,
				width: response.data.newWidth,
				height: response.data.newHeight
			});
			dialogElement.close();
		} else {
			errs = response.errs;
		}
	}
	export function open(imageSet: GeneralVokiCreationQuestionImageSet) {
		images = imageSet.keys;
		aspectRatio = { width: imageSet.width, height: imageSet.height };
		errs = [];
		dialogElement.open();
	}
	async function uploadAndAddImage(file: File): Promise<void> {
		errs = [];
		const formData = new FormData();
		formData.append('file', file);
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			images = [...images, response.data];
		} else {
			errs = response.errs;
		}
	}
	function removeImage(img: string) {
		errs = [];
		images = images.filter((i) => i != img);
	}
	const maxImagesCount = 5;
</script>

<DialogWithCloseButton
	bind:this={dialogElement}
	dialogId="voki-creation-question-images-dialog"
	subheading={images.length === 0 ? 'Add question images' : undefined}
>
	{#if images.length == 0}
		<QuestionHasNoImages uploadImage={uploadAndAddImage} />
	{:else}
		<p class="subheading">Question images ({images.length}/{maxImagesCount})</p>
		<AddedImagesView
			{maxImagesCount}
			{images}
			widthRatio={aspectRatio.width}
			heightRatio={aspectRatio.height}
			{removeImage}
			{uploadAndAddImage}
		/>
		<p class="subheading">Choose images aspect ratio</p>
		<QuestionImagesAspectRationInput bind:aspectRatio />
	{/if}
	{#if errs.length > 0}
		<DefaultErrBlock errList={errs} />
	{/if}
	<PrimaryButton onclick={() => saveData()}>Save</PrimaryButton>
</DialogWithCloseButton>

<style>
	:global(#voki-creation-question-images-dialog .dialog-content) {
		display: flex;
		flex-direction: column;
	}

	:global(#voki-creation-question-images-dialog .dialog-content > .primary-btn) {
		align-self: center;
	}

	.subheading {
		margin: 0 0 0.5rem;
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 550;
		text-align: center;
	}
</style>
