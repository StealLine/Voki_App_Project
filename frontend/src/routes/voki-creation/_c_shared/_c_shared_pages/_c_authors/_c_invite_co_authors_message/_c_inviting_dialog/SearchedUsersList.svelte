<script lang="ts">
	import type { UserPreviewWithInvitesSettings, UserInviteState } from '../../types';
	import SearchedUserInListItem from './SearchedUserInListItem.svelte';

	interface Props {
		getUserInviteState: (user: UserPreviewWithInvitesSettings) => UserInviteState;
		users: UserPreviewWithInvitesSettings[];
	}
	let { users, getUserInviteState }: Props = $props();
</script>

<div class="users-list-wrapper">
	<div class="fade-top"></div>
	<div class="users-list">
		{#each users as user}
			<SearchedUserInListItem
				uniqueName={user.uniqueName}
				displayName={user.displayName}
				profilePic={user.profilePic}
				badge={getUserInviteState(user)}
			/>
		{/each}
	</div>
	<div class="fade-bottom"></div>
</div>

<style>
	.users-list-wrapper {
		position: relative;
		display: grid;
		flex-direction: column;
		height: 100%;
		min-height: 0;
		flex: 1;
	}

	.users-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		height: 100%;
		padding: 0.375rem 0;
		overflow-y: auto;
		scrollbar-gutter: stable;
	}

	.fade-top,
	.fade-bottom {
		position: absolute;
		right: 0;
		left: 0;
		z-index: 2;
		height: 1rem;
		pointer-events: none;
	}

	.fade-top {
		top: -1px;
		background: linear-gradient(var(--back), transparent);
	}

	.fade-bottom {
		bottom: -1px;
		background: linear-gradient(transparent, var(--back));
	}
</style>
