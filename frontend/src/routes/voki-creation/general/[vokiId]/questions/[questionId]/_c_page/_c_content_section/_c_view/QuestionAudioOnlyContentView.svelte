<script lang="ts">
	import type { AnswerDataAudioOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'AudioOnly' }>;
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
{#snippet answerMainContent(answer: AnswerDataAudioOnly)}
	<div class="answer-content">
		<BasicAudio src={answer.audio} />
	</div>
{/snippet}

<style>
	.answer-content {
		width: calc(100% - 4rem);
		margin: auto 2rem;
	}
</style>
