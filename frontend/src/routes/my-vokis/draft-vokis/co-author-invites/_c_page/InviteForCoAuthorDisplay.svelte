<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import { VokiTypeUtils } from '$lib/ts/voki-type';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';
	import InviteDisplayInviter from './_c_invite_display/InviteDisplayInviter.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		onAccept: (inviteId: string) => void;
		onDecline: (inviteId: string) => void;
	}

	let { invite, onAccept, onDecline }: Props = $props();

	const invitedCount = invite.invitedForCoAuthorUserIds?.length ?? 0;
	function handleAccept() {
		onAccept(invite.vokiId);
	}
	function handleDecline() {
		onDecline(invite.vokiId);
	}
</script>

<div class="invite-item">
	<div class="left">
		<div class="main">
			<InviteDisplayInviter userId={invite.primaryAuthorId} />

			<p class="invite-text">
				invited you to participate as co-author of "<span class="voki-name">{invite.vokiName}</span
				>" Voki
			</p>

			<div class="meta" aria-label="Voki meta">
				<span class="badge type">
					<svg class="type-icon"><use href={VokiTypeUtils.icon(invite.vokiType)} /></svg>
					{VokiTypeUtils.name(invite.vokiType)}
				</span>
				<span class="count" title="Current co-authors">
					Co-authors count: {invite.coAuthorIds.length}
				</span>

				{#if invite.invitedForCoAuthorUserIds.length > 0}
					<span class="count" title="Pending invitations">
						Invited for co-author: {invite.invitedForCoAuthorUserIds.length}
					</span>
				{/if}

				<span class="date">
					{DateUtils.toLocale(invite.creationDate)}
				</span>
			</div>
		</div>

		<div class="actions">
			<button class="btn cancel" onclick={handleDecline}>Decline</button>
			<button class="btn accept" onclick={handleAccept}>Accept</button>
		</div>
	</div>

	<div class="cover-wrap">
		<img
			class="cover"
			src={StorageBucketMain.fileSrc(invite.vokiCover)}
			alt={`Cover of ${invite.vokiName}`}
		/>
	</div>
</div>

<style>
	.invite-item {
		display: grid;
		align-items: stretch;
		gap: 1.5rem;
		padding: 1rem 1.5rem;
		border-radius: 1rem;
		background: var(--back);
		box-shadow: var(--shadow-xs) inset;
		grid-template-columns: 1fr 17rem;
	}

	.left {
		display: grid;
		grid-template-rows: 1fr auto;
		align-items: start;
		gap: 1rem;
	}

	.main {
		display: grid;
		gap: 0.5rem;
	}

	.invite-text {
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 500;
	}

	.invite-text .voki-name {
		color: var(--text);
		font-weight: 600;
	}

	.meta {
		display: inline-flex;
		flex-wrap: wrap;
		align-items: center;
		gap: 0.5rem;
		color: var(--muted-foreground);
		font-size: 0.9rem;
	}

	.badge.type {
		display: inline-flex;
		align-items: center;
		gap: 0.25rem;
		padding: 0.125rem 0.5rem;
		border-radius: 100vw;
		background: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
	}

	.type-icon {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}

	.count {
		font-weight: 500;
	}

	.cover-wrap {
		width: 100%;
		height: 100%;
	}

	.cover {
		width: 100%;
		object-fit: fill;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}

	.actions {
		display: flex;
		justify-content: flex-end;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
	}

	.btn {
		padding: 0.5rem 2rem;
		border: none;
		border-radius: 0.25rem;
		font-weight: 500;
		letter-spacing: 0.25px;
		cursor: pointer;
	}

	.btn.accept {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.accept:hover:where(:not([disabled])) {
		background-color: var(--primary-hov);
	}

	.btn.cancel {
		border-color: var(--muted);
		color: var(--muted-foreground);
	}

	.btn.cancel:hover:where(:not([disabled])) {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
