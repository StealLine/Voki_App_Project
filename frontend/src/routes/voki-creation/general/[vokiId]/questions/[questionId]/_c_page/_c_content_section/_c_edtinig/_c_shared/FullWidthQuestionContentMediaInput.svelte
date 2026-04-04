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
	let isDragging = $state(false);
	let uploadingErrs = $state<Err[]>([]);

	let mimePrefix: 'image/' | 'audio/' = $derived(type === 'image' ? 'image/' : 'audio/');
	let acceptString: 'image/*' | 'audio/*' = $derived(type === 'image' ? 'image/*' : 'audio/*');

	async function uploadFile(file: File) {
		if (!file.type.startsWith(mimePrefix)) {
			uploadingErrs = [{ message: `Selected file is not an ${type}` }];
			return;
		}
		uploadingErrs = [];
		isLoading = true;
		const response = await StorageBucketMain.uploadTempFile(type, file);
		isLoading = false;
		if (response.isSuccess) {
			onUploadSuccess(response.data);
		} else {
			uploadingErrs = response.errs;
		}
	}

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			await uploadFile(files[0]);
		}
	}

	async function ondrop(event: DragEvent) {
		event.preventDefault();
		isDragging = false;
		if (event.dataTransfer?.files?.length) {
			await uploadFile(event.dataTransfer.files[0]);
		}
	}

	function ondragover(event: DragEvent) {
		event.preventDefault();
		isDragging = Array.from(event.dataTransfer?.items || []).some((item) =>
			item.type.startsWith(mimePrefix)
		);
	}

	function ondragleave() {
		isDragging = false;
	}
</script>

<div class="media-input">
	{#if isLoading}
		<div class="loading">
			<CubesLoader sizeRem={4} color="var(--primary)" />
		</div>
	{:else if mediaUrl}
		<div class="media-selected">
			<label class="change-media-btn unselectable">
				<span>Change {type}</span>
				<input type="file" accept={acceptString} onchange={handleInputChange} hidden />
			</label>
			{@render mediaDisplay()}
		</div>
	{:else}
		<div
			class="file-input-container"
			class:dragging={isDragging}
			ondrop={(e) => ondrop(e)}
			{ondragover}
			{ondragleave}
		>
			<p>Drag and drop {type} or click the button below</p>
			<label class="upload-button unselectable" class:dragging={isDragging}>
				{#if type === 'image'}
					<svg><use href="#add-image-icon" /></svg>
				{:else}
					{@render micIcon()}
				{/if}
				<span>Add {type}</span>
				<input type="file" accept={acceptString} onchange={handleInputChange} hidden />
			</label>
		</div>
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
	.media-input {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
		height: 100%;
		border-radius: 1rem;
	}

	.media-input > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}

	.loading,
	.file-input-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
		min-height: 8rem;
		animation: var(--default-fade-in-animation);
		border-radius: 1rem;
	}

	.file-input-container {
		gap: 0.75rem;
		border: 0.125rem dashed var(--muted);
		text-align: center;
		transition: all 0.12s ease-in;
	}

	.loading {
		background-color: var(--secondary);
	}

	.file-input-container.dragging {
		border-color: var(--primary);
		background-color: var(--secondary);
	}

	.upload-button {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.375rem 1.25rem;
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
		stroke-width: 1.75;
	}

	.upload-button:hover {
		background-color: var(--primary-hov);
	}

	.upload-button.dragging {
		background-color: var(--primary-hov);
		transform: scale(1.06);
	}

	p {
		margin: 0 1rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
	}

	.media-selected {
		display: grid;
		align-items: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
		grid-template-columns: auto 1fr;
		padding: 0 2rem;
	}

	.change-media-btn {
		height: fit-content;
		padding: 0.375rem 0.75rem;
		border-radius: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-align: center;
		cursor: pointer;
	}

	.change-media-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
