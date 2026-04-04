<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import type { ExistingUnfinishedSessionForVokiData } from '$lib/ts/voki-taking-session';
	import { goto } from '$app/navigation';
	import { VokiTakingSessionMarkerCookie } from '$lib/ts/cookies/voki-taking-session-marker';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	interface Props {
		sessionData: ExistingUnfinishedSessionForVokiData;
		takeVokiPageLink: string;
		replaceStateOnGoto: boolean;
	}
	let { sessionData, takeVokiPageLink, replaceStateOnGoto }: Props = $props();

	function onContinueBtnClick() {
		VokiTakingSessionMarkerCookie.markContinueFor2Min(sessionData.vokiId, sessionData.sessionId);
		goto(`${takeVokiPageLink}?continueExistingUnfinishedSession=true`, {
			replaceState: replaceStateOnGoto
		});
	}
	function onTerminateBtnClick() {
		VokiTakingSessionMarkerCookie.markTerminateFor2Min(sessionData.vokiId, sessionData.sessionId);
		goto(`${takeVokiPageLink}?terminateExistingUnfinishedSession=true`, {
			replaceState: replaceStateOnGoto
		});
	}
</script>

<div class="unfinished-session-container">
	<h1>Resume Session</h1>

	<div class="session-details">
		<div class="detail-item">
			<span class="label">Started</span>
			<span class="value" use:relativeTime={{ date: sessionData.startedAt }}></span>
		</div>
		<div class="separator"></div>
		<div class="detail-item">
			<span class="label">Progress</span>
			<span class="value"
				>{sessionData.questionsWithSavedAnswersCount} / {sessionData.totalQuestionsCount}</span
			>
		</div>
	</div>

	<p class="description">
		You have an unfinished session. Would you like to continue where you left off or start fresh?
	</p>

	<div class="actions">
		<PrimaryButton onclick={onContinueBtnClick} class="continue-btn">Continue Session</PrimaryButton
		>
		<button class="terminate-btn" onclick={onTerminateBtnClick}> Start New Session </button>
	</div>
</div>

<style>
	.unfinished-session-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		max-width: 32rem;
	}

	h1 {
		margin: 0;
		color: var(--text);
		font-size: 1.75rem;
		font-weight: 600;
	}

	.session-details {
		display: grid;
		justify-content: center;
		align-items: center;
		gap: 1.5rem;
		width: 100%;
		padding: 1rem;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		grid-template-columns: 1fr auto 1fr;
	}

	.detail-item {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.25rem;
	}

	.separator {
		width: 1px;
		height: 2rem;
		background-color: var(--muted-foreground);
		opacity: 0.2;
	}

	.label {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		text-transform: uppercase;
		letter-spacing: 0.05em;
	}

	.value {
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 600;
	}

	.description {
		margin: 0.5rem 0;
		color: var(--secondary-foreground);
		font-size: 1rem;
		line-height: 1.25;
		text-align: center;
	}

	.actions {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		width: 100%;
		margin-top: 1.5rem;
	}

	:global(.continue-btn) {
		justify-content: center;
		width: 100% !important;
	}

	.terminate-btn {
		width: 100%;
		padding: 0.75rem;
		border: none;
		border-radius: 0.5rem;
		background: none;
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 500;
		transition: all 0.2s ease;
		cursor: pointer;
	}

	.terminate-btn:hover {
		background-color: var(--red-1);
		color: var(--red-5);
	}
</style>
