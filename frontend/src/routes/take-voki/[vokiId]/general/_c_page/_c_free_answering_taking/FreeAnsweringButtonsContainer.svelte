<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import type { Err } from '$lib/ts/err';
	import type { FreeAnsweringGeneralVokiTakingState } from './free-answering-general-voki-taking-state.svelte';
	interface Props {
		vokiTakingState: FreeAnsweringGeneralVokiTakingState;
		vokiTakingErrs: (Err & { questionOrder?: number })[];
		onResultReceived: (receivedResultId: string) => void;
	}
	let { vokiTakingState, vokiTakingErrs = $bindable(), onResultReceived }: Props = $props();

	let isNextBtnInactive = $derived(vokiTakingState.isCurrentQuestionLast);
	let isPrevBtnInactive = $derived(vokiTakingState.isCurrentQuestionFirst);

	let showFinishBtn = $derived(vokiTakingState.isCurrentQuestionLast);
	let isFinishBtnLoading = $state(false);
	async function finishBtnPressed() {
		isFinishBtnLoading = true;
		const response = await vokiTakingState.finishTakingAndReceiveResult();
		isFinishBtnLoading = false;
		if (response.isSuccess) {
			onResultReceived(response.data.receivedResultId);
		} else if (response.errs.length > 0) {
			vokiTakingErrs = response.errs;
		} else {
			vokiTakingErrs = [{ message: 'Something went wrong' }];
		}
	}
	function onNextButtonClicked() {
		if (vokiTakingErrs.length > 0) {
			vokiTakingErrs = [];
		}
		vokiTakingState.goToNextQuestion();
	}
	function onPrevButtonClicked() {
		if (vokiTakingErrs.length > 0) {
			vokiTakingErrs = [];
		}
		vokiTakingState.goToPreviousQuestion();
	}
</script>

<div class="btns-container unselectable">
	<button
		class="next-prev-btns"
		class:reduced={showFinishBtn}
		class:inactive={isPrevBtnInactive}
		onclick={onPrevButtonClicked}>Previous</button
	>
	<button
		class="finish-btn"
		class:hidden={!showFinishBtn}
		disabled={!showFinishBtn}
		class:loading={isFinishBtnLoading}
		onclick={() => finishBtnPressed()}
		>{#if isFinishBtnLoading}
			<LinesLoader sizeRem={1.75} strokePx={2.5} color="var(--primary-foreground)" />
		{:else}
			Save
		{/if}</button
	>
	<button
		class="next-prev-btns"
		class:reduced={showFinishBtn}
		class:inactive={isNextBtnInactive}
		onclick={onNextButtonClicked}>Next</button
	>
</div>

<style>
	.btns-container {
		display: grid;
		justify-content: center;
		align-items: center;
		padding: 0 5rem;
		grid-template-columns: auto 1fr auto;
	}

	.next-prev-btns {
		width: 9rem;
		height: 2.25rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.75px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
	}

	.next-prev-btns:hover {
		background-color: var(--primary-hov);
	}

	.next-prev-btns.inactive {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		cursor: not-allowed;
	}

	.next-prev-btns.reduced {
		border-radius: 0.25rem;
		transform: scale(0.85);
	}

	.finish-btn {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 12rem;
		height: 2.5rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		letter-spacing: 1px;
		transition: all 0.12s ease-in;
		transform: scale(1);
		cursor: pointer;
		justify-self: center;
	}

	.finish-btn.loading {
		opacity: 0.8;
		pointer-events: none;
	}

	.finish-btn.hidden {
		opacity: 0;
		transform: scale(0);
	}
</style>
