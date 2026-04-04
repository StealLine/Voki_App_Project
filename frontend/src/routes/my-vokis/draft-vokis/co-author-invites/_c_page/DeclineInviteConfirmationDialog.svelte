<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';

	interface Props {
		deleteInviteOnSuccessDecline: (vokiId: string) => void;
	}

	let { deleteInviteOnSuccessDecline }: Props = $props();
	let inviteToDecline: InviteForVokiCoAuthorData | undefined = $state();
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = $state(false);
	let errs = $state<Err[]>([]);

	export function open(invite: InviteForVokiCoAuthorData) {
		inviteToDecline = invite;
		dialog.open();
	}

	async function confirmDecline() {
		errs = [];
		if (!inviteToDecline) {
			errs = [{ message: 'Invite to accept is not selected' }];
			return;
		}

		isLoading = true;
		const response = await ApiVokiCreationCore.fetchVoidResponse(
			`/vokis/${inviteToDecline.vokiId}/decline-co-author-invite`,
			RJO.PATCH({})
		);
		isLoading = false;

		if (response.isSuccess) {
			deleteInviteOnSuccessDecline(inviteToDecline.vokiId);
			closeDialog();
		} else {
			errs = response.errs;
		}
	}

	function closeDialog() {
		dialog.close();
		isLoading = false;
		inviteToDecline = undefined;
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="decline-invite-dialog">
	{#if inviteToDecline}
		<div class="decline-dialog">
			<h1 class="title">
				Are you sure you want to decline the invite to
				<span class="voki-name">{inviteToDecline.vokiName}</span> Voki?
			</h1>

			<p class="subtitle">
				You will be removed from the list of invited co-authors and will no longer have access to
				this draft unless invited again.
			</p>

			<div class="buttons">
				<button class="btn btn-cancel" onclick={() => dialog.close()}>Cancel</button>
				<button class="btn btn-decline" onclick={() => confirmDecline()}>Decline Invite</button>
			</div>
		</div>
	{:else}
		<h1>No invite selected</h1>
	{/if}
</DialogWithCloseButton>

<style>
	.decline-dialog {
		display: flex;
		flex-direction: column;
		gap: 1.5rem;
		max-width: 32rem;
	}

	.title {
		color: var(--text);
		font-size: 1.4rem;
		font-weight: 600;
		line-height: 1.35;
	}

	.voki-name {
		color: var(--primary);
		font-weight: 600;
	}

	.subtitle {
		color: var(--muted-foreground);
		font-size: 1rem;
		line-height: 1.45;
	}

	.buttons {
		display: flex;
		flex-direction: row;
		justify-content: right;
		gap: 1rem;
		margin-top: 0.5rem;
	}

	.btn {
		padding: 0.5rem 1.5rem;
		border: none;
		border-radius: var(--radius);
		font-size: 1rem;
		font-weight: 475;
		transition: all 0.12s ease;
		cursor: pointer;
	}

	.btn:hover {
		transform: scale(1.05);
	}

	.btn-decline {
		background: var(--red-1);
		color: var(--red-5);
	}

	.btn-cancel {
		background: var(--secondary);
		color: var(--secondary-foreground);
	}
</style>
