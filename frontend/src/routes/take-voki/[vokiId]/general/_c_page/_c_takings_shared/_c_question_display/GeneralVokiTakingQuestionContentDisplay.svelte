<script lang="ts">
	import GeneralVokiTakingTextOnlyContent from './_c_content/GeneralVokiTakingTextOnlyContent.svelte';
	import GeneralVokiTakingImageOnlyContent from './_c_content/GeneralVokiTakingImageOnlyContent.svelte';
	import GeneralVokiTakingImageAndTextContent from './_c_content/GeneralVokiTakingImageAndTextContent.svelte';
	import GeneralVokiTakingColorOnlyContent from './_c_content/GeneralVokiTakingColorOnlyContent.svelte';
	import GeneralVokiTakingColorAndTextContent from './_c_content/GeneralVokiTakingColorAndTextContent.svelte';
	import GeneralVokiTakingAudioOnlyContent from './_c_content/GeneralVokiTakingAudioOnlyContent.svelte';
	import GeneralVokiTakingAudioAndTextContent from './_c_content/GeneralVokiTakingAudioAndTextContent.svelte';
	import type { GeneralVokiTakingQuestionContent } from '../../../types';

	interface Props {
		questionChosenAnswers: Record<string, boolean>;
		isQuestionMultipleChoice: boolean;
		content: GeneralVokiTakingQuestionContent;
	}
	let { questionChosenAnswers = $bindable(), isQuestionMultipleChoice, content }: Props = $props();

	function chooseAnswer(answerId: string) {
		if (isQuestionMultipleChoice) {
			questionChosenAnswers[answerId] = !questionChosenAnswers[answerId];
		} else {
			Object.keys(questionChosenAnswers).forEach((key) => {
				questionChosenAnswers[key] = false;
			});
			questionChosenAnswers[answerId] = true;
		}
	}
	function isAnswerChosen(answerId: string) {
		return questionChosenAnswers[answerId];
	}

	export function focusFirstAnswerCard() {
		answersContainer.focusFirstAnswerCard();
	}
	let answersContainer: { focusFirstAnswerCard: () => void } = $state<{
		focusFirstAnswerCard: () => void;
	}>()!;
</script>

{#if content.$type === 'TextOnly'}
	<GeneralVokiTakingTextOnlyContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'ImageOnly'}
	<GeneralVokiTakingImageOnlyContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'ImageAndText'}
	<GeneralVokiTakingImageAndTextContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'ColorOnly'}
	<GeneralVokiTakingColorOnlyContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'ColorAndText'}
	<GeneralVokiTakingColorAndTextContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'AudioOnly'}
	<GeneralVokiTakingAudioOnlyContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if content.$type === 'AudioAndText'}
	<GeneralVokiTakingAudioAndTextContent
		bind:this={answersContainer}
		{content}
		isMultipleChoice={isQuestionMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else}
	<div class="anwer-type-error">
		<h1>This answer type is not supported</h1>
	</div>
{/if}

<style>
	:global(.answers-container) {
		scroll-behavior: smooth;
	}

	:global(.answers-container > .answer) {
		box-shadow: var(--shadow), var(--shadow-xs);
		transition: transform 0.18s ease;
		scroll-margin: 12vh 0;
	}

	:global(.answers-container > .answer:hover),
	:global(.answers-container > .answer:focus),
	:global(.answers-container > .answer.chosen) {
		transition: transform 0.25s ease;
		transform: scale(1.009);
	}
</style>
