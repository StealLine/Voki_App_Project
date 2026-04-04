<script lang="ts">
	import type { AnswerDataImageOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageOnly' }>;
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
{#snippet answerMainContent(answer: AnswerDataImageOnly)}
	<div class="answer-content">
		<GeneralVokiCreationAnswerDisplayImage src={answer.image} />
	</div>
{/snippet}

<style>
	.answer-content {
		margin-inline: auto;
	}
</style>
