<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';
	import ProfileSetupProcess from './_c_page/ProfileSetupProcess.svelte';
	import SetupSavedMessage from './_c_page/SetupSavedMessage.svelte';

	let { data }: PageProps = $props();
	let setupState: 'process' | 'complete' = $state('process');
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load profile setup page" />
{:else}
	<div class="page-content-container">
		{#if setupState === 'process'}
			<ProfileSetupProcess
				uniqueName={data.data.uniqueName}
				initialLangs={data.data.preferredLanguages}
				initialTags={data.data.favoriteTags}
				initialProfilePic={data.data.profilePicKey}
				initialDisplayName={data.data.displayName}
				maxDisplayNameLength={data.data.maxDisplayNameLength}
				maxTagLength={data.data.maxTagLength}
				changeStateToSaved={() => (setupState = 'complete')}
			/>
		{:else if setupState === 'complete'}
			<AuthView>
				{#snippet children(authState)}
					{#if authState.isAuthenticated}
						<SetupSavedMessage authenticatedUserId={authState.userId} />
					{/if}
				{/snippet}
			</AuthView>
		{:else}
			<h1>Something went wrong. Reload the page</h1>
		{/if}
	</div>
{/if}

<style>
	.page-content-container {
		width: 60rem;
		margin: 4vh auto 2rem;
	}
</style>
