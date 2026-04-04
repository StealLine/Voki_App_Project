<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import type { SequentialAnsweringGeneralVokiTakingState } from './sequential-answering-general-voki-taking-state.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	interface Props {
		vokiTakingState: SequentialAnsweringGeneralVokiTakingState;
		onResultReceived: (receivedResultId: string) => void;
	}
	let { vokiTakingState, onResultReceived }: Props = $props();
	let answeringErrs = $state<Err[]>([]);

	async function onButtonClicked() {
		if (vokiTakingState.isCurrentQuestionLast) {
			const response = await vokiTakingState.finishTakingAndReceiveResult();
			if (response.isSuccess) {
				onResultReceived(response.data.receivedResultId);
			} else if (response.errs.length > 0) {
				answeringErrs = response.errs;
			} else {
				answeringErrs = [{ message: 'Something went wrong' }];
			}
		} else {
			const responseErrs = await vokiTakingState.goToNextQuestion();
			answeringErrs = responseErrs;
		}
	}
</script>

<div class="errs">
	<DefaultErrBlock errList={answeringErrs} />
</div>

<div class="navigation-container">
	<button
		class="next-btn"
		class:loading={vokiTakingState.isLoadingNextQuestion}
		onclick={() => onButtonClicked()}
	>
		{#if vokiTakingState.isLoadingNextQuestion}
			<LinesLoader sizeRem={1.3} strokePx={2} color="var(--primary-foreground)" />Loading
		{:else if vokiTakingState.isCurrentQuestionLast}
			Finish
		{:else}
			Next Question
		{/if}
	</button>
	{#if !vokiTakingState.isCurrentQuestionFirst}
		<button
			class="finish-later-btn"
			onclick={() => toast.error('This feature is not implemented yet')}
			>Finish Voki taking later</button
		>
	{/if}
</div>

<style>
	.errs {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
		margin: 1rem 2rem;
	}

	.next-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 14rem;
		padding: 0.5rem 1.5rem;
		margin: 0 auto;
		margin-top: 1rem;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
		letter-spacing: 1px;
		box-shadow: var(--shadow-md);
		cursor: pointer;
	}

	.next-btn:not(.loading):hover {
		background-color: var(--primary-hov);
	}

	.next-btn.loading {
		font-weight: 450;
		opacity: 0.85;
		pointer-events: none;
	}
</style>
