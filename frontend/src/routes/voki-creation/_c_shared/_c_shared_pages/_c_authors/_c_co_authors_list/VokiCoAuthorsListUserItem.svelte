<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import type { Snippet } from 'svelte';

	interface Props {
		userId: string;
		userCoAuthorState: 'viewerIsAuthor' | 'invitedUser' | 'coAuthor';
		actionButton: Snippet<[userId: string]> | null;
	}
	let { userId, userCoAuthorState, actionButton }: Props = $props();
	let user = UsersStore.Get(userId);
</script>

<div
	class="user-item"
	class:loading={user.state === 'loading'}
	class:err={user.state === 'errs'}
	class:viewer-is-user={userCoAuthorState === 'viewerIsAuthor'}
	class:invited={userCoAuthorState === 'invitedUser'}
>
	{#if user.state === 'ok'}
		<label class="badge-label">
			{#if userCoAuthorState === 'viewerIsAuthor'}
				You
			{:else if userCoAuthorState === 'invitedUser'}
				Invited for co-author
			{:else if userCoAuthorState === 'coAuthor'}
				Co-author
			{/if}
		</label>
	{/if}
	{#if user.state === 'ok'}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(user.data.profilePic)}
			alt={`Profile picture of ${user.data.displayName}`}
			decoding="async"
		/>
		<div class="main-content">
			<div class="names-container">
				<label class="display-name">{user.data.displayName}</label>
				<a href="/authors/{userId}" class="unique-name">@{user.data.uniqueName}</a>
			</div>
		</div>
		{#if actionButton}
			{@render actionButton(userId)}
		{/if}
	{:else if user.state === 'loading'}
		<div class="profile-pic"></div>
		<div class="main-content"></div>
	{:else}
		<div class="profile-pic"></div>
		<div class="main-content">
			{#if user.state === 'errs' && user.errs.length > 0}
				{#each user.errs as err}
					<p class="err-view">{err}</p>
				{/each}
			{:else}
				<p class="err-view">Something went wrong. Could not load user data</p>
			{/if}
		</div>
	{/if}
</div>

<style>
	.user-item {
		position: relative;
		display: grid;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		height: 6.25rem;
		padding: 0.675rem 1rem 0.375rem;
		border: 0.125rem solid var(--back);
		border-radius: 1.25rem;
		background: var(--back);
		box-shadow: var(--shadow-xs);
		animation: var(--default-fade-in);
		grid-template-columns: auto 1fr auto;
	}

	.user-item.viewer-is-user {
		border-color: var(--primary);
		box-shadow: none;
	}

	.user-item.invited {
		border-color: var(--accent-foreground);
		box-shadow: none;
	}

	.badge-label {
		position: absolute;
		top: 0%;
		left: 2rem;
		padding: 0 0.25rem;
		border-radius: 10rem;
		background-color: var(--back);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		transform: translateY(calc(-50% - 0.125rem));
	}

	.user-item.viewer-is-user .badge-label {
		color: var(--primary);
	}

	.user-item.invited .badge-label {
		color: var(--accent-foreground);
	}

	.profile-pic {
		aspect-ratio: 1;
		height: 100%;
		border-radius: 100vw;
		overflow: hidden;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
	}

	.main-content {
		display: grid;
		align-content: center;
	}

	.loading {
		pointer-events: none;
		opacity: 0.95;
		overflow: hidden;
	}

	.loading > * {
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.loading > .main-content {
		width: 90%;
		height: 80%;
		margin-left: 0.5rem;
		border-radius: 1.5rem;
	}

	@keyframes shimmer {
		0% {
			transform: translateX(-70%);
		}

		100% {
			transform: translateX(70%);
		}
	}

	.loading::after {
		position: absolute;
		background: linear-gradient(
			-40deg,
			transparent 0%,
			transparent 25%,
			color-mix(in srgb, var(--secondary-foreground) 10%, var(--secondary) 15%) 50%,
			transparent 75%,
			transparent 100%
		);
		transform: translateX(-90%);
		animation: shimmer 1.3s infinite;
		content: '';
		inset: 0;
	}

	.user-item.err {
		border: 0.125rem solid var(--red-5);
		background-color: var(--secondary);
		box-shadow: var(--err-shadow);
	}

	.err-view {
		padding: 0.5rem 0.75rem;
		color: var(--muted-foreground);
	}

	.names-container {
		display: grid;
		justify-content: start;
		gap: 0.125rem;
		width: 100%;
		padding-bottom: 1rem;
		grid-template-rows: auto auto;
	}

	.display-name {
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

	.unique-name {
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 425;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.unique-name:hover {
		color: var(--primary);
	}
</style>
