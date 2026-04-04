<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type {
		VokiPublishingIssue,
		VokiSuccessfullyPublishedData
	} from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { getVokiCreationPageContext } from '../../../voki-creation-page-context';
	import ConfirmCoAuthorsAndManagersMsg from './_c_confirm_dialog/ConfirmCoAuthorsAndManagersMsg.svelte';
	interface Props {
		issues: VokiPublishingIssue[];
		refetchIssues: () => void;
		switchToPublishedSuccessfully: (data: VokiSuccessfullyPublishedData) => void;
		vokiId: string;
		coAuthorIds: string[];
		userIdsToBecomeManagers: string[];
	}
	let {
		issues,
		switchToPublishedSuccessfully,
		vokiId,
		coAuthorIds,
		userIdsToBecomeManagers
	}: Props = $props();
	export function open() {
		publishingErrs = [];
		dialog.open();
	}
	let dialog = $state<DialogWithCloseButton>()!;
	let publishingErrs: Err[] = $state([]);
	const vokiCreationCtx = getVokiCreationPageContext();
	async function publishWithIssuesIgnored() {
		const response = await vokiCreationCtx.vokiCreationApi.publishWithWarningsIgnored(
			vokiId,
			coAuthorIds,
			userIdsToBecomeManagers
		);
		if (response.isSuccess) {
			switchToPublishedSuccessfully(response.data);
			dialog.close();
		} else {
			publishingErrs = response.errs;
		}
	}

	async function publishWithNoIssues() {
		const response = await vokiCreationCtx.vokiCreationApi.publishWithNoIssues(
			vokiId,
			coAuthorIds,
			userIdsToBecomeManagers
		);
		if (response.isSuccess) {
			switchToPublishedSuccessfully(response.data);
			dialog.close();
		} else {
			publishingErrs = response.errs;
		}
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="confirm-voki-publishing-dialog">
	<label class="warning-label">Warning</label>
	<p class="warning-text">
		Publishing your Voki will make it public — anyone can view it and take it. After publishing,
		most parts can’t be changed, so make sure it’s ready. Click the button below to run the check.
		We’ll scan your Voki for issues and show you if anything needs fixing.
	</p>

	<ConfirmCoAuthorsAndManagersMsg {coAuthorIds} {userIdsToBecomeManagers} />
	<DefaultErrBlock errList={publishingErrs} />
	{#if issues.length > 0}
		<div class="issues-msg">
			<svg><use href="#common-warning-icon" /></svg>
			Voki has {issues.length} publishing issues. Its is highly recommended to fix them before publishing
		</div>
		<PrimaryButton onclick={() => dialog.close()}>Fix issues</PrimaryButton>
		<button onclick={() => publishWithIssuesIgnored()} class="ignore-and-publish-btn"
			>Ignore and publish</button
		>
	{:else}
		<PrimaryButton onclick={() => publishWithNoIssues()}>Publish</PrimaryButton>
	{/if}
</DialogWithCloseButton>

<style>
	.warning-label {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 1px;
	}

	.warning-text {
		width: 55ch;
		margin-top: 1rem;
		font-size: 1.25rem;
		font-weight: 425;
		line-height: 1.375;
		text-align: justify;
		letter-spacing: 0.375px;
		text-indent: 1em;
	}

	.issues-msg {
		width: 38ch;
		padding: 0.25rem 0;
		margin: 0.25rem 0;
		border-radius: 1rem;
		background-color: var(--warn-back);
		color: var(--warn-foreground);
		font-size: 1rem;
		font-weight: 425;
		line-height: 1.25;
		text-align: center;
		text-wrap: balance;
	}

	.issues-msg > svg {
		width: 1.25rem;
		height: 1.25rem;
		vertical-align: middle;
		stroke-width: 2;
	}

	:global(#confirm-voki-publishing-dialog .primary-btn) {
		padding-right: 2rem;
		padding-left: 2rem;
		margin-top: 1rem;
		letter-spacing: 0.5px;
	}

	.ignore-and-publish-btn {
		padding: 0.125rem 1rem;
		margin-top: 0.5rem;
		border: none;
		border-radius: 0.5rem;
		background-color: transparent;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		transition: all 0.08s ease-in-out;
		cursor: pointer;
	}

	.ignore-and-publish-btn:hover {
		background-color: var(--secondary);
		text-decoration: underline;
	}
</style>
