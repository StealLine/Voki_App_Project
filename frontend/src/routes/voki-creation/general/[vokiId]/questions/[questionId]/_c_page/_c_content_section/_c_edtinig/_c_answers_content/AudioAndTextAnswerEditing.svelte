<script lang="ts">
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import type { AnswerDataAudioAndText } from '../../../../types';
	import CompactQuestionContentMediaInput from '../_c_shared/CompactQuestionContentMediaInput.svelte';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		answer: AnswerDataAudioAndText;
		onTextChange: (newText: string) => void;
		onAudioChange: (newAudio: string) => void;
	}
	let { answer, onTextChange, onAudioChange }: Props = $props();
</script>

<div class="answer-content">
	<div class="main">
		<AnswerEditingTextArea bind:text={() => answer.text, onTextChange} />
		<div class="audio-part">
			<CompactQuestionContentMediaInput
				type="audio"
				mediaUrl={answer.audio}
				onUploadSuccess={onAudioChange}
				mediaDisplay={audioDisplay}
			/>
		</div>
	</div>
</div>

{#snippet audioDisplay()}
	<BasicAudio src={answer.audio} />
{/snippet}

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
		height: 100%;
		padding: 0;
	}

	.main {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.audio-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		min-width: 12rem;
		transition:
			height 0.12s ease,
			width 0.12s ease;
	}

	.answer-content > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}
</style>
