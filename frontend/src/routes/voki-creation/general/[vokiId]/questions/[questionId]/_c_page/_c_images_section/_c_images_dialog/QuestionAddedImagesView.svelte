<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import GeneralVokiAddQuestionImage from './GeneralVokiAddQuestionImage.svelte';
	interface Props {
		maxImagesCount: number;
		images: string[];
		widthRatio: number;
		heightRatio: number;
		removeImage: (image: string) => void;
		uploadAndAddImage: (file: File) => Promise<void>;
	}

	let { maxImagesCount, images, widthRatio, heightRatio, removeImage, uploadAndAddImage }: Props =
		$props();
</script>

<div class="imgs-container">
	{#each images as img}
		<div class="img-view unselectable">
			<img
				alt=""
				src={StorageBucketMain.fileSrc(img)}
				style="aspect-ratio: {widthRatio} / {heightRatio};"
			/>
			<button class="remove-img-btn" onclick={() => removeImage(img)}>
				<svg> <use href="#common-cross-icon" /></svg>
			</button>
		</div>
	{/each}
	{#if images.length < maxImagesCount}
		<GeneralVokiAddQuestionImage uploadImage={uploadAndAddImage} {widthRatio} {heightRatio} />
	{/if}
</div>

<style>
	.imgs-container {
		display: flex;
		gap: 1rem;
		width: min(80rem, 90vw);
		height: 18rem;
		padding: 0.75rem 0.5rem 0.25rem;
		margin: 0 auto 2rem;
		transition: all 0.12s ease;
		overflow: auto hidden;
		overscroll-behavior: contain;
	}

	.imgs-container::-webkit-scrollbar {
		height: 0.675rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
	}

	.imgs-container::-webkit-scrollbar-thumb {
		border-radius: 0.25rem;
		background-color: var(--primary);
	}

	.imgs-container::-webkit-scrollbar-thumb:hover {
		background-color: var(--primary);
	}

	.img-view {
		position: relative;
		display: flex;
		justify-content: center;
		align-items: center;
		width: fit-content;
		height: 100%;
		border-radius: 1rem;
	}

	.img-view > img {
		z-index: 1;
		height: 100%;
		border-radius: 0.75rem;
		object-fit: fill;
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.remove-img-btn {
		position: absolute;
		top: 0;
		right: 0;
		z-index: 2;
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.25rem;
		border: none;
		border-radius: 50%;
		background-color: var(--secondary-foreground);
		transition: opacity 0.08s ease;
		transform: translate(40%, -40%);
		cursor: pointer;
		outline: none;
	}

	.img-view:has(img:hover) .remove-img-btn {
		opacity: 0.3;
	}

	.remove-img-btn:hover {
		background-color: var(--primary-hov);
	}

	.remove-img-btn > svg {
		width: 1rem;
		height: 1rem;
		color: var(--primary-foreground);
		stroke-width: 2.3;
	}
</style>
