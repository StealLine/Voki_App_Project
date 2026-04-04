<script lang="ts">
	const { text, iconId }: { text: string; iconId: string } = $props<{
		text: string;
		iconId: string;
	}>();
</script>

<div class="field">
	<svg><use href={iconId} /></svg>

	<label>{text}</label>
</div>

<style>
	.field {
		display: grid;
		align-items: center;
		gap: 0.375rem;
		width: 10rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.4px;
		grid-template-columns: 1.5rem 1fr;
		box-shadow: var(--shadow-xs);
	}

	.field > svg {
		width: 100%;
		aspect-ratio: 1/1;
		stroke-width: 1.8;
	}
</style>
