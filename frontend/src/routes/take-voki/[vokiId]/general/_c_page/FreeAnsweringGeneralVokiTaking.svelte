<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { onMount, onDestroy } from 'svelte';
	import type { GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from '../types';
	import GeneralVokiTakingQuestionDisplay from './_c_takings_shared/GeneralVokiTakingQuestionDisplay.svelte';
	import FreeAnsweringVokiTakingErrsList from './_c_free_answering_taking/FreeAnsweringVokiTakingErrsList.svelte';
	import { FreeAnsweringGeneralVokiTakingState } from './_c_free_answering_taking/free-answering-general-voki-taking-state.svelte';
	import { createQuestionsKeyHandler } from './_c_free_answering_taking/free-answering-voki-taking-questions-nav';
	import FreeAnsweringButtonsContainer from './_c_free_answering_taking/FreeAnsweringButtonsContainer.svelte';

	interface Props {
		takingData: GeneralVokiTakingData;
		saveData: PosssibleGeneralVokiTakingDataSaveData;
		clearVokiSeenUpdateTimer: () => void;
		onResultReceived: (resultId: string) => void;
	}
	let { takingData, saveData, clearVokiSeenUpdateTimer, onResultReceived }: Props = $props();

	let vokiTakingState = new FreeAnsweringGeneralVokiTakingState(
		takingData,
		saveData,
		clearVokiSeenUpdateTimer
	);

	type ErrWithOrder = Err & { questionOrder?: number };
	let vokiTakingErrs: ErrWithOrder[] = $state([]);

	function jumpToSpecificQuestionFromErrsList(questionOrder: number): Err[] {
		vokiTakingErrs = [];
		return vokiTakingState.jumpToSpecificQuestion(questionOrder);
	}

	let answersContainer: { focusFirstAnswerCard: () => void } = $state()!;

	onMount(() => {
		const handler = createQuestionsKeyHandler({
			goToNextQuestion: () => vokiTakingState.goToNextQuestion(),
			goToPreviousQuestion: () => vokiTakingState.goToPreviousQuestion(),
			focusFirstAnswerCardInContainer: () => answersContainer.focusFirstAnswerCard()
		});

		window.addEventListener('keydown', handler);
		onDestroy(() => window.removeEventListener('keydown', handler));
	});
</script>

<div class="taking-container">
	{#if vokiTakingState.currentQuestion}
		<GeneralVokiTakingQuestionDisplay
			question={vokiTakingState.currentQuestion}
			totalQuestionsCount={vokiTakingState.totalQuestionsCount}
			bind:questionChosenAnswers={vokiTakingState.chosenAnswers[vokiTakingState.currentQuestion.id]}
		/>
	{:else}
		<h1>Question error</h1>
		<button onclick={() => vokiTakingState.jumpToSpecificQuestion(1)}>Go to first question</button>
	{/if}

	<FreeAnsweringVokiTakingErrsList
		errs={vokiTakingErrs}
		jumpToSpecificQuestion={jumpToSpecificQuestionFromErrsList}
	/>
	<FreeAnsweringButtonsContainer {vokiTakingState} bind:vokiTakingErrs {onResultReceived} />
</div>
