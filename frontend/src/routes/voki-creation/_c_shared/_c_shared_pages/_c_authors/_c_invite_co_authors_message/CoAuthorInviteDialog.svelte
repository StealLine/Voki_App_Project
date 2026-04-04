<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import UserSearchBar from './_c_inviting_dialog/UserSearchBar.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import ConfirmInviteBtnContainer from './_c_inviting_dialog/ConfirmInviteBtnContainer.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { CoAuthorsInviteDialogState } from '../co-authors-page-state.svelte';
	import type { UserPreviewWithInvitesSettings, UserInviteState } from '../types';
	import NoUsersSearched from './_c_inviting_dialog/NoUsersSearched.svelte';
	import SearchedUsersList from './_c_inviting_dialog/SearchedUsersList.svelte';
	import SearchInputEmptyState from './_c_inviting_dialog/SearchInputEmptyState.svelte';

	interface Props {
		maxCoAuthorsCount: number;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		dialogState: CoAuthorsInviteDialogState;
		updateCoAuthors: (newCoAuthorIds: string[], newInvitedForCoAuthorUserIds: string[]) => void;
		getUserInviteState: (user: UserPreviewWithInvitesSettings) => UserInviteState;
	}

	let {
		maxCoAuthorsCount,
		coAuthorIds,
		invitedForCoAuthorUserIds,
		dialogState,
		updateCoAuthors,
		getUserInviteState
	}: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;

	export function open() {
		dialogState.clearOnDialogOpen();
		dialog.open();
	}

	function onInviteButtonClick() {
		dialogState.confirmUsersInvite(
			(newCoAuthorIds: string[], newInvitedForCoAuthorUserIds: string[]) => {
				updateCoAuthors(newCoAuthorIds, newInvitedForCoAuthorUserIds);
				dialog.close();
			}
		);
	}
</script>

<DialogWithCloseButton dialogId="co-author-inviting-dialog" bind:this={dialog}>
	<UserSearchBar
		bind:searchBarInputVal={dialogState.searchBarInputVal}
		bind:searchedUsers={dialogState.searchedUsers}
	/>
	<p1 class="co-authors-count"
		>Co-authors (including invited) count: {coAuthorIds.length +
			invitedForCoAuthorUserIds.length}/{maxCoAuthorsCount}</p1
	>

	{#if StringUtils.isNullOrWhiteSpace(dialogState.searchBarInputVal)}
		<SearchInputEmptyState
			usersRecommendedToInvite={dialogState.usersRecommendedToInvite}
			{getUserInviteState}
		/>
	{:else if dialogState.searchedUsers.length === 0}
		<NoUsersSearched />
	{:else}
		<SearchedUsersList users={dialogState.searchedUsers} {getUserInviteState} />
	{/if}

	<ConfirmInviteBtnContainer
		usersChosenToInvite={dialogState.usersChosenToInvite}
		isLoading={dialogState.isLoadingSave}
		{onInviteButtonClick}
	/>
	<div class="errs-container">
		<DefaultErrBlock errList={dialogState.savingErrs} />
	</div>
	<label class="note"
		>Note: invited user will see the type, name, cover and other main details of this Voki</label
	>
</DialogWithCloseButton>

<style>
	:global(#co-author-inviting-dialog > .dialog-content) {
		display: grid;
		width: 44rem;
		padding: 2rem;
		grid-template-rows: auto auto 32rem auto auto auto;
	}

	.co-authors-count {
		margin: 0.25rem 0 0;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
	}

	.errs-container {
		min-height: 1.5rem;
		margin: 0.25rem 0;
	}

	.note {
		margin-top: auto;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		text-align: center;
	}
</style>
