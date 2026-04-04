<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';

	interface Props {
		onImageUploaded: (tempKey: string) => void;
	}
	let { onImageUploaded }: Props = $props();

	let isLoading = $state(false);
	let errs: Err[] = $state([]);

	async function handleFile(file: File) {
		if (!file.type.startsWith('image/')) {
			errs = [{ message: 'File is not an image' }];
			return;
		}
		isLoading = true;
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			onImageUploaded(response.data);
		} else {
			errs = response.errs;
		}
		isLoading = false;
	}

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			await handleFile(files[0]);
		}
	}

	async function ondrop(event: DragEvent) {
		event.preventDefault();
		isDragging = false;
		if (event.dataTransfer?.files?.length) {
			await handleFile(event.dataTransfer.files[0]);
		}
	}

	function ondragover(event: DragEvent) {
		event.preventDefault();
		isDragging = Array.from(event.dataTransfer?.items || []).some((item) =>
			item.type.startsWith('image/')
		);
	}

	function ondragleave() {
		isDragging = false;
	}
	let isDragging = $state(false);
</script>

<div
	class="img-input-container"
	class:loading={isLoading}
	class:dragging={isDragging}
	ondrop={(e) => ondrop(e)}
	{ondragover}
	{ondragleave}
>
	{#if isLoading}
		<div class="loading-container">
			<CubesLoader sizeRem={5} color="var(--primary)" />
		</div>
	{:else}
		<h1>Select new Voki cover</h1>

		<p>Drag and drop image or click the button below</p>
		<label class="upload-button unselectable" class:dragging={isDragging}>
			<svg><use href="#add-image-icon" /></svg>
			<span>Select image</span>
			<input type="file" accept="image/*" onchange={handleInputChange} hidden />
		</label>
	{/if}
</div>
<DefaultErrBlock errList={errs} class="img-input-errs-block" />

<style>
	.img-input-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
		margin: 1rem;
		border: 0.125rem dashed var(--muted);
		border-radius: 1rem;
		text-align: center;
		transition: all 0.12s ease-in;
	}

	.img-input-container.dragging:not(.loading) {
		border-color: var(--primary);
		background-color: var(--secondary);
	}

	.loading-container {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}

	.upload-button {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.375rem 1.25rem;
		margin-top: 1rem;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		transition: inherit;
		cursor: pointer;
	}

	.upload-button svg {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 2;
		fill: var(--primary-foreground);
	}

	.upload-button:hover {
		background-color: var(--primary-hov);
	}

	.upload-button.dragging {
		background-color: var(--primary-hov);
		transform: scale(1.06);
	}

	h1 {
		margin-bottom: 0.5rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 700;
		letter-spacing: 0.5px;
		transition: inherit;
		cursor: default;
	}

	p {
		margin-bottom: 1rem;
		color: var(--secondary-foreground);
		font-weight: 440;
		cursor: default;
	}

	:global(.img-input-errs-block) {
		margin-top: 0.5rem;
	}
</style>
