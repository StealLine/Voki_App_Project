<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { MyVokiInvitesPageState } from './my-voki-invites-page-state.svelte';
	import InviteForCoAuthorDisplay from './_c_page/InviteForCoAuthorDisplay.svelte';
	import AcceptInviteConfirmationDialog from './_c_page/AcceptInviteConfirmationDialog.svelte';
	import DeclineInviteConfirmationDialog from './_c_page/DeclineInviteConfirmationDialog.svelte';
	import MyVokisPageInitialLoading from '../../_c_shared/MyVokisPageInitialLoading.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../../_c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';

	const pageState = new MyVokiInvitesPageState();
	

	let acceptInviteDialog = $state<AcceptInviteConfirmationDialog>()!;
	let declineInviteDialog = $state<DeclineInviteConfirmationDialog>()!;
</script>

{#if pageState.loadingState.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your invites" />
{:else if pageState.loadingState.state === 'errs'}
	<DefaultErrBlock errList={pageState.loadingState.errs} />
{:else if pageState.loadingState.state === 'loaded'}
	{#if pageState.loadingState.invites.length === 0}
		<h1>You don't have any invites</h1>
	{:else}
		<AcceptInviteConfirmationDialog
			bind:this={acceptInviteDialog}
			deleteInviteOnSuccessAccept={(vokiId) => pageState.deleteInvite(vokiId)}
		/>
		<DeclineInviteConfirmationDialog
			bind:this={declineInviteDialog}
			deleteInviteOnSuccessDecline={(vokiId) => pageState.deleteInvite(vokiId)}
		/>
		<div class="invites-container">
			{#each pageState.loadingState.invites as inv}
				<InviteForCoAuthorDisplay
					invite={inv}
					onAccept={() => acceptInviteDialog.open(inv)}
					onDecline={() => declineInviteDialog.open(inv)}
				/>
			{/each}
		</div>
	{/if}
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}

<style>
	.invites-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>
