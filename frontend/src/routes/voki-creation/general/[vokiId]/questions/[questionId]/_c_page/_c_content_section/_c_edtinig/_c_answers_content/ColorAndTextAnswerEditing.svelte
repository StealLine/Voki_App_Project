<script lang="ts">
	import type { AnswerDataColorAndText } from '../../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import AnswerEditingBasicColorInput from './_c_shared/AnswerEditingBasicColorInput.svelte';
	interface Props {
		answer: AnswerDataColorAndText;
		onTextChange: (newText: string) => void;
		onColorChange: (newColor: string) => void;
	}
	let { answer, onTextChange, onColorChange }: Props = $props();
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text={() => answer.text, onTextChange} />
	<AnswerEditingBasicColorInput bind:color={() => answer.color, onColorChange} />
</div>

<style>
	.answer-content {
		display: grid;
		align-items: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
		padding: 0.5rem 0 0;
		grid-template-columns: 1fr auto;
	}
</style>
