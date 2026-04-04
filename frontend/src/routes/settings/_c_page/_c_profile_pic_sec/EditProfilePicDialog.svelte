<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import UserProfilePicDisplay from '$lib/components/profile_pic/UserProfilePicDisplay.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import type { UserProfilePicData } from '$lib/ts/users';
	import EditProfilePicDialogSubheader from './_c_pic_dialog/EditProfilePicDialogSubheader.svelte';
	import ProfilePicShapeSelector from './_c_pic_dialog/ProfilePicShapeSelector.svelte';

	let currentVal = $state<UserProfilePicData>();
	let dialog = $state<DialogWithCloseButton>()!;
	export function open(profilePicData: UserProfilePicData) {
		currentVal = profilePicData;
		dialog.open();
	}

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
			if (currentVal) {
				currentVal.key = response.data;
			}
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

<DialogWithCloseButton
	dialogId="edit-profile-pic-dialog"
	bind:this={dialog}
	subheading="Update your profile picture"
>
	{#if currentVal}
		<div
			class="img-upload-container"
			class:loading={isLoading}
			class:dragging={isDragging}
			ondrop={(e) => ondrop(e)}
			{ondragover}
			{ondragleave}
		>
			<EditProfilePicDialogSubheader
				text="Drag and drop new profile picture or click the button below"
			/>
			<div class="input-wrapper">
				<UserProfilePicDisplay
					key={currentVal.key}
					shape={currentVal.shape}
					sizeInRem={16}
					borderWidthInRem={0.375}
					borderColor="var(--back)"
				/>
				<input type="file" accept="image/*" onchange={handleInputChange} />
			</div>
			<EditProfilePicDialogSubheader text="Choose profile picture shape" />
			<ProfilePicShapeSelector
				currentShape={currentVal.shape}
				onSelect={(newShape) => {
					if (currentVal) {
						currentVal.shape = newShape;
					}
				}}
			/>
			{#if isLoading}
				<div class="loading-overlay">
					<div class="blur"></div>
					<CubesLoader sizeRem={5} color="var(--primary)" />
				</div>
			{/if}
		</div>
	{:else}
		<ErrView err={{ message: 'Something went wrong. Please close this dialog and try again' }} />
	{/if}
</DialogWithCloseButton>

<style>
	.img-upload-container {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		flex-direction: column;
		border-radius: 2rem;
		text-align: center;
		border: 0.125rem dashed var(--back);
		padding: 0.25rem 2rem;
		position: relative;
	}
	.img-upload-container.dragging:not(.loading) {
		border-color: var(--primary);
		background-color: var(--secondary);
	}
	.img-upload-container.loading {
		border-color: var(--secondary-foreground);
	}
	.img-upload-container.loading .loading-overlay {
		position: absolute;
		width: 100%;
		height: 100%;
		background-color: transparent;
		display: flex;
		justify-content: center;
		align-items: center;
	}
	.img-upload-container.loading > *:not(.loading-overlay) {
		opacity: 0.95;
		filter: blur(0.0625rem);
	}
	.img-upload-container.loading .loading-overlay .blur {
		position: absolute;
		width: 100%;
		height: 100%;
		background-color: var(--back);
		opacity: 0.25;
		filter: blur(0.25rem);
		transform: scale(1.1);
	}
	.input-wrapper {
		position: relative;
		width: fit-content;
		height: fit-content;
		margin-bottom: 1rem;
	}
	.input-wrapper > input {
		position: absolute;
		width: 100%;
		height: 100%;
		cursor: pointer;
		top: 0;
		left: 0;
		opacity: 0;
	}
</style>
