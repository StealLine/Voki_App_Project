<script lang="ts">
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import type { AnswerDataImageAndText } from '../../../../types';
	import CompactQuestionContentMediaInput from '../_c_shared/CompactQuestionContentMediaInput.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	interface Props {
		answer: AnswerDataImageAndText;
		onTextChange: (newText: string) => void;
		onImageChange: (newImage: string) => void;
	}
	let { answer, onTextChange, onImageChange }: Props = $props();
</script>

<div class="answer-content">
	<div class="main">
		<AnswerEditingTextArea bind:text={() => answer.text, onTextChange} />
		<div class="image-part">
			<CompactQuestionContentMediaInput
				type="image"
				mediaUrl={answer.image}
				onUploadSuccess={onImageChange}
				mediaDisplay={imageDisplay}
			/>
		</div>
	</div>
</div>

{#snippet imageDisplay()}
	<GeneralVokiCreationAnswerDisplayImage src={answer.image} maxWidth={20} />
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

	.image-part {
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
