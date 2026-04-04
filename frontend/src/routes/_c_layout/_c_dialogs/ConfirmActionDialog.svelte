<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { Snippet } from 'svelte';
	import type {
		ConfirmActionDialogButtons,
		ConfirmActionDialogContent,
		ConfirmActionDialogMainContent
	} from '../_ts_layout_contexts/confirm-action-dialog-context';
	import type { Err } from '$lib/ts/err';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';

	let content = $state<ConfirmActionDialogMainContent | Snippet>();
	let buttons = $state<ConfirmActionDialogButtons | Snippet>();

	function isContentText(
		val: ConfirmActionDialogMainContent | Snippet | undefined
	): val is ConfirmActionDialogMainContent {
		return (val && 'mainText' in val) ?? false;
	}
	function isDialogButtons(
		val: ConfirmActionDialogButtons | Snippet | undefined
	): val is ConfirmActionDialogButtons {
		return (val && 'confirmBtnText' in val && 'confirmBtnOnclick' in val) ?? false;
	}
	let dialog = $state<DialogWithCloseButton>()!;

	export function open(dialogContent: ConfirmActionDialogContent) {
		confirmErrs = [];
		content = dialogContent.mainContent;
		buttons = dialogContent.dialogButtons;
		dialog.open();
	}
	export function close() {
		dialog.close();
	}
	async function onConfirm() {
		if (isDialogButtons(buttons)) {
			let res = await buttons.confirmBtnOnclick();
			if (res && res.length > 0) {
				confirmErrs = res;
			} else {
				dialog.close();
			}
		}
	}
	let confirmErrs = $state<Err[]>([]);
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="confirm-action-dialog"
	subheading={isContentText(content) ? content.subheading : undefined}
>
	{#if isContentText(content)}
		<div class="text">{content.mainText}</div>
	{:else if content}
		{@render content()}
	{/if}

	{#if isDialogButtons(buttons)}
		{#if confirmErrs.length > 0}
			<DefaultErrBlock errList={confirmErrs} />
		{/if}
		<div class="buttons">
			<button class="confirm" onclick={onConfirm}>{buttons.confirmBtnText}</button>
			<button class="cancel" onclick={buttons.cancelBtnOnclick}>{buttons.cancelBtnText}</button>
		</div>
	{:else if buttons}
		{@render buttons()}
	{/if}
</DialogWithCloseButton>

<style>
	.buttons {
		display: grid;
		gap: 0.75rem;
		margin-top: 2rem;
		align-self: end;
		grid-template-columns: 1fr 1fr;
	}

	.text {
		padding: 0 1rem;
		font-size: 2rem;
		font-weight: 450;
		text-align: center;
		text-wrap: pretty;
	}

	.buttons button {
		padding: 0.375rem 1.5rem;
		border: none;
		border-radius: 0.5rem;
		font-size: 1.5rem;
		text-align: center;
		transition: all 0.08s ease-in;
		cursor: pointer;
		outline: none;
	}

	.confirm {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.confirm:hover {
		background-color: var(--primary-hov);
	}

	.cancel {
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 480;
	}

	.cancel:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
