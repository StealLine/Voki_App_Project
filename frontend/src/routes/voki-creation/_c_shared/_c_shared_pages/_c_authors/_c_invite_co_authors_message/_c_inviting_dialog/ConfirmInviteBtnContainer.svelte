<script lang="ts">
	import type { UserPreviewWithInvitesSettings } from '../../types';

	interface Props {
		usersChosenToInvite: UserPreviewWithInvitesSettings[];
		isLoading: boolean;
		onInviteButtonClick: () => void;
	}
	let { usersChosenToInvite, isLoading, onInviteButtonClick }: Props = $props();
</script>

<div class="confirm-btn-container">
	{#if usersChosenToInvite.length === 0}
		<label class="no-users-selected-label">Choose users to invite</label>
	{:else}
		<button onclick={() => onInviteButtonClick()} class:loading={isLoading}>
			{#if isLoading}
				Loading...
			{:else}
				Invite {usersChosenToInvite.length} users for co-author
			{/if}
		</button>
		<label class="users-list-label">
			{#if isLoading}
				Inviting {usersChosenToInvite.length} users for co-author ...{:else}
				{Array.from(usersChosenToInvite)
					.map((u) => `@${u.uniqueName}`)
					.join(', ')} will be invited
			{/if}
		</label>
	{/if}
</div>

<style>
	.confirm-btn-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.375rem;
		height: 4rem;
		text-align: center;
	}

	.no-users-selected-label {
		margin-top: auto;
		color: var(--secondary-foreground);
	}

	.confirm-btn-container button {
		width: 18rem;
		padding: 0.375rem 1.25rem;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1rem;
		font-weight: 500;
		box-shadow: var(--shadow-xs);
		transition: background-color 0.12s ease-in;
		cursor: pointer;
		font-variant-numeric: tabular-nums;
	}

	.confirm-btn-container button:hover:not(.loading) {
		background-color: var(--primary-hov);
	}

	button.loading {
		opacity: 0.8;
		pointer-events: none;
	}

	.confirm-btn-container .users-list-label {
		max-width: 100%;
		color: var(--muted-foreground);
		font-size: 0.875rem;
		line-height: 1;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

	.confirm-btn-container > * {
		animation: fade-in-from-zero 0.2s ease-in;
	}
</style>
