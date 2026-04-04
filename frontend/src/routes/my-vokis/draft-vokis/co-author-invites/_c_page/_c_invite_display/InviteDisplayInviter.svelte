<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { getErrsViewDialogOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/errs-view-dialog-context';

	interface Props {
		userId: string;
	}
	let { userId }: Props = $props();
	let inviter = UsersStore.Get(userId);
	const openErrsViewDialog = getErrsViewDialogOpenFunction();
</script>

<div
	class="inviter"
	class:loading={inviter.state === 'loading'}
	class:error={inviter.state === 'errs'}
	class:ok={inviter.state === 'ok'}
>
	{#if inviter.state === 'loading'}
		<div class="profile-pic" />
		<div class="names-container-loading">
			<div class="line w1" />
			<div class="line w2" />
		</div>
	{:else if inviter.state === 'errs'}
		<svg class="profile-pic">
			<use href="#common-crossed-circle-icon" />
		</svg>
		<label class="could-not-load-label" onclick={() => openErrsViewDialog(inviter.errs)}
			>Could not load user data<svg><use href="#common-info-icon" /></svg>
		</label>
	{:else}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(inviter.data.profilePic)}
			alt={inviter.data.displayName}
		/>
		<div class="names-container">
			<div class="display-name">{inviter.data.displayName}</div>
			<a class="unique-name" href="/authors/{inviter.data.id}">@{inviter.data.uniqueName}</a>
		</div>
	{/if}
</div>

<style>
	.inviter {
		display: grid;
		align-items: center;
		gap: 0.25rem;
		height: 3.5rem;
		grid-template-columns: auto 1fr;
	}

	.profile-pic {
		aspect-ratio: 1/1;
		height: 3.5rem;
		border-radius: 50%;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
	}

	.names-container {
		display: grid;
		gap: 0;
	}

	.display-name {
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
		cursor: default;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.unique-name {
		color: var(--muted-foreground);
		font-size: 1rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.unique-name:hover {
		color: var(--primary);
	}

	.loading .profile-pic {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.names-container-loading {
		display: grid;
		gap: 0.5rem;
	}

	.line {
		border-radius: 0.375rem;
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.w1 {
		width: 12rem;
		height: 1.125rem;
	}

	.w2 {
		width: 7.5rem;
		height: 1rem;
	}

	.inviter.error > .profile-pic {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.could-not-load-label {
		width: fit-content;
		padding: 0.125rem 0.5rem;
		border-radius: 0.5rem;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 475;
		cursor: pointer;
	}

	.could-not-load-label > svg {
		width: 1.375rem;
		height: 1.375rem;
		padding: 0;
		padding-bottom: 0.125rem;
		margin-left: 0.25rem;
		color: inherit;
		stroke-width: 2.25;
		vertical-align: middle;
	}

	.could-not-load-label:hover {
		background-color: var(--secondary);
	}

	.inviter.error > svg {
		padding: 0.5rem;
		color: var(--secondary-foreground);
	}
</style>
