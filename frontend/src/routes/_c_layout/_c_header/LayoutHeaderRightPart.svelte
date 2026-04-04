<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { getSignInDialogOpenFunction } from '../_ts_layout_contexts/sign-in-dialog-context';
	import LayoutHeaderRightPartAuthenticated from './_c_right/LayoutHeaderRightPartAuthenticated.svelte';
	type ViewState =
		| { name: 'authenticated'; userId: string }
		| { name: 'loading' }
		| { name: 'error'; errs: Err[] }
		| { name: 'unauthenticated' };
	const openSignInDialog = getSignInDialogOpenFunction();
	let authState = AuthStore.Get();
	let viewState: ViewState = $state(authState);
	const LOADING_DELAY_MS = 500;

	$effect(() => {
		//don't show loading state immediately so the component doesn't flash
		if (authState.name === 'loading') {
			const timer = setTimeout(() => {
				viewState = { name: 'loading' };
			}, LOADING_DELAY_MS);

			return () => {
				clearTimeout(timer);
			};
		}

		viewState = authState;
	});
	function onErrorStateReloadBtnClick() {
		if (viewState.name !== 'error') {
			return;
		}
		viewState = { name: 'loading' };
		AuthStore.GetWithForceRefresh();
	}
</script>

{#if viewState.name === 'authenticated'}
	<LayoutHeaderRightPartAuthenticated currentUserId={viewState.userId} />
{:else if viewState.name === 'loading'}
	<div class="loading">Loading...</div>
{:else if viewState.name === 'error'}
	<div
		class="loading-error unselectable"
		title="Click to reload"
		onclick={onErrorStateReloadBtnClick}
	>
		Authentication loading error
	</div>
{:else}
	<div class="sign-in-btn-container unselectable">
		<button class="sign-in-btn" onclick={() => openSignInDialog(null)}>Sign in</button>
	</div>
{/if}

<style>
	.loading-error {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
		padding: 0 0.25rem;
		border-radius: 100vw;
		background-color: var(--red-1);
		color: var(--red-3);
		font-size: 1rem;
		font-weight: 450;
		line-height: 1.1;
		text-align: center;
		letter-spacing: 0.2px;
		transition: all 0.06s ease-in;
		cursor: pointer;
	}

	.loading-error:hover {
		background-color: var(--red-2);
		color: var(--red-4);
	}

	.sign-in-btn-container {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
	}

	.sign-in-btn {
		padding: 0.25rem 1.75rem;
		border: none;
		border-radius: var(--radius);
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 450;
		letter-spacing: 1px;
		box-shadow: var(--shadow);
		transition:
			background-color 0.06s ease-in,
			padding 0.08s ease-in;
		animation: fade-in-from-zero 0.12s ease-in;
		cursor: pointer;
		outline: none;
	}

	.sign-in-btn:hover {
		background-color: var(--primary-hov);
	}

	.sign-in-btn:active {
		padding: 0.25rem 1.675rem;
	}

	.loading {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 100%;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		animation: loading-fade-with-delay 1.5s ease-in;
	}

	@keyframes loading-fade-with-delay {
		0%,
		60% {
			opacity: 0;
		}

		100% {
			opacity: 1;
		}
	}
</style>
