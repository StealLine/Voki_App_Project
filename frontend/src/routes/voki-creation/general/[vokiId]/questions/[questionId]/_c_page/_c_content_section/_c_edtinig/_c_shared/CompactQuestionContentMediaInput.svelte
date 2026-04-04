<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import type { Snippet } from 'svelte';

	interface Props {
		type: 'image' | 'audio';
		mediaUrl: string;
		onUploadSuccess: (newUrl: string) => void;
		mediaDisplay: Snippet;
	}
	let { type, mediaUrl, onUploadSuccess, mediaDisplay }: Props = $props();

	let isLoading = $state(false);
	let uploadingErrs = $state<Err[]>([]);

	let acceptString = $derived(type === 'image' ? 'image/*' : 'audio/*');
	let mimePrefix = $derived(type === 'image' ? 'image/' : 'audio/');

	async function uploadFile(file: File) {
		if (!file.type.startsWith(mimePrefix)) {
			uploadingErrs = [{ message: `Selected file is not an ${type}` }];
			return;
		}
		isLoading = true;
		uploadingErrs = [];

		let response;
		if (type === 'image') {
			response = await StorageBucketMain.uploadTempImage(file);
		} else {
			response = await StorageBucketMain.uploadTempAudio(file);
		}

		if (response.isSuccess) {
			onUploadSuccess(response.data);
		} else {
			uploadingErrs = response.errs;
		}
		isLoading = false;
	}

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			await uploadFile(files[0]);
		}
	}
</script>

<div class="media-input compact">
	{#if isLoading}
		<div class="loading compact">
			<CubesLoader sizeRem={3} color="var(--primary)" />
		</div>
	{:else if mediaUrl}
		<div class="media-selected compact">
			{@render mediaDisplay()}
			<label class="compact-btn unselectable">
				<span>Change {type}</span>
				<input type="file" accept={acceptString} onchange={handleInputChange} hidden />
			</label>
		</div>
	{:else}
		<label class="compact-btn unselectable">
			{#if type === 'image'}
				<svg><use href="#add-image-icon" /></svg>
			{:else}
				{@render micIcon()}
			{/if}
			<span>Add {type}</span>
			<input type="file" accept={acceptString} onchange={handleInputChange} hidden />
		</label>
	{/if}
	{#if uploadingErrs.length > 0}
		<DefaultErrBlock errList={uploadingErrs} />
	{/if}
</div>

{#snippet micIcon()}
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		stroke="currentColor"
		stroke-linecap="round"
		stroke-linejoin="round"
	>
		<path
			d="M20 11C20 11 20 9.4306 19.8478 9.06306C19.6955 8.69552 19.4065 8.40649 18.8284 7.82843L14.0919 3.09188C13.593 2.593 13.3436 2.34355 13.0345 2.19575C12.9702 2.165 12.9044 2.13772 12.8372 2.11401C12.5141 2 12.1614 2 11.4558 2C8.21082 2 6.58831 2 5.48933 2.88607C5.26731 3.06508 5.06508 3.26731 4.88607 3.48933C4 4.58831 4 6.21082 4 9.45584V14C4 17.7712 4 19.6569 5.17157 20.8284C6.34315 22 8.22876 22 12 22M13 2.5V3C13 5.82843 13 7.24264 13.8787 8.12132C14.7574 9 16.1716 9 19 9H19.5"
		/>
		<path
			d="M19.9998 19.4068V16.5932C19.9998 15.0206 19.9998 14.2343 19.46 14.0386C18.9201 13.843 18.2848 14.399 17.0141 15.511L16.5 16H15.0039C14.0611 16 13.5897 16 13.2968 16.2929C13.0039 16.5858 13.0039 17.0572 13.0039 18C13.0039 18.9428 13.0039 19.4142 13.2968 19.7071C13.5897 20 14.0611 20 15.0039 20H16.5L17.0141 20.489C18.2848 21.601 18.9201 22.157 19.46 21.9614C19.9998 21.7657 19.9998 20.9794 19.9998 19.4068Z"
		/>
	</svg>
{/snippet}

<style>
	.media-input.compact {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		width: 100%;
		height: 100%;
		border-radius: 1rem;
		gap: 0.5rem;
		min-width: 12rem;
		transition:
			height 0.12s ease,
			width 0.12s ease;
	}

	.media-input > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}

	.loading.compact {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
		min-height: auto;
		background-color: transparent;
		animation: var(--default-fade-in-animation);
		border-radius: 1rem;
	}

	.media-selected.compact {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
		padding: 0;
	}

	/* Compact variant button */
	.compact-btn {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		padding: 0.375rem 0;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 420;
		text-align: center;
		cursor: pointer;
	}

	.compact-btn > svg {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}

	.compact-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
