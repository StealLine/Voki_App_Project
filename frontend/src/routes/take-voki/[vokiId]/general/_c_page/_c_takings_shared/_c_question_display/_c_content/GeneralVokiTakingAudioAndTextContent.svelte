<script lang="ts">
	import { onMount, onDestroy } from 'svelte';
	import GeneralTakingAnswerChosenIndicator from './_c_shared/GeneralTakingAnswerChosenIndicator.svelte';
	import GeneralTakingAnswerText from './_c_shared/GeneralTakingAnswerText.svelte';
	import { answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiTakingQuestionContent } from '../../../../types';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		content: Extract<GeneralVokiTakingQuestionContent, { $type: 'AudioAndText' }>;
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
			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
			<div class="content-wrapper">
				<GeneralTakingAnswerText text={answer.text} />
				<BasicAudio src={answer.audio} />
			</div>
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.answer {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.5rem 1rem;
		border-radius: 0.5rem;
		grid-template-columns: auto 1fr;
	}

	.content-wrapper {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}
</style>
