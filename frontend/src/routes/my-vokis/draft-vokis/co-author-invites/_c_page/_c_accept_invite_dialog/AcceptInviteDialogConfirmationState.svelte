<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { InviteForVokiCoAuthorData } from '../../my-voki-invites-page-state.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		changeStateToConfirmed: (inviteData: InviteForVokiCoAuthorData) => void;
		closeDialog: () => void;
		deleteInviteOnSuccessAccept: (vokiId: string) => void;
	}
	let { invite, changeStateToConfirmed, closeDialog, deleteInviteOnSuccessAccept }: Props =
		$props();

	let isLoading = $state(false);
	let errs = $state<Err[]>([]);

	async function acceptInvite() {
		errs = [];
		isLoading = true;
		const response = await ApiVokiCreationCore.fetchVoidResponse(
			`/vokis/${invite.vokiId}/accept-co-author-invite`,
			RJO.PATCH({})
		);
		isLoading = false;

		if (response.isSuccess) {
			deleteInviteOnSuccessAccept(invite.vokiId);
			changeStateToConfirmed(invite);
		} else {
			errs = response.errs;
		}
	}
	export function reset() {
		isLoading = false;
		errs = [];
	}
</script>

{#if isLoading}
	<div class="loading-backdrop" aria-hidden="true">
		<div class="loading-content">
			<CubesLoader sizeRem={4} speedSec={1.75} color="var(--primary)" />
			<p class="loading-text">We are adding you to Voki co-authors<br /> Please wait a bit</p>
		</div>
	</div>
{/if}
<p class="main-text">
	Are you sure you want to join
	<BasicUserDisplay userId={invite.primaryAuthorId} interactionLevel="WholeComponentLink" />
	in the creation of <span class="voki-name">{invite.vokiName}</span> Voki as a co-author?
</p>

<DefaultErrBlock errList={errs} class="confirm-invite-errs-block" />

<div class="buttons">
	<button class="btn secondary" disabled={isLoading} onclick={() => closeDialog()}> Cancel </button>
	<button
		class="btn primary"
		disabled={isLoading}
		aria-busy={isLoading}
		onclick={() => acceptInvite()}
	>
		Confirm
	</button>
</div>

<style>
	.main-text {
		margin: auto 0;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 475;
		line-height: 1.375;
		text-align: justify;
		text-indent: 1em;
		text-wrap: pretty;
	}

	.main-text > :global(.user-display) {
		display: inline-grid;
		vertical-align: middle;

		--profile-pic-width: 2.375rem;

		margin: 0.125rem 0.25rem;
	}

	.main-text > .voki-name {
		border-radius: 0.5rem;
		color: var(--muted-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		text-decoration: underline;
		text-indent: 0;
	}

	:global(.confirm-invite-errs-block) {
		margin: 0.5rem 0;
	}

	.buttons {
		display: flex;
		justify-content: flex-end;
		gap: 0.75rem;
		width: 100%;
		margin-top: auto;
	}

	.btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		width: 7.5rem;
		padding: 0.375rem 1rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		cursor: pointer;
	}

	.btn.primary {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover:where(:not([disabled])) {
		background-color: var(--primary-hov);
	}

	.btn.secondary {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.btn.secondary:hover:where(:not([disabled])) {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.loading-backdrop {
		position: absolute;
		display: grid;
		place-content: center center;
		width: 100%;
		height: 100%;
		border-radius: inherit;
		background: rgb(40 40 40 / 6%);
		animation: var(--default-fade-in);
		place-items: center;
		inset: 0;
		backdrop-filter: blur(1px);
	}

	.loading-content {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1.5rem;
		width: fit-content;
		padding: 2rem;
		margin-bottom: 1rem;
		border-radius: 1rem;
		background-color: var(--back);
	}

	.loading-text {
		color: var(--muted-foreground);
		font-size: 1.375rem;
		font-weight: 475;
		line-height: 1.125;
		text-align: center;
		letter-spacing: 0.5px;
	}
</style>
