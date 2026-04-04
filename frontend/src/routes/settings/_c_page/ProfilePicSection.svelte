<script lang="ts">
	import UserProfilePicDisplay from '$lib/components/profile_pic/UserProfilePicDisplay.svelte';
	import type { UserProfilePicData } from '$lib/ts/users';
	import EditProfilePicDialog from './_c_profile_pic_sec/EditProfilePicDialog.svelte';

	interface Props {
		profilePicData: UserProfilePicData;
	}
	let { profilePicData }: Props = $props();
	let editProfilePicDialog = $state<EditProfilePicDialog>()!;
</script>

<div class="profile-pic-section">
	<EditProfilePicDialog bind:this={editProfilePicDialog} />
	<UserProfilePicDisplay
		key={profilePicData.key}
		shape={profilePicData.shape}
		sizeInRem={14}
		borderWidthInRem={0.375}
		borderColor="var(--back)"
	/>
	<button class="update-pic-btn" onclick={() => editProfilePicDialog.open(profilePicData)}
		>Update profile picture</button
	>
</div>

<style>
	.profile-pic-section {
		display: flex;
		flex-direction: column;
		align-items: center;
	}
	.update-pic-btn {
		width: 100%;
		border-radius: 0.5rem;
		border: none;
		background-color: var(--primary);
		color: var(--primary-foreground);
		padding: 0.5rem;
		cursor: pointer;
		font-weight: 450;
		font-size: 1.125rem;
	}
	.update-pic-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
