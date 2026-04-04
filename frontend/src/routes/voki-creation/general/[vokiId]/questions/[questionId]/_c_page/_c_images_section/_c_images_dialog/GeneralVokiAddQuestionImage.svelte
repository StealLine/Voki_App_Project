<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	interface Props {
		widthRatio: number;
		heightRatio: number;
		uploadImage: (image: File) => Promise<void>;
	}

	let { widthRatio, heightRatio, uploadImage }: Props = $props();

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			const file = files?.[0];
			if (!file.type.startsWith('image/')) {
				return;
			}
			isLoading = true;
			await uploadImage(file);
			isLoading = false;
		}
	}
	let isLoading = $state(false);
</script>

{#if isLoading}
	<div class="loading-container" style="aspect-ratio: {widthRatio} / {heightRatio};">
		<CubesLoader sizeRem={4}  color= 'var(--primary)'/>
		<h1>Uploading image...</h1>
	</div>
{/if}
<label class="upload-button unselectable">
	<svg><use href="#add-image-icon" /></svg>
	<span>Add image</span>
	<input type="file" accept="image/*" onchange={handleInputChange} hidden />
</label>

<style>
	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		height: 100%;
		padding: 0 1rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		animation: var(--default-fade-in-animation);
	}

	.loading-container > h1 {
		margin: 1rem 0 2rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 550;
		text-align: center;
		letter-spacing: 0.5px;
	}

	.upload-button {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		height: 100%;
		padding: 0 1rem;
		border: 0.1875rem dashed var(--secondary-foreground);
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
		letter-spacing: 0.25px;
		transition: inherit;
		cursor: pointer;
		white-space: nowrap;
	}

	.upload-button svg {
		width: 3.5rem;
		height: 3.5rem;
		stroke-width: 1.5;
	}

	.upload-button:hover {
		border-color: var(--accent-foreground);
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
