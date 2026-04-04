<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	interface Props {
		goToPage: () => void;
		onBeforeCancel: () => void;
	}

	let { goToPage, onBeforeCancel }: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;

	export function open() {
		dialog.open();
	}

	export function close() {
		dialog.close();
	}
	function onConfirm() {
		goToPage();
		dialog.close();
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	onBeforeClose={onBeforeCancel}
	dialogId="voki-creationunsaved-changes-confirm-dialog"
>
	<p class="message">You have unsaved changes.<br />Are you sure you want to leave?</p>
	<div class="actions">
		<button class="stay-btn" onclick={() => dialog.close()}>Stay</button>
		<button class="leave-btn" onclick={onConfirm}>Leave</button>
	</div>
</DialogWithCloseButton>

<style>
	:global(#voki-creationunsaved-changes-confirm-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		width: 100%;
		padding: 3rem;
	}

	.message {
		color: var(--muted-foreground);
		font-size: 1.25em;
		font-weight: 450;
		line-height: 1.2;
		text-align: center;
		letter-spacing: 0.25px;
		text-wrap: balance;
	}

	.actions {
		display: grid;
		gap: 2rem;
		width: 100%;
		padding: 0 2rem;
		grid-template-columns: 1fr 1fr;
	}

	.stay-btn,
	.leave-btn {
		padding: 0.5rem 0;
		border: none;
		border-radius: 0.25rem;
		font-size: 1rem;
		font-weight: 500;
		transition: 0.15s ease;
		cursor: pointer;
	}

	.stay-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.stay-btn:hover {
		background-color: var(--primary-hov);
	}

	.leave-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.leave-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
