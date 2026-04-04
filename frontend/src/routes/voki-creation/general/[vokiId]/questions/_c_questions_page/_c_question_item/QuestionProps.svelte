<script lang="ts">
	import { GeneralVokiUtils } from '$lib/ts/utils/general-voki-utils';
	import type { QuestionBriefInfo } from '../../types';

	interface Props {
		question: QuestionBriefInfo;
	}
	const { question }: Props = $props();
</script>

<div class="props">
	<div class="property">
		{GeneralVokiUtils.questionContentTypeDisplayName(question.contentType)}
	</div>
	<div class="property">
		{#if question.images.length == 0}
			<svg><use href="#no-image-icon" /></svg>
			No images
		{:else if question.images.length == 1}
			<svg><use href="#single-image-icon" /></svg>
			1 image
		{:else}
			<svg><use href="#multiple-images-icon" /></svg>
			{question.images.length} images
		{/if}
	</div>
	<div class="property">
		{#if question.isMultipleChoice}
			<svg><use href="#general-voki-multiple-choice-icon" /></svg>
			Multiple choice
		{:else}
			<svg><use href="#general-voki-single-choice-icon" /></svg>
			Single choice
		{/if}
	</div>
	<div class="property">
		{#if question.shuffleAnswers}
			<svg><use href="#common-shuffle-icon" /></svg>
			Answers shuffled
		{:else}
			<svg><use href="#common-order-icon" /></svg>
			Answers ordered
		{/if}
	</div>
</div>

<style>
	.props {
		display: flex;
		flex-direction: row;
		gap: 1rem;
	}

	.property {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.25rem;
		width: fit-content;
		padding: 0.25rem 0.675rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		letter-spacing: 0.2px;
		box-shadow: var(--shadow-xs);
		cursor: default;
	}

	.property > svg {
		width: 1.125rem;
		aspect-ratio: 1/1;
		stroke-width: 2.2;
	}
</style>
