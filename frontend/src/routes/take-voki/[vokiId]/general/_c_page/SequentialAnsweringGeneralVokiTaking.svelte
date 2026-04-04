<script lang="ts">
	import type { GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from '../types';
	import GeneralVokiTakingQuestionDisplay from './_c_takings_shared/GeneralVokiTakingQuestionDisplay.svelte';
	import { SequentialAnsweringGeneralVokiTakingState } from './_c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import SequentialAnsweringNavigationContainer from './_c_sequential_answering_taking/SequentialAnsweringNavigationContainer.svelte';

	interface Props {
		takingData: GeneralVokiTakingData;
		saveData: PosssibleGeneralVokiTakingDataSaveData;
		clearVokiSeenUpdateTimer: () => void;
		onResultReceived: (resultId: string) => void;
	}
	let { takingData, saveData, clearVokiSeenUpdateTimer, onResultReceived }: Props = $props();

	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(
		takingData,
		saveData,
		clearVokiSeenUpdateTimer
	);
</script>

{#if vokiTakingState.currentQuestion}
	<GeneralVokiTakingQuestionDisplay
		question={vokiTakingState.currentQuestion}
		totalQuestionsCount={vokiTakingState.totalQuestionsCount}
		bind:questionChosenAnswers={vokiTakingState.currentQuestionChosenAnswers}
	/>
	<SequentialAnsweringNavigationContainer {vokiTakingState} {onResultReceived} />
{:else}
	<h1>Question error</h1>
{/if}
