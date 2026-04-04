<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { getSignInDialogOpenFunction } from '../../routes/_c_layout/_ts_layout_contexts/sign-in-dialog-context';

	let { closeDialog }: { closeDialog: () => void } = $props<{ closeDialog: () => void }>();
	const openSignInDialog = getSignInDialogOpenFunction();

	function createNewAccount() {
		closeDialog();
		openSignInDialog('signup');
	}
	function login() {
		closeDialog();
		openSignInDialog('login');
	}
</script>

<div class="sign-in-required">
	<h1>
		Yoo need to sign in to use albums
		<svg class="icon-1"><use href="#albums-bookmark-1-icon" /></svg>
		<svg class="icon-2"><use href="#albums-bookmark-2-icon" /></svg>
		<svg class="icon-3"><use href="#albums-clock-1-icon" /></svg>
		<svg class="icon-4"><use href="#albums-clock-2-icon" /></svg>
		<svg class="icon-5"><use href="#albums-star-1-icon" /></svg>
		<svg class="icon-6"><use href="#albums-star-2-icon" /></svg>
	</h1>
	<div class="buttons-container">
		<PrimaryButton onclick={createNewAccount}>Create an account</PrimaryButton>
		<label onclick={login} class="login-btn">I already have an account</label>
	</div>
</div>

<style>
	.sign-in-required {
		display: flex;
		flex-direction: column;
		width: 100%;
		height: 100%;
	}

	h1 {
		position: relative;
		padding: 4rem 6rem;
		letter-spacing: 0.5px;
		white-space: nowrap;
	}

	.buttons-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		margin-top: auto;
	}

	.login-btn {
		padding: 0.125rem 1rem;
		border-radius: 0.375rem;
		background-color: var(--back-main);
		color: var(--secondary-foreground);
		font-size: 1rem;
		text-decoration: underline;
		transition: all 0.12s ease;
		cursor: pointer;
	}

	.login-btn:hover {
		background-color: var(--secondary);
		color: var(--primary);
	}

	.login-btn:active {
		color: var(--primary-hov);
		transform: scale(0.98);
	}

	svg {
		position: absolute;
		z-index: 0;
		width: 2.5rem;
		height: 2.5rem;
		opacity: 0.8;
		stroke-width: 1.5;
		filter: drop-shadow(0 4px 6px rgb(0 0 0 / 5%));
	}

	.icon-1 {
		top: 10%;
		left: 8%;
		width: 2.125rem;
		height: 2.125rem;

		--icon-color-1: var(--primary);
		--icon-color-2: var(--primary);

		transform: rotate(-10deg);
	}

	.icon-2 {
		top: 2%;
		left: 32%;
		width: 2.75rem;
		height: 2.75rem;

		--icon-color-2: #f15bb5;
		--icon-color-1: #9b5de5;

		transform: rotate(2deg);
	}

	.icon-3 {
		top: 75%;
		right: 10%;
		width: 2.875rem;
		height: 2.875rem;

		--icon-color-1: #bc8a5f;
		--icon-color-2: #6f4518;
	}

	.icon-4 {
		top: 65%;
		left: 25%;

		--icon-color-1: #293241;
		--icon-color-2: #ee6c4d;

		transform: rotate(15deg);
	}

	.icon-5 {
		bottom: 10%;
		left: 45%;
		width: 2.75rem;
		height: 2.75rem;

		--icon-color-1: var(--primary-hov);
		--icon-color-2: var(--warn-foreground);

		transform: rotate(-2deg);
	}

	.icon-6 {
		top: 15%;
		right: 9%;

		--icon-color-2: #0077b6;
		--icon-color-1: #48cae4;

		transform: rotate(10deg);
	}
</style>
