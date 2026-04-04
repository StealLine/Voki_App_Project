<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';
	import AuthView from '$lib/components/AuthView.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import AutoAlbumsSection from './_c_page/AutoAlbumsSection.svelte';
	import UserAlbumsSection from './_c_page/UserAlbumsSection.svelte';

	let { data }: PageProps = $props();
	let userAlbums = $state(data?.data?.albums ?? []);
</script>

<AuthView>
	{#snippet children(authState)}
		{#if authState.name === 'loading'}
			<div class="loading-container">Loading...</div>
		{:else if authState.isAuthenticated}
			{#if !data.isSuccess}
				<PageLoadErrView errs={data.errs} defaultMessage="Could not load your albums" />
			{:else}
				<AutoAlbumsSection initialAutoAlbumsAppearance={data.data.autoAlbumsAppearance} />
				<UserAlbumsSection albums={userAlbums} />
			{/if}
		{:else}
			<div class="login-required-container">
				<h1>To create, view and manage your albums you need to be logged in</h1>
				{#if authState.name === 'error'}
					<DefaultErrBlock errList={authState.errs} />
				{/if}
			</div>
		{/if}
	{/snippet}
</AuthView>

<style>
	.login-required-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		width: fit-content;
		margin: 4rem auto 1rem;
	}

	.loading-container {
		animation: fade-in-from-zero-with-delay 1s ease-in !important;
	}
</style>
