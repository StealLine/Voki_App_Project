<script lang="ts">
	import { GeneralVokiUtils } from '$lib/ts/utils/general-voki-utils';
	import type { GeneralVokiQuestionContentType } from '$lib/ts/voki';

	interface Props {
		type: GeneralVokiQuestionContentType;
		selectedType: GeneralVokiQuestionContentType;
	}
	let { type, selectedType = $bindable() }: Props = $props();
</script>

<div class="type-card" class:selected={type === selectedType} onclick={() => (selectedType = type)}>
	{GeneralVokiUtils.questionContentTypeDisplayName(type)}
</div>

<style>
	.type-card {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		width: 11.5rem;
		padding: 0.75rem 0;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: bold;
		font-weight: 580;
		text-align: center;
		letter-spacing: 0.5px;
		box-shadow: var(--shadow-xs);
		transition: transform 0.1s ease-in;
		cursor: pointer;
		white-space: nowrap;
	}

	.type-card:not(.selected):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		box-shadow: none;
	}

	.type-card.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: var(--shadow-xl);
		box-shadow: none;
		transition: transform 0.18s ease-in;
		transform: scale(1.06);
	}
</style>
