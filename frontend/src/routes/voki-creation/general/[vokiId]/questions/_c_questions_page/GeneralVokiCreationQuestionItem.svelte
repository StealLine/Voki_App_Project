<script lang="ts">
	import { toast } from 'svelte-sonner';
	import type { QuestionBriefInfo } from '../types';
	import QuestionOrderButtons from './_c_question_item/QuestionOrderButtons.svelte';
	import QuestionProps from './_c_question_item/QuestionProps.svelte';

	interface Props {
		vokiId: string;
		question: QuestionBriefInfo;
		questionsCount: number;
		moveQuestionUpInOrder: () => void;
		moveQuestionDownInOrder: () => void;
		deleteQuestion: () => void;
	}
	let {
		vokiId,
		question,
		questionsCount,
		moveQuestionDownInOrder,
		moveQuestionUpInOrder,
		deleteQuestion
	}: Props = $props();
</script>

<div class="question">
	<QuestionOrderButtons
		questionOrder={question.orderInVoki}
		{questionsCount}
		{moveQuestionUpInOrder}
		{moveQuestionDownInOrder}
	/>
	<div class="question-content">
		<p class="text">
			<span class="order">{question.orderInVoki}.</span>
			{question.text}
		</p>
		<div class="question-bottom">
			<QuestionProps {question} />
			<div class="buttons-container">
				<a href="/voki-creation/general/{vokiId}/questions/{question.id}" class="edit-btn">Edit</a>
				<button class="delete-btn" onclick={deleteQuestion}>
					<svg><use href="#common-trash-can-icon" /></svg>
				</button>
			</div>
		</div>
	</div>
</div>

<style>
	.question {
		display: grid;
		gap: 0.5rem;
		padding: 0.25rem 0.5rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.75rem;
		grid-template-columns: auto 1fr;
	}

	.question-content {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.text {
		margin: 0.25rem 0 0;
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 450;
	}

	.text .order {
		margin: 0 0.125rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
	}

	.question-bottom {
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		align-items: center;
		margin: 0.25rem 0.25rem 0.25rem 0;
	}

	.buttons-container {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
	}

	.buttons-container > * {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		box-shadow: var(--shadow);
		outline: none;
		cursor: pointer;
	}

	.edit-btn {
		padding: 0 1.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 480;
		letter-spacing: 1.5px;
	}

	.edit-btn:hover {
		background-color: var(--primary-hov);
	}

	.delete-btn {
		width: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		stroke-width: 2.1;
	}

	.delete-btn:hover {
		background-color: var(--red-5);
		color: var(--primary-foreground);
		stroke-width: 1.8;
	}

	.delete-btn svg {
		width: 1.5rem;
		height: 1.5rem;
	}
</style>
