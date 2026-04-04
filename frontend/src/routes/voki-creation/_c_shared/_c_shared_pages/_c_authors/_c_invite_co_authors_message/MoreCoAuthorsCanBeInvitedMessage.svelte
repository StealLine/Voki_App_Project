<script lang="ts">
	import type { Snippet } from 'svelte';
	import CoAuthorsMessageTemplate from './_c_co_authors_msgs_shared/CoAuthorsMessageTemplate.svelte';

	interface Props {
		openInviteDialog: () => void;
		maxCoAuthors: number;
		coAuthorsWithInvitedCount: number;
		isViewerPrimaryAuthor: boolean;
	}

	let { openInviteDialog, maxCoAuthors, coAuthorsWithInvitedCount, isViewerPrimaryAuthor }: Props =
		$props();

	function templateActionItem():
		| { type: 'button'; text: string; onClick: () => void }
		| { type: 'custom'; actionItem: Snippet } {
		if (isViewerPrimaryAuthor) {
			return { type: 'button', text: 'Invite more co-author', onClick: openInviteDialog };
		} else {
			return { type: 'custom', actionItem: onlyPrimaryAuthorCanInvite };
		}
	}
</script>

<CoAuthorsMessageTemplate {subtitle} title={null} actionItem={templateActionItem()} />
{#snippet subtitle()}
	{maxCoAuthors - coAuthorsWithInvitedCount} more co-authors can be invited
{/snippet}
{#snippet onlyPrimaryAuthorCanInvite()}
	<div class="only-primary-author-can-invite">
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M4.27 18.84C4.49 20.52 5.88 21.82 7.56 21.9C8.98 21.97 10.42 22 12 22C13.58 22 15.02 21.97 16.44 21.9C18.12 21.82 19.51 20.52 19.73 18.84C19.88 17.75 20 16.64 20 15.5C20 14.36 19.88 13.25 19.73 12.15C19.51 10.49 18.12 9.18 16.44 9.1C15.02 9.03 13.58 9 12 9C10.42 9 8.98 9.03 7.56 9.1C5.88 9.18 4.49 10.49 4.27 12.15C4.12 13.25 4 14.36 4 15.5C4 16.64 4.12 17.75 4.27 18.84Z"
				stroke="currentColor"
			/>
			<path
				d="M7.5 9V6.5C7.5 4.01 9.51 2 12 2C14.49 2 16.5 4.01 16.5 6.5V9"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
			<path
				d="M12 15.5H12.01"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
		</svg>
		Only the primary author can invite co-authors
	</div>
{/snippet}

<style>
	.only-primary-author-can-invite {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 0.75rem;
		margin-top: 0.5rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 425;
		box-shadow: var(--shadow-xs);
	}

	.only-primary-author-can-invite svg {
		width: 1.5rem;
		height: 1.5rem;
		color: inherit;
		stroke-width: 1.875;
	}
</style>
