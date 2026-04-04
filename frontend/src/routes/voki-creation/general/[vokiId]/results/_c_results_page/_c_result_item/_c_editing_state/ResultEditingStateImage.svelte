<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';

	let {
		image = $bindable(),
		errs = $bindable(),
		resultId
	}: { image: string | null; errs: Err[]; resultId: string } = $props<{
		image: string | null;
		errs: Err[];
		resultId: string;
	}>();
	async function handleInputChange(event: Event) {
		errs = [];
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			const file = files?.[0];
			if (!file.type.startsWith('image/')) {
				return;
			}
			isLoadingImage = true;
			const response = await StorageBucketMain.uploadTempImage(file);
			if (response.isSuccess) {
				image = response.data;
			} else {
				errs = response.errs;
			}
			isLoadingImage = false;
		}
	}
	async function removeResultImage() {
		image = null;
	}
	let isLoadingImage = $state(false);
</script>

{#if isLoadingImage}
	<div class="loading-container img-not-set">
		<CubesLoader sizeRem={3} color="var(--primary)" />
	</div>
{:else if image}
	<div class="image-set">
		<img class="result-image" src={StorageBucketMain.fileSrc(image)} alt="result" />
		<label for="result-image-{resultId}" class="img-btn change-btn unselectable">Change image</label
		>
		<input
			type="file"
			id="result-image-{resultId}"
			accept=".jpg,.png,.webp"
			hidden
			onchange={handleInputChange}
		/>
		<button class="img-btn remove-btn unselectable" onclick={() => removeResultImage()}
			>Remove image</button
		>
	</div>
{:else}
	<label class="upload-button img-not-set unselectable">
		<svg><use href="#add-image-icon" /></svg>
		<span>Add image</span>
		<input type="file" accept=".jpg,.png,.webp" onchange={handleInputChange} hidden />
	</label>
{/if}

<style>
	.img-not-set {
		width: 6rem;
		height: 6rem;
		margin: 0.5rem;
	}

	.image-set {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
	}

	.result-image {
		max-width: 15rem;
		max-height: 18rem;
		object-fit: contain;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs);
	}

	.img-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 90%;
		min-width: 11rem;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		font-weight: 420;
		letter-spacing: 0.2px;
		box-shadow: var(--shadow);
		cursor: pointer;
	}

	.change-btn {
		margin-top: 1rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.remove-btn {
		margin-top: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.remove-btn:hover {
		background-color: var(--red-1);
		color: var(--red-5);
	}

	.upload-button {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		border: none;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		box-shadow: var(--shadow-xs);
	}

	.upload-button > svg {
		width: 2rem;
		height: 2rem;
		color: inherit;
		stroke-width: 1.3;
	}

	.upload-button:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		box-shadow: none;
	}
</style>
