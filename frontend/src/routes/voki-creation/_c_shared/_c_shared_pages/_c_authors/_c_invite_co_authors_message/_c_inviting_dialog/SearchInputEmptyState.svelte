<script lang="ts">
	import type {
		UserInviteState,
		UserPreviewWithInvitesSettings,
		UsersRecommendedToInvite
	} from '../../types';
	import SearchedUsersList from './SearchedUsersList.svelte';

	interface Props {
		usersRecommendedToInvite: UsersRecommendedToInvite;
		getUserInviteState: (user: UserPreviewWithInvitesSettings) => UserInviteState;
	}
	let { usersRecommendedToInvite, getUserInviteState }: Props = $props();
</script>

<div class="input-empty-state">
	{#if usersRecommendedToInvite.state === 'errs'}
		<div class="err-container">Could not load users to recommend</div>
	{:else if usersRecommendedToInvite.state === 'loading'}
		<div class="loading-container">Loading users to recommend</div>
	{:else if usersRecommendedToInvite.state === 'ok'}
		<SearchedUsersList users={usersRecommendedToInvite.users} {getUserInviteState} />
	{/if}
	<div class="notes">
		<p class="title">If you want to invite specific user use input above to search for them</p>
		<p class="subtitle">
			Search by <span class="search-val">display name</span> or
			<span class="search-val">@uniqueName</span>
		</p>
	</div>
</div>

<style>
	.input-empty-state {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		height: 100%;
	}

	.notes {
		padding: 0.25rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
	}

	.title {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		line-height: 1.25rem;
		text-align: center;
	}

	.subtitle {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 450;
		line-height: 1rem;
		text-align: center;
	}

	.search-val {
		font-weight: 550;
		text-decoration: underline;
	}
</style>
