<script lang="ts">
	import type { AnswerDataTextOnly } from '../../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';

	interface Props {
		answer: AnswerDataTextOnly;
		onTextChange: (newText: string) => void;
	}
	let { answer, onTextChange }: Props = $props();
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text={() => answer.text, onTextChange} />
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
		height: 100%;
	}
</style>
