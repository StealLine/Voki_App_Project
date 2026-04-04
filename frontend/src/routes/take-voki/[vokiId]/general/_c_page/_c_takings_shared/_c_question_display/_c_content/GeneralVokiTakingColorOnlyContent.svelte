<script lang="ts">
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import { onDestroy, onMount } from 'svelte';
	import type { GeneralVokiTakingQuestionContent } from '../../../../types';
	import { answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './_c_shared/GeneralTakingAnswerChosenIndicator.svelte';

	interface Props {
		content: Extract<GeneralVokiTakingQuestionContent, { $type: 'ColorOnly' }>;
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
			<div
				class="color-div"
				style="background-color:{ColorUtils.normalizeHex6(answer.color) ?? answer.color};"
			>
				<div class="indicator-container">
					<GeneralTakingAnswerChosenIndicator
						{isMultipleChoice}
						isChosen={isAnswerChosen(answer.id)}
					/>
				</div>
			</div>
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		gap: 2rem;
	}

	.answer {
		display: grid;
		gap: 0.5rem;
		height: 7rem;
		padding: 1rem;
		border-radius: 1rem;
		flex: 0 1 18rem;
	}

	.color-div {
		--color-div-border-radius: 0.675rem;

		position: relative;
		width: 100%;
		height: 100%;
		border: 1px solid var(--muted);
		border-radius: var(--color-div-border-radius);
	}

	.indicator-container {
		position: absolute;
		bottom: -0.75rem;
		left: 50%;
		padding: 0.5rem 0.5rem 0.25rem;
		border-radius: var(--color-div-border-radius);
		background-color: var(--back);
		transform: translateX(-50%);
		border-top: 1px solid var(--muted);
	}
</style>
