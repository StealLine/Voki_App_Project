<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiTakingQuestionData } from '../../types';
	import GeneralVokiTakingQuestionContentDisplay from './_c_question_display/GeneralVokiTakingQuestionContentDisplay.svelte';

	interface Props {
		question: GeneralVokiTakingQuestionData;
		totalQuestionsCount: number;
		questionChosenAnswers: Record<string, boolean>;
	}
	let { question, totalQuestionsCount, questionChosenAnswers = $bindable() }: Props = $props();
	let isMultipleChoice = $derived(question.minAnswersCount != 1 || question.maxAnswersCount != 1);
</script>

<div class="question-display-container">
	<label class="question-num">
		Question #{question.orderInVokiTaking} out of {totalQuestionsCount}
	</label>
	<h2 class="question-text">{question.text}</h2>
	<div class="images-container">
		{#each question.imageKeys as image}
			<img
				src={StorageBucketMain.fileSrc(image)}
				alt="question-img"
				style="aspect-ratio: {question.imagesAspectRatio}"
			/>
		{/each}
	</div>
	<label class="answers-count-label">
		{#if question.minAnswersCount === question.maxAnswersCount}
			Choose {question.minAnswersCount} answer
		{:else}
			Choose from {question.minAnswersCount} to {question.maxAnswersCount} answers
		{/if}
	</label>
</div>
<GeneralVokiTakingQuestionContentDisplay
	content={question.content}
	isQuestionMultipleChoice={isMultipleChoice}
	bind:questionChosenAnswers
/>

<style>
	.question-display-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		margin-bottom: 2rem;
	}

	.question-num {
		color: var(--muted-foreground);
		font-size: 1.125rem;
	}

	.question-text {
		margin: 4px 0;
		color: var(--text);
		font-size: 24px;
		font-weight: 500;
	}

	.images-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		gap: 1rem;
	}

	.images-container img {
		width: 100%;
		min-width: 20rem;
		max-width: 32rem;
		min-height: 15rem;
		max-height: 25rem;
		object-fit: fill;
		border-radius: 1rem;
	}

	.answers-count-label {
		margin: 0;
		color: var(--text);
		font-size: 18px;
	}
</style>
