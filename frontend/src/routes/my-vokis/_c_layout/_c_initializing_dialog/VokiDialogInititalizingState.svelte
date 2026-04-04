<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';
	import InitializingStateCheckFailed from './_c_initializing_state/InitializingStateCheckFailed.svelte';
	import InitializingStateProgressCircles from './_c_initializing_state/InitializingStateProgressCircles.svelte';
	import InitializingStateSuccessMsg from './_c_initializing_state/InitializingStateSuccessMsg.svelte';
	import { fade } from 'svelte/transition';

	interface Props {
		state:
			| { name: 'initLoading' }
			| { name: 'existsCheckLoading' }
			| { name: 'existsCheckFailed'; vokiId: string }
			| { name: 'success'; vokiId: string; vokiType: VokiType; vokiName: string };
	}
	let { state }: Props = $props();
</script>

<div class="init-container">
	<InitializingStateProgressCircles state={state.name} />
	{#key state.name}
		<div class="main-content" in:fade={{ duration: 300, delay: 100 }} out:fade={{ duration: 100 }}>
			{#if state.name === 'initLoading'}
				<p class="loading-text">Initializing new voki</p>
			{:else if state.name === 'existsCheckLoading'}
				<p class="loading-text">Verifying voki creation...</p>
			{:else if state.name === 'existsCheckFailed'}
				<InitializingStateCheckFailed vokiId={state.vokiId} />
			{:else if state.name === 'success'}
				<InitializingStateSuccessMsg
					id={state.vokiId}
					type={state.vokiType}
					name={state.vokiName}
				/>
			{/if}
		</div>
	{/key}
</div>

<style>
	.init-container {
		display: grid;
		grid-template-rows: auto 1fr;
		width: 100%;
		max-width: 50rem;
		height: 100%;
		gap: 2rem;
	}

	.main-content {
		width: 100%;
		height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		margin-top: auto;
	}

	.loading-text {
		margin: 0;
		color: var(--muted-foreground);
		font-size: 1.5rem;
		font-weight: 500;
		text-align: center;
		letter-spacing: -0.01em;
		animation: pulse 2s infinite ease-in-out;
	}

	@keyframes pulse {
		0%,
		100% {
			opacity: 0.6;
		}
		50% {
			opacity: 1;
		}
	}
</style>
