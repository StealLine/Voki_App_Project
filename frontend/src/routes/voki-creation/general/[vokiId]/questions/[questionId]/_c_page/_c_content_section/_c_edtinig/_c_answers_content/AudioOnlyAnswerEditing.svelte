<script lang="ts">
	import type { AnswerDataAudioOnly } from '../../../../types';
	import FullWidthQuestionContentMediaInput from '../_c_shared/FullWidthQuestionContentMediaInput.svelte';
	import BasicAudio from '$lib/components/BasicAudio.svelte';

	interface Props {
		answer: AnswerDataAudioOnly;
		onAudioChange: (newAudio: string) => void;
	}
	let { answer, onAudioChange }: Props = $props();
</script>

<div class="answer-content">
	<FullWidthQuestionContentMediaInput
		type="audio"
		mediaUrl={answer.audio}
		onUploadSuccess={onAudioChange}
		mediaDisplay={audioDisplay}
	/>
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
	}
</style>
