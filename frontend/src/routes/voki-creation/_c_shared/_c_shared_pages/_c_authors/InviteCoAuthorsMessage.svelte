<script lang="ts">
	import CoAuthorInviteDialog from './_c_invite_co_authors_message/CoAuthorInviteDialog.svelte';
	import InviteFirstCoAuthorMessage from './_c_invite_co_authors_message/InviteFirstCoAuthorMessage.svelte';
	import MoreCoAuthorsCanBeInvitedMessage from './_c_invite_co_authors_message/MoreCoAuthorsCanBeInvitedMessage.svelte';
	import type { CoAuthorsPageState } from './co-authors-page-state.svelte';

	interface Props {
		pageState: CoAuthorsPageState;
		isViewerPrimaryAuthor: boolean;
	}
	let { pageState, isViewerPrimaryAuthor }: Props = $props();
	let coAuthorsWithInvitedCount = $derived(
		pageState.coAuthorIds.length + pageState.invitedForCoAuthorUserIds.length
	);
	let dialog = $state<CoAuthorInviteDialog>()!;

	function updatePageStateCoAuthors(
		newCoAuthorIds: string[],
		newInvitedForCoAuthorUserIds: string[]
	) {
		pageState.coAuthorIds = newCoAuthorIds;
		pageState.invitedForCoAuthorUserIds = newInvitedForCoAuthorUserIds;
	}
</script>

<CoAuthorInviteDialog
	bind:this={dialog}
	maxCoAuthorsCount={pageState.maxCoAuthorsCount}
	coAuthorIds={pageState.coAuthorIds}
	invitedForCoAuthorUserIds={pageState.invitedForCoAuthorUserIds}
	updateCoAuthors={(newAuthors, newInvites) => updatePageStateCoAuthors(newAuthors, newInvites)}
	dialogState={pageState.coAuthorsDialogState}
	getUserInviteState={(u) => pageState.getUserInviteState(u)}
/>

{#if coAuthorsWithInvitedCount === 0 && isViewerPrimaryAuthor}
	<InviteFirstCoAuthorMessage openInviteDialog={() => dialog.open()} />
{:else if coAuthorsWithInvitedCount >= pageState.maxCoAuthorsCount}
	<div class="co-authors-limit">
		<h2>Co-authors limit reached</h2>
		<p>Voki cannot have more than {pageState.maxCoAuthorsCount} co-authors</p>
	</div>
{:else}
	<MoreCoAuthorsCanBeInvitedMessage
		openInviteDialog={() => dialog.open()}
		maxCoAuthors={pageState.maxCoAuthorsCount}
		{coAuthorsWithInvitedCount}
		{isViewerPrimaryAuthor}
	/>
{/if}
