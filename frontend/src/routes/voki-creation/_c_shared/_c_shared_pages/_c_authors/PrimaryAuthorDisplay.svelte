<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import { getErrsViewDialogOpenFunction } from '../../../../_c_layout/_ts_layout_contexts/errs-view-dialog-context';

	interface Props {
		viewerId: string;
		primaryAuthorId: string;
		creationDate: Date;
	}
	let { viewerId, primaryAuthorId, creationDate }: Props = $props();
	let primaryAuthor = UsersStore.Get('1');
	let viewerIsPrimaryAuthor = $derived(viewerId === primaryAuthorId);

	const openErrsViewDialog = getErrsViewDialogOpenFunction();
</script>

<div
	class="primary-author-container"
	class:viewer-is-primary-author={primaryAuthor.state === 'ok' && viewerIsPrimaryAuthor}
	class:loading={primaryAuthor.state === 'loading'}
	class:err={primaryAuthor.state === 'errs'}
	onclick={() => {
		if (primaryAuthor.state === 'errs') {
			openErrsViewDialog(primaryAuthor.errs);
		}
	}}
>
	{#if primaryAuthor.state === 'ok'}
		<label class="prim-author-label">
			{#if viewerIsPrimaryAuthor}
				You are the primary author
			{:else}
				Voki primary author
			{/if}
		</label>
	{/if}
	{#if primaryAuthor.state === 'ok'}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(primaryAuthor.data.profilePic)}
			alt={`Profile picture of ${primaryAuthor.data.displayName}`}
			loading="lazy"
			decoding="async"
		/>
		<div class="main-content">
			<label class="display-name">{primaryAuthor.data.displayName}</label>
			<a href="/authors/{primaryAuthorId}" class="unique-name">@{primaryAuthor.data.uniqueName}</a>
			<label class="created-voki-date"
				>Created voki on {DateUtils.toLocaleDateOnly(creationDate)}</label
			>
		</div>
	{:else if primaryAuthor.state === 'loading'}
		<div class="profile-pic"></div>
		<div class="main-content"></div>
	{:else}
		<div class="profile-pic"></div>
		<div class="main-content">
			Something went wrong. Could not load user data <svg> <use href="#common-info-icon" /></svg>
		</div>
	{/if}
</div>

<style>
	.primary-author-container {
		position: relative;
		display: grid;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		height: 8rem;
		padding: 0.75rem 1rem;
		border: 0.125rem solid var(--back);
		border-radius: 1.25rem;
		background: var(--back);
		box-shadow: var(--shadow-xs), var(--shadow-md);
		grid-template-columns: auto 1fr;
	}

	.primary-author-container.viewer-is-primary-author {
		border-color: var(--primary);
		box-shadow: none;
	}

	.prim-author-label {
		position: absolute;
		top: 0%;
		left: 2rem;
		padding: 0 0.25rem;
		border-radius: 10rem;
		background: var(--back);
		color: var(--text);
		font-size: 1rem;
		font-weight: 450;
		transform: translateY(calc(-50% - 0.125rem));
	}

	.viewer-is-primary-author > .prim-author-label {
		color: var(--primary);
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

	.display-name {
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 600;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

	.unique-name {
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 425;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.unique-name:hover {
		color: var(--primary);
	}

	.created-voki-date {
		margin-top: calc(0.125rem + 0.5vh);
		color: var(--secondary-foreground);
		font-size: 1rem;
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

	.primary-author-container.err {
		border: 0.125rem solid var(--red-5);
		background-color: var(--secondary);
		box-shadow: var(--err-shadow);
		cursor: pointer;
	}

	.err .main-content {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.125rem;
		padding: 0.5rem 0.75rem;
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 450;
	}

	.err:hover .main-content {
		text-decoration: underline;
	}

	.err .main-content > svg {
		width: 1.375rem;
		height: 1.375rem;
		padding-top: 0.125rem;
		stroke-width: 2;
	}
</style>
