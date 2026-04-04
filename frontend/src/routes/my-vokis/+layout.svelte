<script lang="ts">
	import MyVokisLink from './_c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './_c_layout/VokiInitializingDialog.svelte';
	import { type Snippet } from 'svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import MyVokiAuthNeeded from './_c_layout/MyVokiAuthNeeded.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { toast } from 'svelte-sonner';
	import { setCurrentPage, type MyVokiPageApi } from './my-vokis-page-context';
	import type { LayoutData } from './$types';

	let { data, children }: { data: LayoutData; children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;

	let currentPage = $state<MyVokiPageApi>();
	setCurrentPage((curPage) => {
		currentPage = curPage;
	});

	async function handleRefreshClick() {
		if (!currentPage) {
			toast.error('Something went wrong. Could not refresh the page');
			return;
		}
		await currentPage.forceRefetch();
	}
</script>

{#snippet authStateChildren(authState: AuthStore.AuthState)}
	{#if !authState.isAuthenticated}
		<MyVokiAuthNeeded currentAuthState={authState} />
	{:else}
		<div class="my-vokis-page">
			<div class="nav-header">
				<div class="tabs-links">
					<MyVokisLink text="Draft Vokis" tab="draft-vokis" currentTab={data.currentTab} />
					<MyVokisLink text="Published Vokis" tab="published-vokis" currentTab={data.currentTab} />
					<MyVokisLink text="Vokis To Manage" tab="vokis-to-manage" currentTab={data.currentTab} />
				</div>
				<div class="invites-link-container">
					{#if currentPage?.invitesPage.exists}
						<a
							href="/my-vokis/{data.currentTab}/{currentPage.invitesPage.path}"
							class="invites-link"
						>
							invites
						</a>
					{:else}
						<div></div>
					{/if}
				</div>
			</div>
			<div class="my-vokis-page-content">
				{@render children()}
			</div>

			<PrimaryButton onclick={() => vokiInitializingDialog.open()} class="create-new-voki-btn"
				>Create new voki
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
					<path
						d="M21.5 12C21.5 7.52166 21.5 5.28249 20.1088 3.89124C18.7175 2.5 16.4783 2.5 12 2.5C7.52166 2.5 5.28249 2.5 3.89124 3.89124C2.5 5.28249 2.5 7.52166 2.5 12C2.5 16.4783 2.5 18.7175 3.89124 20.1088C5.28249 21.5 7.52166 21.5 12 21.5C16.4783 21.5 18.7175 21.5 20.1088 20.1088C21.5 18.7175 21.5 16.4783 21.5 12Z"
						stroke="currentColor"
						stroke-width="2"
						stroke-linejoin="round"
					></path>
					<path
						d="M10 8L13.3322 11.0203C14.2226 11.8273 14.2226 12.1727 13.3322 12.9797L10 16"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
					></path>
				</svg>
			</PrimaryButton>
			<VokiInitializingDialog bind:this={vokiInitializingDialog} />
		</div>
	{/if}
{/snippet}

<AuthView children={authStateChildren} />

<style>
	.my-vokis-page {
		position: relative;
		display: grid;
		height: 100vh;
		padding-bottom: 2rem;
		grid-template-rows: auto 1fr;
		animation: loading-fade-in 0.1s ease-in-out;
	}

	.nav-header {
		display: grid;
		grid-template-rows: auto 1.5rem;
		gap: 0;
	}

	.tabs-links {
		display: grid;
		justify-content: center;
		align-items: center;
		gap: 1.25rem;
		width: fit-content;
		box-sizing: border-box;
		padding: 1.25rem 0 0.75rem;
		margin: 0 auto;
		background-color: var(--back);
		grid-template-columns: 1fr 1fr 1fr;
	}

	.invites-link-container {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 100%;
	}

	.invites-link {
		display: flex;
		justify-content: center;
		align-items: center;
		width: fit-content;
		height: 100%;
		padding: 0.25rem 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-weight: 425;
	}

	.my-vokis-page-content {
		overflow-y: auto;
		z-index: 1;
		padding-bottom: 2rem;
	}

	:global(.my-vokis-page > .create-new-voki-btn) {
		position: absolute;
		bottom: 1rem;
		left: 50%;
		z-index: 2;
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 2rem;
		border-radius: 0.375rem;
		box-shadow: var(--shadow-xl);
		transform: translateX(-50%);
		outline: none;
	}

	:global(.my-vokis-page > .create-new-voki-btn:active) {
		transform: translateX(-50%) scaleX(0.96);
	}

	:global(.my-vokis-page > .create-new-voki-btn > svg) {
		height: 1.75rem;
	}

	@keyframes loading-fade-in {
		0% {
			opacity: 0;
		}

		100% {
			opacity: 1;
		}
	}
</style>
