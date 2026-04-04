<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { onMount, onDestroy } from 'svelte';
	import type { GeneralVokiTakingQuestionContent } from '../../../../types';
	import { answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './_c_shared/GeneralTakingAnswerChosenIndicator.svelte';
	import GeneralTakingAnswerText from './_c_shared/GeneralTakingAnswerText.svelte';

	interface Props {
		content: Extract<GeneralVokiTakingQuestionContent, { $type: 'ImageAndText' }>;
		isMultipleChoice: boolean;
		isAnswerChosen: (answerId: string) => boolean;
		chooseAnswer: (answerId: string) => void;
	}
	let { content, isMultipleChoice, isAnswerChosen, chooseAnswer }: Props = $props();

	let navAction = $state<ReturnType<typeof answersKeyboardNav>>()!;
	let answersContainer: HTMLDivElement = $state<HTMLDivElement>()!;
	export function focusFirstAnswerCard() {
		navAction?.focusFirstAnswer();
	}
	onMount(() => {
		navAction = answersKeyboardNav(answersContainer, {
			answers: content.answers,
			chooseAnswer,
			focusOnMount: true,
			useSpacebarToChoose: true
		});
	});
	onDestroy(() => navAction?.destroy?.());
</script>

<div
	class="answers-container"
	bind:this={answersContainer}
	tabindex="-1"
	role={isMultipleChoice ? 'group' : 'radiogroup'}
	aria-label="Answer choices"
>
	{#each content.answers.toSorted((a, b) => a.orderInQuestionInSession - b.orderInQuestionInSession) as answer}
		<div
			class="answer"
			class:chosen={isAnswerChosen(answer.id)}
			onclick={() => chooseAnswer(answer.id)}
			tabindex="0"
			role={isMultipleChoice ? 'checkbox' : 'radio'}
			aria-checked={isAnswerChosen(answer.id)}
		>
			<div class="img-container">
				<img
					class="unselectable"
					src={StorageBucketMain.fileSrc(answer.image)}
					alt="Answer"
					draggable="false"
				/>
			</div>
			<div class="text-indicator-wrapper">
				<GeneralTakingAnswerChosenIndicator
					{isMultipleChoice}
					isChosen={isAnswerChosen(answer.id)}
				/>
				<GeneralTakingAnswerText text={answer.text} />
			</div>
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: grid;
		justify-content: center;
		gap: 1.5rem;
		grid-template-columns: repeat(auto-fill, minmax(25rem, 1fr));
	}

	.answer {
		display: grid;
		gap: 0.5rem;
		padding: 1rem 0.5rem;
		border-radius: 1rem;
		background-color: var(--back);
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.img-container {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		width: fit-content;
		height: 100%;
	}

	img {
		min-width: 16rem;
		max-width: 25rem;
		max-height: 18rem;
		border-radius: 0.75rem;
		object-fit: contain;
		-webkit-user-drag: none;
		box-shadow: var(--shadow-xs);
	}

	.text-indicator-wrapper {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
	}
</style>
