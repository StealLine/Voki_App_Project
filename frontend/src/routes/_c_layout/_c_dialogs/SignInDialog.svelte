<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { SignInDialogState } from '../_ts_layout_contexts/sign-in-dialog-context';
	import ConfirmationLinkState from './_c_sign_in_dialog/ConfirmationLinkState.svelte';
	import LoginState from './_c_sign_in_dialog/LoginState.svelte';
	import SignUpState from './_c_sign_in_dialog/SignUpState.svelte';

	export function open(state: SignInDialogState | null = null) {
		signUpStateComponent?.clearErrs();
		loginStateComponent?.clearErrs();

		dialog.open();
		if (state) {
			dialogState = state;
		}
	}
	let dialogState = $state<SignInDialogState>('signup');
	let dialog = $state<DialogWithCloseButton>()!;
	let email = $state('');
	let password = $state('');

	let signUpStateComponent = $state<SignUpState>();
	let loginStateComponent = $state<LoginState>();
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="sign-in-dialog" closedby="any">
	{#if dialogState === 'login'}
		<LoginState
			bind:this={loginStateComponent}
			bind:email
			bind:password
			changeState={(val) => (dialogState = val)}
		/>
	{:else if dialogState === 'signup'}
		<SignUpState
			bind:this={signUpStateComponent}
			bind:email
			bind:password
			changeState={(val) => (dialogState = val)}
		/>
	{:else if dialogState === 'confirmation-sent'}
		<ConfirmationLinkState {email} />
	{:else}
		<SignUpState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{/if}
</DialogWithCloseButton>

<style>
	:global(#sign-in-dialog .dialog-content) {
		width: 28rem;
		height: 34rem;
	}
</style>
