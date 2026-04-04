<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import SignInDialogConfirmButton from './_c_states_shared/SignInDialogConfirmButton.svelte';
	import SignInDialogHeader from './_c_states_shared/SignInDialogHeader.svelte';
	import SignInDialogInput from './_c_states_shared/SignInDialogInput.svelte';
	import SignInDialogLink from './_c_states_shared/SignInDialogLink.svelte';
	import { ApiAuth, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { SignInDialogState } from '../../_ts_layout_contexts/sign-in-dialog-context';

	interface Props {
		email: string;
		password: string;
		changeState: (val: SignInDialogState) => void;
	}
	let { email = $bindable(), password = $bindable(), changeState }: Props = $props();
	let uniqueName = $state('');
	let errs: Err[] = $state([]);
	let isLoading = $state(false);

	export function clearErrs() {
		errs = [];
	}

	async function confirmSignUp() {
		if (isLoading) {
			return;
		}
		validateForm();
		if (errs.length > 0) {
			return;
		}
		isLoading = true;
		const response = await ApiAuth.fetchVoidResponse(
			'/sign-up',
			RJO.POST({ email, password, uniqueName })
		);
		isLoading = false;
		if (response.isSuccess) {
			changeState('confirmation-sent');
		} else {
			errs = response.errs;
		}
	}
	function validateForm(): Err[] {
		errs = [];
		if (StringUtils.isNullOrWhiteSpace(email)) {
			errs.push({ message: 'Email is required' });
		} else if (email.indexOf('@') === -1) {
			errs.push({ message: 'Email is invalid' });
		}
		if (StringUtils.isNullOrWhiteSpace(password)) {
			errs.push({ message: 'Password is required' });
		}
		if (StringUtils.isNullOrWhiteSpace(uniqueName)) {
			errs.push({ message: 'User name is required', details: 'Fill the username field' });
		}
		return errs;
	}
</script>

<SignInDialogHeader text="Create Vokimi account" />
<SignInDialogInput type="text" fieldName="Unique name" bind:value={uniqueName}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M2 12C2 7.75736 2 5.63604 3.46447 4.31802C4.92893 3 7.28596 3 12 3C16.714 3 19.0711 3 20.5355 4.31802C22 5.63604 22 7.75736 22 12C22 16.2426 22 18.364 20.5355 19.682C19.0711 21 16.714 21 12 21C7.28596 21 4.92893 21 3.46447 19.682C2 18.364 2 16.2426 2 12Z"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M5 16.5C6.20831 13.9189 10.7122 13.7491 12 16.5M10.5 9.5C10.5 10.6046 9.60457 11.5 8.5 11.5C7.39543 11.5 6.5 10.6046 6.5 9.5C6.5 8.39543 7.39543 7.5 8.5 7.5C9.60457 7.5 10.5 8.39543 10.5 9.5Z"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
		></path>
		<path
			d="M15 10H19"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M15 14H19"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
</SignInDialogInput>
<SignInDialogInput type="email" fieldName="Email" bind:value={email}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M2 5L8.91302 8.92462C11.4387 10.3585 12.5613 10.3585 15.087 8.92462L22 5"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linejoin="round"
		/>
		<path
			d="M10.5 19.5C10.0337 19.4939 9.56682 19.485 9.09883 19.4732C5.95033 19.3941 4.37608 19.3545 3.24496 18.2184C2.11383 17.0823 2.08114 15.5487 2.01577 12.4814C1.99475 11.4951 1.99474 10.5147 2.01576 9.52843C2.08114 6.46113 2.11382 4.92748 3.24495 3.79139C4.37608 2.6553 5.95033 2.61573 9.09882 2.53658C11.0393 2.4878 12.9607 2.48781 14.9012 2.53659C18.0497 2.61574 19.6239 2.65532 20.755 3.79141C21.8862 4.92749 21.9189 6.46114 21.9842 9.52844C21.9939 9.98251 21.9991 10.1965 21.9999 10.5"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M19 17C19 17.8284 18.3284 18.5 17.5 18.5C16.6716 18.5 16 17.8284 16 17C16 16.1716 16.6716 15.5 17.5 15.5C18.3284 15.5 19 16.1716 19 17ZM19 17V17.5C19 18.3284 19.6716 19 20.5 19C21.3284 19 22 18.3284 22 17.5V17C22 14.5147 19.9853 12.5 17.5 12.5C15.0147 12.5 13 14.5147 13 17C13 19.4853 15.0147 21.5 17.5 21.5"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
</SignInDialogInput>
<SignInDialogInput type="password" fieldName="Password" bind:value={password}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M4.26781 18.8447C4.49269 20.515 5.87613 21.8235 7.55966 21.9009C8.97627 21.966 10.4153 22 12 22C13.5847 22 15.0237 21.966 16.4403 21.9009C18.1239 21.8235 19.5073 20.515 19.7322 18.8447C19.879 17.7547 20 16.6376 20 15.5C20 14.3624 19.879 13.2453 19.7322 12.1553C19.5073 10.485 18.1239 9.17649 16.4403 9.09909C15.0237 9.03397 13.5847 9 12 9C10.4153 9 8.97627 9.03397 7.55966 9.09909C5.87613 9.17649 4.49269 10.485 4.26781 12.1553C4.12105 13.2453 4 14.3624 4 15.5C4 16.6376 4.12105 17.7547 4.26781 18.8447Z"
			stroke="currentColor"
			stroke-width="1.5"
		/>
		<path
			d="M7.5 9V6.5C7.5 4.01472 9.51472 2 12 2C14.4853 2 16.5 4.01472 16.5 6.5V9"
			stroke="currentColor"
			stroke-width="1.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M16 15.49V15.5"
			stroke="currentColor"
			stroke-width="2"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M12 15.49V15.5"
			stroke="currentColor"
			stroke-width="2"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M8 15.49V15.5"
			stroke="currentColor"
			stroke-width="2"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
</SignInDialogInput>
<div class="gap" />

<SignInDialogLink text="I already have an account" onClick={() => changeState('login')} />
<DefaultErrBlock errList={errs} class="sign-up-err-block" />
<SignInDialogConfirmButton text="Sign Up" onclick={() => confirmSignUp()} {isLoading} />

<style>
	.gap {
		margin-top: auto;
	}

	:global(.err-block.sign-up-err-block) {
		margin-top: 0.375rem;
	}
</style>
