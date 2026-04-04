<script lang="ts">
	import type { AnswerDataAudioAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'AudioAndText' }>;
		resultsIdToName: QuestionPageResultsState;
	}
	let { content, resultsIdToName }: Props = $props();
</script>

<div class="question-content">
	<QuestionContentViewAnswersList
		answers={content.answers}
		resultsIdToNameState={resultsIdToName}
		{answerMainContent}
	/>
</div>
{#snippet answerMainContent(answer: AnswerDataAudioAndText)}
	<div class="answer-content">
		<AnswersViewTextDisplay text={answer.text} />
		<div class="audio-container">
			<BasicAudio src={answer.audio} />
		</div>
	</div>
{/snippet}

<style>
	.answer-content {
		display: grid;
		grid-template-columns: 1fr 20rem;
		gap: 1rem;
		align-items: center;
		margin-inline: auto;
		width: 100%;
	}

	.audio-container {
		width: 100%;
	}
</style>
