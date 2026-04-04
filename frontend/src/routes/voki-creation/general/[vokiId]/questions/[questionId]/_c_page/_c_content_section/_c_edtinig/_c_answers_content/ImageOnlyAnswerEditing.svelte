<script lang="ts">
	import type { AnswerDataImageOnly } from '../../../../types';
	import FullWidthQuestionContentMediaInput from '../_c_shared/FullWidthQuestionContentMediaInput.svelte';
	import GeneralVokiCreationAnswerDisplayImage from '../../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	interface Props {
		answer: AnswerDataImageOnly;
		onImageChange: (newImage: string) => void;
	}
	let { answer, onImageChange }: Props = $props();
</script>

<div class="answer-content">
	<FullWidthQuestionContentMediaInput
		type="image"
		mediaUrl={answer.image}
		onUploadSuccess={onImageChange}
		mediaDisplay={imageDisplay}
	/>
</div>

{#snippet imageDisplay()}
	<GeneralVokiCreationAnswerDisplayImage src={answer.image} />
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
