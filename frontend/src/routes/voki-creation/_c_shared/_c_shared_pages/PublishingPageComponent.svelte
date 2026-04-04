<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type {
		VokiPublishingIssue,
		VokiSuccessfullyPublishedData
	} from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { getVokiCreationPageContext } from '../../voki-creation-page-context';
	import ConfirmVokiPublishingDialog from './_c_publishing/ConfirmVokiPublishingDialog.svelte';
	import NoVokiPublishingIssues from './_c_publishing/NoVokiPublishingIssues.svelte';
	import VokiPublishingIssuesList from './_c_publishing/VokiPublishingIssuesList.svelte';
	import VokiSuccessfullyPublishedMessage from './_c_publishing/VokiSuccessfullyPublishedMessage.svelte';

	interface Props {
		vokiId: string;
		initialIssues: VokiPublishingIssue[];
		primaryAuthorId: string;
		coAuthorIds: string[];
		userIdsToBecomeManagers: string[];
	}
	let { vokiId, initialIssues, primaryAuthorId, coAuthorIds, userIdsToBecomeManagers }: Props =
		$props();
	const vokiCreationCtx = getVokiCreationPageContext();

	let pageState = $state<PageState>({ name: 'issues', issues: initialIssues });

	type PageState =
		| { name: 'loading' }
		| { name: 'error'; errs: Err[] }
		| { name: 'issues'; issues: VokiPublishingIssue[] }
		| { name: 'published'; vokiData: VokiSuccessfullyPublishedData };

	async function loadPublishingIssues() {
		pageState = { name: 'loading' };
		const response = await vokiCreationCtx.vokiCreationApi.loadPublishingData(vokiId);
		if (response.isSuccess) {
			pageState = { name: 'issues', issues: response.data.issues };
		} else {
			pageState = { name: 'error', errs: response.errs };
		}
	}
	let confirmVokiPublishedDialog = $state<ConfirmVokiPublishingDialog>()!;
	function switchToPublishedSuccessfully(vokiData: VokiSuccessfullyPublishedData) {
		pageState = { name: 'published', vokiData };
	}

	function isUserPrimaryAuthor(userId: string) {
		return userId === primaryAuthorId;
	}
</script>

<ConfirmVokiPublishingDialog
	bind:this={confirmVokiPublishedDialog}
	issues={pageState.name === 'issues' ? pageState.issues : []}
	refetchIssues={() => loadPublishingIssues()}
	{switchToPublishedSuccessfully}
	{vokiId}
	{coAuthorIds}
	{userIdsToBecomeManagers}
/>

{#if pageState.name === 'loading'}
	<div class="msg-container loading">
		<CubesLoader sizeRem={5} color="var(--primary)" />
		<label>Searching for issues</label>
	</div>
{:else if pageState.name === 'error'}
	<div class="msg-container error">
		<DefaultErrBlock errList={pageState.errs} />
		<PrimaryButton onclick={() => loadPublishingIssues()} class="refetch">Refetch</PrimaryButton>
	</div>
{:else if pageState.name === 'issues' && pageState.issues.length != 0}
	<VokiPublishingIssuesList
		issues={pageState.issues}
		refetch={() => loadPublishingIssues()}
		openPublishingConfirmationDialog={() => confirmVokiPublishedDialog.open()}
		{isUserPrimaryAuthor}
	/>
{:else if pageState.name === 'issues' && pageState.issues.length === 0}
	<NoVokiPublishingIssues
		openPublishingConfirmationDialog={() => confirmVokiPublishedDialog.open()}
		{isUserPrimaryAuthor}
	/>
{:else if pageState.name === 'published'}
	<VokiSuccessfullyPublishedMessage vokiData={pageState.vokiData} />
{:else}
	<h2>Something went wrong. Please refresh the page</h2>
{/if}

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 52rem;
		height: 18rem;
		padding: 2rem;
		margin: 0 auto;
		margin-top: 8rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow);
		animation: var(--default-fade-in-animation);
	}

	.msg-container > :global(.check-for-issues-btn) {
		margin-top: auto;
	}

	.msg-container.loading {
		justify-content: center;
		gap: 2rem;
	}

	.msg-container.loading > label {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 1px;
	}

	.msg-container.error {
		border: 0.125rem dashed var(--red-5);
		background-color: var(--back);
		box-shadow: var(--err-shadow);
	}

	.msg-container.error > :global(.refetch) {
		margin-top: auto;
	}
</style>
