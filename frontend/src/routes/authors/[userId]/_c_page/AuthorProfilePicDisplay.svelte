<script lang="ts">
	import UserProfilePicDisplay from '$lib/components/profile_pic/UserProfilePicDisplay.svelte';
	import type { UserProfilePicData } from '$lib/ts/users';

	interface Props {
		profilePicData: UserProfilePicData;
	}
	let { profilePicData }: Props = $props();
</script>

<UserProfilePicDisplay
	key={profilePicData.key}
	shape={profilePicData.shape}
	class="author-profile-pic-display"
	sizeInRem={12}
	borderWidthInRem={0.375}
	borderColor="var(--back)"
/>

<style>
	:global(.author-profile-pic-display) {
		margin-top: -6rem;
		flex-shrink: 0;
		position: relative;
	}
</style>
