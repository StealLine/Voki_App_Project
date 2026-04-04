<script lang="ts">
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { UserInviteState } from '../../types';

	interface Props {
		uniqueName: string;
		displayName: string;
		profilePic: string;
		badge: UserInviteState;
	}

	let { uniqueName, displayName, profilePic, badge }: Props = $props();

	let isInInvitedList = $derived(badge.state === 'CandidateToInvite' && badge.isUserInListToInvite);
	function onUserItemClick() {
		if (badge.state === 'CandidateToInvite') {
			if (isInInvitedList) {
				badge.removeFromListToInvite();
			} else {
				badge.addToListToInvite();
			}
		}
	}
</script>

<div class="user" onclick={() => onUserItemClick()}>
	<img class="profile-pic" src={StorageBucketMain.fileSrc(profilePic)} alt="user profile pic" />
	<div class="names-container">
		<span class="display-name">{displayName}</span>
		<span class="unique-name-content">@{uniqueName}</span>
	</div>
	{#if badge.state === 'CoAuthor'}
		<span class="badge co-author">Co-author</span>
	{:else if badge.state === 'PrimaryAuthor'}
		<span class="badge primary-author">Primary author</span>
	{:else if badge.state === 'AlreadyInvited'}
		<span class="badge already-invited">Already invited</span>
	{:else}
		<DefaultCheckBox bind:checked={isInInvitedList} parentOnlyControl={true} />
	{/if}
</div>

<style>
	.user {
		display: grid;
		place-items: center center;
		gap: 0.375rem;
		grid-template-columns: auto 1fr 9rem;
	}

	.user * {
		cursor: default;
	}

	.profile-pic {
		display: grid;
		width: 3rem;
		height: 3rem;
		border-radius: 100vw;
		background: var(--muted);
		aspect-ratio: 1/1;
		object-fit: cover;
		place-items: center;
	}

	.names-container {
		display: grid;
		grid-template-rows: auto auto;
		justify-content: start;
		width: 100%;
	}

	.display-name {
		display: block;
		width: 100%;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 550;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.unique-name-content {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 480;
	}

	.badge {
		padding: 0.125rem 0.5rem;
		border-radius: 100vw;
		font-size: 0.875rem;
		font-weight: 450;
		letter-spacing: 0;
	}

	.co-author,
	.primary-author {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.already-invited {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}
</style>
