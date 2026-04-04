<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiType } from '$lib/ts/voki-type';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';
	import AcceptInviteDialogConfirmationState from './_c_accept_invite_dialog/AcceptInviteDialogConfirmationState.svelte';
	import AcceptInviteDialogConfirmedState from './_c_accept_invite_dialog/AcceptInviteDialogConfirmedState.svelte';

	interface Props {
		deleteInviteOnSuccessAccept: (vokiId: string) => void;
	}
	let { deleteInviteOnSuccessAccept }: Props = $props();
	type DialogState =
		| { name: 'NoInviteSelected' }
		| { name: 'ConfirmMessage'; invite: InviteForVokiCoAuthorData }
		| {
				name: 'Confirmed';
				vokiName: string;
				vokiCover: string;
				vokiId: string;
				vokiType: VokiType;
		  };

	let dialogState: DialogState = $state<DialogState>({ name: 'NoInviteSelected' });
	let dialog = $state<DialogWithCloseButton>()!;
	let confirmationState = $state<AcceptInviteDialogConfirmationState>();

	export function open(invite: InviteForVokiCoAuthorData) {
		dialogState = { name: 'ConfirmMessage', invite };
		if (confirmationState) {
			confirmationState.reset();
		}
		dialog.open();
	}

	function closeDialog() {
		dialog.close();
		dialogState = { name: 'NoInviteSelected' };
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="accept-invite-dialog">
	{#if dialogState.name === 'ConfirmMessage'}
		<AcceptInviteDialogConfirmationState
			bind:this={confirmationState}
			invite={dialogState.invite}
			changeStateToConfirmed={(inv) => {
				dialogState = {
					name: 'Confirmed',
					vokiName: inv.vokiName,
					vokiCover: inv.vokiCover,
					vokiId: inv.vokiId,
					vokiType: inv.vokiType
				};
			}}
			{closeDialog}
			{deleteInviteOnSuccessAccept}
		/>
	{:else if dialogState.name === 'Confirmed'}
		<AcceptInviteDialogConfirmedState
			vokiType={dialogState.vokiType}
			vokiId={dialogState.vokiId}
			vokiName={dialogState.vokiName}
			vokiCover={dialogState.vokiCover}
			{closeDialog}
		/>
	{:else if dialogState.name === 'NoInviteSelected'}
		<div class="empty">
			<p class="invite-not-selected">Invite to accept is not selected</p>
			<div class="buttons">
				<button class="btn secondary" onclick={() => closeDialog()}>Close</button>
			</div>
		</div>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#accept-invite-dialog > .dialog-content) {
		position: relative;
		width: 48rem;
		min-height: 32rem;
		padding-top: 2.5rem;
		padding-bottom: 2.5rem;
	}

	.empty {
		display: grid;
		gap: 0.875rem;
	}

	.invite-not-selected {
		color: var(--muted-foreground);
	}
</style>
