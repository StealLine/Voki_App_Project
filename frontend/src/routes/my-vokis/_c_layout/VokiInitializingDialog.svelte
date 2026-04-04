<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiType } from '$lib/ts/voki-type';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import VokiDialogInititalizingState from './_c_initializing_dialog/VokiDialogInititalizingState.svelte';
	import VokiDialogInputsState from './_c_initializing_dialog/VokiDialogInputsState.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	type DialogState =
		| { name: 'input' }
		| { name: 'initLoading' }
		| { name: 'existsCheckLoading' }
		| { name: 'existsCheckFailed'; vokiId: string }
		| { name: 'success'; vokiId: string; vokiType: VokiType; vokiName: string };

	let dialogState = $state<DialogState>({ name: 'input' });

	let selectedVokiType = $state<VokiType>('General');
	let vokiName = $state('');
	let errs: Err[] = $state([]);

	export function open() {
		dialogState = { name: 'input' };
		selectedVokiType = 'General';
		vokiName = '';
		errs = [];
		dialog.open();
	}

	async function onSubmitBtnClicked() {
		validateForm();
		if (errs.length > 0) {
			return;
		}
		dialogState = { name: 'initLoading' };
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			id: string;
			name: string;
			type: VokiType;
		}>('/initialize-new-voki', RJO.POST({ newVokiName: vokiName, vokiType: selectedVokiType }));
		if (response.isSuccess) {
			if (response.data.type === 'General') {
				checkIfVokiExistsInSpecificService(response.data);
			} else {
				onNewVokiInitialized(response.data);
			}
		} else {
			dialogState = { name: 'input' };
			errs = response.errs;
		}
	}

	async function checkIfVokiExistsInSpecificService(vokiData: {
		id: string;
		name: string;
		type: VokiType;
	}) {
		dialogState = { name: 'existsCheckLoading' };
		const vokiId = vokiData.id;
		const startTime = Date.now();
		const timeout = 5000;

		while (Date.now() - startTime < timeout) {
			const existsResult = await ApiVokiCreationGeneral.ensureVokiExists(vokiId);
			if (existsResult) {
				onNewVokiInitialized(vokiData);
				return;
			}
			await new Promise((resolve) => setTimeout(resolve, 500));
		}

		dialogState = { name: 'existsCheckFailed', vokiId };
	}

	function onNewVokiInitialized(newVokiData: { id: string; name: string; type: VokiType }) {
		dialogState = {
			name: 'success',
			vokiId: newVokiData.id,
			vokiType: newVokiData.type,
			vokiName: newVokiData.name
		};
	}

	function validateForm(): Err[] {
		errs = [];
		if (StringUtils.isNullOrWhiteSpace(vokiName)) {
			errs.push({ message: 'Voki name cannot be empty' });
		}
		if (selectedVokiType != 'General') {
			errs.push({ message: 'Unsupported voki type' });
		}
		return errs;
	}
</script>

<DialogWithCloseButton dialogId="voki-initializing-dialog" bind:this={dialog}>
	{#if dialogState.name === 'input'}
		<VokiDialogInputsState
			bind:vokiName
			bind:selectedVokiType
			{errs}
			onSubmit={onSubmitBtnClicked}
		/>
	{:else if dialogState.name === 'initLoading' || dialogState.name === 'existsCheckLoading' || dialogState.name === 'existsCheckFailed' || dialogState.name === 'success'}
		<VokiDialogInititalizingState state={dialogState} />
	{:else}
		<p>Unknown state</p>
		<button onclick={() => (dialogState = { name: 'input' })}>Refresh</button>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#voki-initializing-dialog > .dialog-content) {
		width: 48rem;
		height: 38rem;
	}
</style>
