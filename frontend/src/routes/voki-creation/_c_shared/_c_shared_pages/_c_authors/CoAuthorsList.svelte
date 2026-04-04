<script lang="ts">
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { toast } from 'svelte-sonner';
	import VokiCoAuthorsListUserItem from './_c_co_authors_list/VokiCoAuthorsListUserItem.svelte';
	import DropCoAuthorConfirmationDialog from './_c_co_authors_list/DropCoAuthorConfirmationDialog.svelte';
	import type { CoAuthorsPageState } from './co-authors-page-state.svelte';
	import LeaveVokiCreationConfirmationDialog from './_c_co_authors_list/LeaveVokiCreationConfirmationDialog.svelte';

	interface Props {
		viewerId: string;
		isViewerPrimaryAuthor: boolean;
		pageState: CoAuthorsPageState;
	}
	let { viewerId, isViewerPrimaryAuthor, pageState }: Props = $props();

	let dropCoAuthorDialog = $state<DropCoAuthorConfirmationDialog>()!;
	let leaveVokiCreationDialog = $state<LeaveVokiCreationConfirmationDialog>()!;

	async function cancelInvite(userId: string) {
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			coAuthorIds: string[];
			invitedForCoAuthorUserIds: string[];
		}>(`/vokis/${pageState.vokiId}/cancel-co-author-invite`, RJO.DELETE({ userId }));
		if (response.isSuccess) {
			pageState.updateCoAuthorsInfo(
				response.data.coAuthorIds,
				response.data.invitedForCoAuthorUserIds
			);
			toast.success('Invite cancelled');
		} else {
			toast.error("Couldn't cancel invite");
		}
	}
	function decideCoAuthorActionButton(userId: string) {
		if (viewerId === userId && !isViewerPrimaryAuthor) {
			return leaveVokiCreationButton;
		}
		if (isViewerPrimaryAuthor) {
			return dropCoAuthorButton;
		}
		return null;
	}
</script>

{#snippet cancelInviteButton(userId: string)}
	<button class="action-btn" onclick={() => cancelInvite(userId)}>Cancel invite</button>
{/snippet}
{#snippet dropCoAuthorButton(userId: string)}
	<button class="action-btn" onclick={() => dropCoAuthorDialog.open(userId)}>Drop co-author</button>
{/snippet}
{#snippet leaveVokiCreationButton()}
	<button class="action-btn" onclick={() => leaveVokiCreationDialog.open()}
		>Leave Voki creation</button
	>
{/snippet}

<DropCoAuthorConfirmationDialog
	bind:this={dropCoAuthorDialog}
	vokiId={pageState.vokiId}
	onSuccessfulDrop={(newCoAuthors, newInvites) =>
		pageState.updateCoAuthorsInfo(newCoAuthors, newInvites)}
/>
<LeaveVokiCreationConfirmationDialog
	bind:this={leaveVokiCreationDialog}
	vokiId={pageState.vokiId}
/>
<div class="all-co-authors-list">
	{#each [...pageState.coAuthorIds].sort((a, b) => a.localeCompare(b)) as userId}
		{#key userId}
			<VokiCoAuthorsListUserItem
				{userId}
				userCoAuthorState={userId === viewerId ? 'viewerIsAuthor' : 'coAuthor'}
				actionButton={decideCoAuthorActionButton(userId)}
			/>
		{/key}
	{/each}
	{#if pageState.coAuthorIds.length > 0 && pageState.invitedForCoAuthorUserIds.length > 0}
		<h2 class="invited-subheading">
			Invited co-authors({pageState.invitedForCoAuthorUserIds.length})
		</h2>
	{/if}

	{#each [...pageState.invitedForCoAuthorUserIds].sort((a, b) => a.localeCompare(b)) as userId}
		{#key userId}
			<VokiCoAuthorsListUserItem
				{userId}
				userCoAuthorState="invitedUser"
				actionButton={isViewerPrimaryAuthor ? cancelInviteButton : null}
			/>
		{/key}
	{/each}
</div>

<style>
	.all-co-authors-list {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
		width: 100%;
		margin-top: 1rem;
	}

	.action-btn {
		width: 10rem;
		padding: 0.375rem 0;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		cursor: pointer;
	}

	.action-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
