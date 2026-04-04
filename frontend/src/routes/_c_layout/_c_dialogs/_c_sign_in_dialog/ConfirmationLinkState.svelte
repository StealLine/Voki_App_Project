<script lang="ts">
	import SignInDialogHeader from './_c_states_shared/SignInDialogHeader.svelte';

	let { email }: { email: string } = $props<{ email: string }>();
</script>

<SignInDialogHeader text="Check your inbox" />
<p class="msg">Thank you for signing up. Confirmation link has been sent to <span>{email}</span></p>
<p class="email"></p>

<style>
	.msg {
		margin: 1rem 0 0;
		font-size: 2rem;
		font-weight: 410;
		text-align: center;
	}

	span {
		color: var(--primary);
		font-size: 2rem;
	}
</style>
