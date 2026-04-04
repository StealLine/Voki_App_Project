<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import type { ResultOverViewData } from './types';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	const {
		vokiId,
		updateParentResults
	}: { vokiId: string; updateParentResults: (newResults: ResultOverViewData[]) => void } = $props<{
		vokiId: string;
		updateParentResults: (newResults: ResultOverViewData[]) => void;
	}>();
	let dialog = $state<DialogWithCloseButton>()!;
	let name = $state<string>('');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
		name = '';
	}
	async function submitCreate() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			results: ResultOverViewData[];
		}>(`/vokis/${vokiId}/results/add-new`, RJO.POST({ resultName: name }));
		if (response.isSuccess) {
			updateParentResults(response.data.results);
			dialog.close();
		} else {
			errs = response.errs;
		}
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-result-initializing-dialog"
	bind:this={dialog}
	subheading="Enter the name of the new result"
>
	<input bind:value={name} class="name-input" type="text" placeholder="Result name..." />
	<DefaultErrBlock errList={errs} />
	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>

<style>
	.name-input {
		width: 40rem;
		height: 2.5rem;
		padding: 0.125rem 0.5rem;
		margin: 0 2rem;
		border: none;
		border: 0.2rem solid transparent;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 420;
		letter-spacing: 0.25px;
		box-shadow: var(--shadow);
		transition: background-color 0.08s ease-in-out;
	}

	.name-input:focus {
		outline: none;
		border-color: var(--primary);
	}

	:global(#general-voki-result-initializing-dialog .err-block) {
		margin: 1rem;
	}

	:global(#general-voki-result-initializing-dialog .primary-btn) {
		padding: 0.25rem 1.5rem;
		margin: 2rem 0 0;
	}
</style>
