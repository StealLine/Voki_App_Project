<script lang="ts">
	import { ApiSubscriptions } from '$lib/ts/backend-communication/backend-services';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { watch } from 'runed';
	import AuthorProfileContextMenuCurrentUser from './_c_actions/AuthorProfileContextMenuCurrentUser.svelte';
	import AuthorProfileContextMenuOtherUser from './_c_actions/AuthorProfileContextMenuOtherUser.svelte';
	import AuthorProfileContextMenuGuest from './_c_actions/AuthorProfileContextMenuGuest.svelte';
	import { toast } from 'svelte-sonner';

	interface Props {
		profileId: string;
	}

	let { profileId }: Props = $props();
	let actionsState: 'current-user' | 'loading' | 'subscribed' | 'not-subscribed' | 'error' =
		$state('loading');
	let authState = $state(AuthStore.Get());
	let currentUserMenuRef = $state<AuthorProfileContextMenuCurrentUser>()!;
	let otherUserMenuRef = $state<AuthorProfileContextMenuOtherUser>()!;
	let guestMenuRef = $state<AuthorProfileContextMenuGuest>()!;

	watch([() => authState.name], () => updateActionsState());

	function updateActionsState() {
		if (authState.name === 'loading') {
			actionsState = 'loading';
			return;
		}
		if (authState.name === 'error') {
			actionsState = 'error';
			return;
		}
		if (authState.name === 'authenticated' && authState.userId === profileId) {
			actionsState = 'current-user';
			return;
		}

		ApiSubscriptions.fetchJsonResponse<{ isFollowing: boolean }>(
			`/is-subscribed?userId=${profileId}`,
			{ method: 'GET' }
		).then((response) => {
			if (response.isSuccess) {
				actionsState = response.data.isFollowing ? 'subscribed' : 'not-subscribed';
			} else {
				actionsState = 'error';
			}
		});
	}

	function handleInvite() {
		toast.info('Co-Author invitation feature is coming soon!');
	}
</script>

