<script lang="ts">
	import { toast } from 'svelte-sonner';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { getVokiCreationPageContext } from '../../../voki-creation-page-context';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import CoverChangingDialog from './_c_cover_section/CoverChangingDialog.svelte';
	interface Props {
		savedCover: string;
		vokiId: string;
		updateSavedVokiCover: (newCover: string) => void;
	}
	let { savedCover, vokiId, updateSavedVokiCover }: Props = $props();
	const vokiCreationCtx = getVokiCreationPageContext();
	let version = $state(0);
	let isLoading = $state(false);
	let changeCoverDialog = $state<CoverChangingDialog>()!;
	async function changeImageToDefault() {
		isLoading = true;
		const response = await vokiCreationCtx.vokiCreationApi.setVokiCoverToDefault(vokiId);
		if (response.isSuccess) {
			updateSavedVokiCover(response.data.newCover);
		} else {
			toast.error("Couldn't set voki cover to default");
		}
		isLoading = false;
	}
	async function updateVokiCoverWithNewSelected(newCover: string) {
		isLoading = true;
		const response = await vokiCreationCtx.vokiCreationApi.updateVokiCover(vokiId, newCover);
		if (response.isSuccess) {
			updateSavedVokiCover(response.data.newCover);
			version++;
		} else {
			toast.error("Couldn't update voki cover");
		}
		isLoading = false;
	}
</script>

<div class="img-container">
	<CoverChangingDialog
		bind:this={changeCoverDialog}
		updateCoverToNew={updateVokiCoverWithNewSelected}
	/>
	{#if isLoading}
		<div class="loading-container">
			<CubesLoader sizeRem={5} color="var(--primary)" />
			<label>Updating Voki cover</label>
		</div>
	{:else}
		<img src={StorageBucketMain.fileSrcWithVersion(savedCover, version)} alt="voki cover" />
		<button class="img-btn change-btn" onclick={() => changeCoverDialog.open()}>Change cover</button
		>

		<button class="img-btn set-default-btn" onclick={changeImageToDefault}>Set to default </button>
	{/if}
</div>

<style>
	.img-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 28rem;
		border-radius: 1rem;
	}

	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		border-radius: 1rem;
		background-color: var(--secondary);
		animation: var(--default-fade-in-animation);
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.loading-container > label {
		color: var(--secondary-foreground);
		font-size: 1.5rem;
		font-weight: 570;
		letter-spacing: 0.5px;
		white-space: nowrap;
	}

	.img-container img {
		width: 100%;
		border-radius: 1rem;
		object-fit: fill;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		animation: var(--default-fade-in-animation);
	}

	.img-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.2px;
		box-shadow: var(--shadow);
		cursor: pointer;
	}

	.change-btn {
		margin-top: 1rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.set-default-btn {
		margin-top: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.set-default-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
