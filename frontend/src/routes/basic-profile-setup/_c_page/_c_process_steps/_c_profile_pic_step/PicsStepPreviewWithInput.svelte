<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { toast } from 'svelte-sonner';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	interface Props {
		setCurrent: (newPic: string) => void;
		currentPic: string;
	}
	let { setCurrent, currentPic }: Props = $props();
	let isLoading = $state(false);
	let isDragging = $state(false);

	async function handleFile(file: File) {
		if (!file.type.startsWith('image/')) {
			toast.error('File is not an image');
			return;
		}
		isLoading = true;
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			setCurrent(response.data);
		} else {
			toast.error('Something went wrong. Please try again.');
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
</script>

<div
	class="preview-container"
	class:dragging={isDragging}
	class:loading={isLoading}
	{ondragover}
	{ondragleave}
	ondrop={(e) => ondrop(e)}
>
	{#if isDragging}
		<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
			<path
				fill="currentColor"
				stroke="currentColor"
				d="M12.2 21.95c-0.6333 0 -1.24165 -0.04585 -1.825 -0.1375 -0.5833 -0.09165 -1.14165 -0.22915 -1.675 -0.4125 -0.163 -0.05215 -0.2954 -0.1434 -0.39725 -0.27375 -0.1018 -0.13015 -0.15275 -0.2865 -0.15275 -0.469 0 -0.2215 0.07425 -0.40725 0.22275 -0.55725 0.1487 -0.15 0.33275 -0.225 0.55225 -0.225 0.08335 0 0.175 0.01665 0.275 0.05 0.45 0.16665 0.9417 0.29165 1.475 0.375 0.53335 0.08335 1.0417 0.13335 1.525 0.15 0.2125 0 0.3907 0.07235 0.5345 0.217 0.1437 0.1445 0.2155 0.32365 0.2155 0.5375 0 0.21365 -0.0718 0.39135 -0.2155 0.533 -0.1438 0.14165 -0.322 0.2125 -0.5345 0.2125Zm4.0955 -0.9c-0.21365 0 -0.3913 -0.0719 -0.533 -0.21575 -0.14165 -0.14365 -0.2125 -0.32175 -0.2125 -0.53425V16c0 -0.2125 0.07195 -0.39065 0.21575 -0.5345 0.1437 -0.14365 0.32175 -0.2155 0.53425 -0.2155h4.3c0.2125 0 0.3907 0.07235 0.5345 0.217 0.1437 0.1445 0.2155 0.32365 0.2155 0.5375 0 0.21365 -0.0718 0.39135 -0.2155 0.533 -0.1438 0.14165 -0.322 0.2125 -0.5345 0.2125h-2.5l2.75 2.75c0.15 0.15 0.225 0.325 0.225 0.525s-0.075 0.375 -0.225 0.525c-0.15 0.15 -0.325 0.225 -0.525 0.225s-0.375 -0.075 -0.525 -0.225l-2.75 -2.75v2.5c0 0.2125 -0.0723 0.3906 -0.217 0.53425 -0.1445 0.14385 -0.32365 0.21575 -0.5375 0.21575ZM5.45 19.2c-0.1 0 -0.1958 -0.01665 -0.2875 -0.05 -0.09165 -0.03335 -0.17806 -0.0905 -0.259225 -0.1715 -0.5355 -0.519 -1.007415 -1.091 -1.41575 -1.716 -0.408335 -0.625 -0.729165 -1.3125 -0.9625 -2.0625l-0.05 -0.275c0 -0.2125 0.0745 -0.39065 0.2235 -0.5345 0.149165 -0.14365 0.333915 -0.2155 0.55425 -0.2155 0.1815 0 0.33475 0.05 0.45975 0.15 0.125 0.1 0.2125 0.225 0.2625 0.375 0.212 0.62015 0.485835 1.1961 0.8215 1.72775 0.335675 0.5315 0.727175 1.0209 1.174475 1.46825 0.086 0.086 0.1457 0.1665 0.179 0.2415 0.03335 0.075 0.05 0.17085 0.05 0.2875 0 0.21965 -0.0718 0.40375 -0.2155 0.55225 -0.1438 0.1485 -0.322 0.22275 -0.5345 0.22275Zm15.7705 -6.55c-0.21365 0 -0.3913 -0.0719 -0.533 -0.21575 -0.14165 -0.14365 -0.2125 -0.32175 -0.2125 -0.53425 -0.01665 -0.45 -0.0583 -0.9125 -0.125 -1.3875 -0.06665 -0.475 -0.19165 -0.92915 -0.375 -1.3625 -0.01665 -0.03335 -0.04165 -0.15835 -0.075 -0.375 0 -0.2125 0.07275 -0.39065 0.21825 -0.5345 0.1455 -0.14365 0.32585 -0.2155 0.541 -0.2155 0.1772 0 0.32825 0.0486 0.45325 0.14575 0.125 0.09735 0.2125 0.22375 0.2625 0.37925 0.2 0.53335 0.35 1.07915 0.45 1.6375 0.1 0.55835 0.15 1.12915 0.15 1.7125 0 0.2125 -0.0723 0.3906 -0.217 0.53425 -0.1445 0.14385 -0.32365 0.21575 -0.5375 0.21575ZM2.774525 11.725c-0.216335 0 -0.395335 -0.07075 -0.537 -0.21225 -0.141665 -0.1415 -0.2125 -0.31675 -0.2125 -0.52575v-0.112c0.083335 -0.76665 0.25 -1.5125 0.5 -2.2375 0.25 -0.725 0.583335 -1.40415 1 -2.0375 0.063665 -0.09815 0.152915 -0.1811 0.26775 -0.24875 0.114665 -0.0675 0.242085 -0.10125 0.38225 -0.10125 0.216665 0 0.395835 0.0729 0.5375 0.21875 0.141665 0.14585 0.2125 0.3265 0.2125 0.542 0 0.07615 -0.011915 0.1496 -0.03575 0.22025 -0.023835 0.0705 -0.053585 0.13515 -0.08925 0.194 -0.316665 0.56665 -0.583665 1.1539 -0.801 1.76175 -0.217165 0.60785 -0.375165 1.2289 -0.474 1.86325 -0.016665 0.18335 -0.095 0.34165 -0.235 0.475 -0.14 0.13335 -0.311835 0.2 -0.5155 0.2ZM18.55 6.3c-0.1 0 -0.1958 -0.01665 -0.2875 -0.05 -0.09165 -0.03335 -0.17915 -0.09165 -0.2625 -0.175 -0.47865 -0.48065 -0.99415 -0.89665 -1.5465 -1.248 -0.5523 -0.351335 -1.1368 -0.635335 -1.7535 -0.852 -0.15 -0.052165 -0.275 -0.143415 -0.375 -0.27375 -0.1 -0.130165 -0.15 -0.2865 -0.15 -0.469 0 -0.2215 0.07425 -0.403085 0.22275 -0.54475 0.1487 -0.141665 0.33275 -0.2125 0.55225 -0.2125 0.0667 0 0.15 0.016665 0.25 0.05 0.78335 0.266665 1.49585 0.6125 2.1375 1.0375 0.6417 0.425 1.22205 0.90525 1.74105 1.44075 0.081 0.08115 0.13815 0.16525 0.17145 0.25225 0.03335 0.08685 0.05 0.17665 0.05 0.2695 0 0.2195 -0.0718 0.4036 -0.2155 0.55225 -0.1438 0.1485 -0.322 0.22275 -0.5345 0.22275ZM7.0575 4.9c-0.22165 0 -0.4075 -0.07325 -0.5575 -0.21975 -0.15 -0.146665 -0.225 -0.328335 -0.225 -0.545 0 -0.140165 0.03375 -0.265665 0.10125 -0.3765 0.0677 -0.110835 0.1506 -0.197085 0.24875 -0.25875 0.6667 -0.416665 1.35 -0.745835 2.05 -0.9875C9.375 2.270835 10.1167 2.1 10.9 2h0.13025c0.211 0 0.38795 0.071835 0.53075 0.2155 0.1427 0.143835 0.214 0.322 0.214 0.5345 0 0.2 -0.06665 0.370835 -0.2 0.5125 -0.1333 0.141665 -0.29165 0.220835 -0.475 0.2375 -0.6833 0.083335 -1.3125 0.229165 -1.8875 0.4375S8.05 4.425 7.45 4.775c-0.06665 0.033335 -0.12915 0.0625 -0.1875 0.0875 -0.0583 0.025 -0.12665 0.0375 -0.205 0.0375Z"
			></path>
		</svg>

		<label>Place the image here</label>
	{:else if isLoading}
		<CubesLoader color="var(--primary)"/>
		<label>Loading...</label>
	{:else}
		<div class="preview">
			<img src={StorageBucketMain.fileSrc(currentPic)} alt="Current profile pic" />
		</div>

		<label class="upload-btn unselectable">
			<span>Choose custom</span>
			<input type="file" accept="image/*" onchange={handleInputChange} hidden />
		</label>
	{/if}
</div>

<style>
	.preview-container {
		display: grid;
		place-items: center center;
		gap: 1rem;
		border: 0.125rem solid transparent;
		border-radius: 1rem;
		grid-template-rows: 9rem 2.5rem;
	}

	.preview-container.dragging {
		border: 0.125rem dashed var(--secondary-foreground);
		opacity: 0.8;
	}

	.preview-container.dragging * {
		pointer-events: none;
	}

	.preview-container.dragging svg {
		height: 7rem;
		aspect-ratio: 1/1;
		stroke-width: 0.25;
	}

	.preview-container.dragging label {
		font-size: 1rem;
		font-weight: 500;
		white-space: nowrap;
	}

	.preview {
		display: grid;
		height: 100%;
		border-radius: 999rem;
		background: var(--back);
		box-shadow: var(--shadow-xs), var(--shadow-md);
		aspect-ratio: 1/1;
		overflow: hidden;
		place-items: center;
	}

	.preview img {
		inline-size: 100%;
		block-size: 100%;
		object-fit: cover;
		display: grid;
		place-content: center;
		aspect-ratio: 1/1;
	}

	.upload-btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		height: 100%;
		padding: 0 1.5rem;
		border-radius: var(--radius);
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 600;
		letter-spacing: 0.25px;
		cursor: pointer;
	}

	.upload-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