<div class="actions-wrapper" class:loading={actionsState === 'loading'}>
	{#if actionsState === 'error'}
		<div class="action-item error-state">
			<span class="error-text">Loading failed</span>
			<button class="icon-btn refresh" onclick={() => updateActionsState()} aria-label="Retry">
				<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"
					><path d="M21 12a9 9 0 11-9-9c2.52 0 4.85.83 6.72 2.24M21 3v5h-5" /></svg
				>
			</button>
		</div>
	{:else if actionsState === 'loading'}
		<div class="skeleton main-skeleton"></div>
		<div class="skeleton secondary-skeleton"></div>
		<div class="skeleton icon-skeleton"></div>
	{:else}
		<div class="actions-group">
			{#if actionsState === 'current-user'}
				<a href="/settings" class="btn primary edit-profile">
					<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
						><path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" /><path
							d="M18.5 2.5a2.121 2.121 0 113 3L12 15l-4 1 1-4 9.5-9.5z"
						/></svg
					>
					<span>Edit Profile</span>
				</a>
			{:else}
				<button
					class="btn follow-btn"
					class:primary={actionsState === 'not-subscribed'}
					class:subscribed={actionsState === 'subscribed'}
					onclick={() => updateActionsState()}
				>
					{#if actionsState === 'subscribed'}
						<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"
							><path d="M20 6L9 17l-5-5" /></svg
						>
						<span>Following</span>
					{:else}
						<span>Follow</span>
					{/if}
				</button>

				{#if authState.name === 'authenticated'}
					<button class="btn secondary invite-btn" onclick={handleInvite}>
						<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
							><path d="M16 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" /><circle
								cx="8.5"
								cy="7"
								r="4"
							/><line x1="20" y1="8" x2="20" y2="14" /><line x1="23" y1="11" x2="17" y2="11" /></svg
						>
						<span>Invite</span>
					</button>
				{/if}
			{/if}

			<button
				class="icon-btn more-trigger"
				onclick={() => {
					if (actionsState === 'current-user') currentUserMenuRef?.open();
					else if (authState.name === 'authenticated') otherUserMenuRef?.open();
					else guestMenuRef?.open();
				}}
				aria-label="More options"
			>
				<div class="dots-layout">
					<span></span><span></span><span></span>
				</div>
			</button>
		</div>
	{/if}

	<AuthorProfileContextMenuCurrentUser {profileId} bind:this={currentUserMenuRef} />
	<AuthorProfileContextMenuOtherUser {profileId} bind:this={otherUserMenuRef} />
	<AuthorProfileContextMenuGuest {profileId} bind:this={guestMenuRef} />
</div>

<style>
	.actions-wrapper {
		margin-top: 1.25rem;
		height: 2.125rem;
		display: flex;
		align-items: center;
		position: relative;
	}

	.actions-group {
		display: flex;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
	}

	.btn {
		flex: 1;
		height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 0.5rem;
		border-radius: 0.5rem;
		font-size: 0.975rem;
		font-weight: 550;
		transition: all 0.25s cubic-bezier(0.23, 1, 0.32, 1);
		cursor: pointer;
		text-decoration: none;
		white-space: nowrap;
	}

	.btn svg {
		width: 1rem;
		height: 1rem;
	}

	.btn.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover {
		background: var(--primary-hov);
	}

	.btn.secondary,
	.btn.subscribed {
		background: var(--muted);
		color: var(--foreground);
	}

	.btn.secondary:hover,
	.btn.subscribed:hover {
		background: var(--accent);
	}

	.icon-btn {
		height: 100%;
		aspect-ratio: 1;
		flex-shrink: 0;
		display: flex;
		align-items: center;
		justify-content: center;
		border-radius: 0.5rem;
		background: var(--muted);
		border: none;
		cursor: pointer;
		transition: all 0.2s ease;
		color: var(--foreground);
	}

	.icon-btn:hover {
		background: var(--accent);
	}

	.dots-layout {
		display: flex;
		gap: 0.125rem;
	}

	.dots-layout span {
		width: 0.25rem;
		height: 0.25rem;
		border-radius: 50%;
		background: currentColor;
		transition: transform 0.2s ease;
	}

	.icon-btn:hover .dots-layout span:nth-child(2) {
		transform: scale(1.25);
	}

	.error-state {
		display: flex;
		align-items: center;
		background: color-mix(in srgb, var(--red-5) 8%, transparent);
		padding: 0 0.5rem 0 1rem;
		border-radius: 0.75rem;
		color: var(--red-5);
		border: 0.0625rem solid color-mix(in srgb, var(--red-5) 20%, transparent);
		width: 100%;
		justify-content: space-between;
	}

	.error-text {
		font-size: 0.8125rem;
		font-weight: 500;
	}

	.icon-btn.refresh {
		width: 1.75rem;
		height: 1.75rem;
		background: transparent;
		border: none;
	}

	.icon-btn.refresh svg {
		width: 0.875rem;
	}

	.skeleton {
		background: var(--muted);
		border-radius: 0.75rem;
		position: relative;
		overflow: hidden;
	}

	.skeleton::after {
		content: '';
		position: absolute;
		inset: 0;
		background: linear-gradient(
			90deg,
			transparent,
			color-mix(in srgb, var(--foreground) 5%, transparent),
			transparent
		);
		animation: shimmer 1.5s infinite;
	}

	.main-skeleton {
		flex: 2;
		height: 100%;
		margin-right: 0.5rem;
	}
	.secondary-skeleton {
		flex: 1;
		height: 100%;
		margin-right: 0.5rem;
	}
	.icon-skeleton {
		width: 2.25rem;
		height: 100%;
	}

	@keyframes shimmer {
		0% {
			transform: translateX(-100%);
		}
		100% {
			transform: translateX(100%);
		}
	}
</style>
