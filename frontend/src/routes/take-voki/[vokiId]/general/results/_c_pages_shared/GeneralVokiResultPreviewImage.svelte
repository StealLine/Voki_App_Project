<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	let { resultImage, resultName }: { resultImage: string | null; resultName: string } = $props<{
		resultImage: string | null;
		resultName: string;
	}>();
</script>

<div class="img-container">
	{#if resultImage}
		<img
			class="result-image"
			src={StorageBucketMain.fileSrc(resultImage)}
			alt={`Image for result "${resultName}"`}
		/>
	{:else}
		<div class="no-image">
			<svg><use href="#no-image-icon" /></svg>
			<span>No image</span>
		</div>
	{/if}
</div>

<style>
	.img-container {
		display: flex;
		justify-content: center;
		align-items: center;
		width: fit-content;
		height: fit-content;
	}

	.result-image {
		width: 100%;
		height: 100%;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs);
		object-fit: contain;
	}

	.no-image {
		display: grid;
		padding: 0.375rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.75rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		aspect-ratio: 1/1;
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.no-image svg {
		width: 100%;
		height: 100%;
		stroke-width: 1.125;
	}

	.no-image span {
		padding: 0.25rem 0;
		font-size: 0.875rem;
		font-weight: 500;
	}
</style>
