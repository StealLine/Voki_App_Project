<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';

	interface Props {
		currentAuthState: Exclude<AuthStore.AuthState, { isAuthenticated: true }>;
	}
	let { currentAuthState }: Props = $props();
</script>

<div>
	<h1>To create and manage Vokis you need to sign in</h1>
	{#if currentAuthState.name === 'unauthenticated'}
		<div>Login and sign up buttons</div>
	{:else if currentAuthState.name === 'loading'}
		<div>Loading...</div>
	{:else if currentAuthState.name === 'error'}
		<div>An error occurred</div>
		<DefaultErrBlock errList={currentAuthState.errs} />
	{:else}
		<div>Unexpected authentication state</div>
	{/if}
</div>
