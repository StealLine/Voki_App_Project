<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import CoverImageInput from './CoverImageInput.svelte';
	interface Props {
		updateCoverToNew: (newKey: string) => Promise<void>;
	}
	let { updateCoverToNew }: Props = $props();
	type DialogStateType =
		| { isInput: false; newKey: string; isLoading: boolean }
		| { isInput: true; errs: Err[] };
	let dialogState = $state<DialogStateType>({
		isInput: true,
		errs: []
	});
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = false;
	export function open() {
		dialogState = { isInput: true, errs: [] };
		isLoading = false;
		dialog.open();
	}
	function onDialogSaveButtonClick() {
		if (dialogState.isInput) {
			toast.error('An error has occurred. Please refresh the page');
		} else {
			dialogState.isLoading = true;
			updateCoverToNew(dialogState.newKey).then(() => {
				if (!dialogState.isInput) {
					dialogState.isLoading = false;
				}
			});
		}
		dialog.close();
	}
	function onImageUploaded(tempKey: string) {
		dialogState = { isInput: false, newKey: tempKey, isLoading: false };
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="voki-creation-cover-changing-dialog">
	{#if dialogState.isInput}
		<CoverImageInput {onImageUploaded} />
	{:else}
		<img src={StorageBucketMain.fileSrcWithVersion(dialogState.newKey)} alt="voki cover" />
		<VokiCreationSaveAndCancelButtons
			onCancel={() => dialog.close()}
			onSave={() => onDialogSaveButtonClick()}
			isSaveLoading={dialogState.isLoading}
		/>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#voki-creation-cover-changing-dialog > .dialog-content) {
		width: 38rem;
		height: 26rem;
	}

	img {
		width: 100%;
		border-radius: 1rem;
		object-fit: fill;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		animation: var(--default-fade-in-animation);
	}
</style>
