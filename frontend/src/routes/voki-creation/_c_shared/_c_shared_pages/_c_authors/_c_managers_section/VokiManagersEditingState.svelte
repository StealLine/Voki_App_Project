<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import VokiCreationOneOfMultipleChoicesInput from '../../../VokiCreationOneOfMultipleChoicesInput.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import type { VokiExpectedManagersSetting } from '../types';
	import CoAuthorsToBecomeManagersChoosingDialog from './_c_editing_state/CoAuthorsToBecomeManagersChoosingDialog.svelte';

	interface Props {
		cancelEditing: () => void;
		vokiCoAuthors: string[];
		savedSelectedUserIds: string[];
		initialMakeAllManagers: boolean;
		updateParent: (setting: VokiExpectedManagersSetting) => void;
		vokiId: string;
	}
	let {
		cancelEditing,
		vokiCoAuthors,
		savedSelectedUserIds,
		initialMakeAllManagers,
		updateParent,
		vokiId
	}: Props = $props();

	type ExpectedManagersType =
		| { name: 'all' }
		| { name: 'selected'; userIds: string[] }
		| { name: 'none' };
	let expectedManagers: ExpectedManagersType = $derived(
		initialMakeAllManagers
			? { name: 'all' }
			: savedSelectedUserIds.length > 0
				? { name: 'selected', userIds: savedSelectedUserIds }
				: { name: 'none' }
	);
	let dialog = $state<CoAuthorsToBecomeManagersChoosingDialog>()!;
	let savingErrs: Err[] = $state([]);
	let isLoading = $state(false);
	function onCoAuthorRemoveBtnClick(coAuthorId: string) {
		if (expectedManagers.name === 'selected') {
			expectedManagers.userIds = expectedManagers.userIds.filter((id) => id !== coAuthorId);
		}
	}
	async function saveChanges() {
		savingErrs = [];
		const bodyObj: { makeAllCoAuthorsManagers: boolean; userIdsToBecomeManagers: string[] } =
			expectedManagers.name === 'all'
				? { makeAllCoAuthorsManagers: true, userIdsToBecomeManagers: [] }
				: expectedManagers.name === 'selected'
					? { makeAllCoAuthorsManagers: false, userIdsToBecomeManagers: expectedManagers.userIds }
					: { makeAllCoAuthorsManagers: false, userIdsToBecomeManagers: [] };
		isLoading = true;
		const response = await ApiVokiCreationCore.fetchJsonResponse<VokiExpectedManagersSetting>(
			`/vokis/${vokiId}/update-expected-managers`,
			RJO.PATCH(bodyObj)
		);
		isLoading = false;
		if (response.isSuccess) {
			updateParent(response.data);
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<p class="managers-input-p">
	<VokiCreationFieldName fieldName="Managers after publishing:" />
	<VokiCreationOneOfMultipleChoicesInput
		containerClass="voki-managers-input"
		options={[
			{
				name: 'All co-authors',
				selected: expectedManagers.name === 'all',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'all' })
			},
			{
				name: 'Selected co-authors',
				selected: expectedManagers.name === 'selected',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'selected', userIds: savedSelectedUserIds })
			},
			{
				name: 'No co-authors',
				selected: expectedManagers.name === 'none',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'none' })
			}
		]}
	/>
</p>
<p class="hint-p">
	{#if expectedManagers.name === 'all'}
		All co-authors will become managers when Voki will be published.
	{:else if expectedManagers.name === 'selected'}
		Selected co-authors will become managers when Voki will be published.
	{:else if expectedManagers.name === 'none'}
		No co-authors will become managers when Voki will be published.
	{/if}
	You will always be able to change managers list in the future
</p>
{#if expectedManagers.name === 'selected'}
	<div class="co-authors-select-container">
		<label class="chosen-users-label">Chosen users:</label>
		{#each expectedManagers.userIds as userId}
			<div class="co-author">
				<BasicUserDisplay {userId} interactionLevel={'UniqueNameGotoOnClick'} />
				<svg class="remove-btn" onclick={() => onCoAuthorRemoveBtnClick(userId)}
					><use href="#common-cross-icon" /></svg
				>
			</div>
		{/each}
		<button
			class="edit-co-authors-btn"
			onclick={() =>
				dialog.open(expectedManagers.name === 'selected' ? expectedManagers.userIds : [])}
			>{expectedManagers.userIds.length > 0 ? 'Change' : 'Choose first co-author'}
		</button>
	</div>
{/if}
<CoAuthorsToBecomeManagersChoosingDialog
	bind:this={dialog}
	allCoAuthors={vokiCoAuthors}
	choseCoAuthors={(coAuthorIds) => (expectedManagers = { name: 'selected', userIds: coAuthorIds })}
/>
<DefaultErrBlock errList={savingErrs} />
<VokiCreationSaveAndCancelButtons
	onCancel={cancelEditing}
	onSave={() => saveChanges()}
	isSaveLoading={isLoading}
/>

<style>
	.managers-input-p {
		display: grid;
		grid-template-columns: max-content 1fr;
		align-items: center;
	}

	.managers-input-p > :global(.voki-managers-input) {
		gap: 3rem;
		width: fit-content;
		margin-left: auto;
	}

	.managers-input-p > :global(.voki-managers-input > .option) {
		width: 13rem;
		padding: 0.375rem 0;
	}

	.hint-p {
		margin: 0.375rem 0;
		margin-top: 0.5rem;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 425;
	}

	.co-authors-select-container {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		gap: 1rem;
	}

	.chosen-users-label {
		padding: 0.875rem 0;
		color: var(--muted-foreground);
		font-weight: 500;
		letter-spacing: 0.125px;
	}

	.co-author {
		display: flex;
		align-items: center;
		gap: 0.5rem;
	}

	.co-author > .remove-btn {
		width: 1.25rem;
		height: 1.25rem;
		padding: 0.125rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		cursor: pointer;
		stroke-width: 2;
	}

	.co-author > .remove-btn:hover {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.edit-co-authors-btn {
		height: fit-content;
		padding: 0.25rem 1.25rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.25px;
	}

	.edit-co-authors-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
