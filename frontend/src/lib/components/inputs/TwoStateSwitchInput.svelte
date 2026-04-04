<script lang="ts">
	let {
		value = $bindable(),
		trueLabel,
		trueIconId,
		falseLabel,
		falseIconId
	}: {
		value: boolean;
		trueIconId: string;
		trueLabel: string;
		falseLabel: string;
		falseIconId: string;
	} = $props<{
		value: boolean;
		trueLabel: string;
		trueIconId: string;
		falseLabel: string;
		falseIconId: string;
	}>();
</script>

<div class="switch-container">
	<div class="option" class:selected={value} onclick={() => (value = true)}>
		<svg><use href={trueIconId} /></svg>
		{trueLabel}
	</div>
	<div class="sep" />
	<div class="option" class:selected={!value} onclick={() => (value = false)}>
		<svg><use href={falseIconId} /></svg>
		{falseLabel}
	</div>
</div>

<style>
	.switch-container {
		display: grid;
		gap: 0.5rem;
        width: fit-content;
		border-radius: 0.375rem;
		grid-template-columns: 1fr auto 1fr;
	}

	.sep {
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--muted);
	}

	.option {
		display: grid;
		align-items: center;
		gap: 0.25rem;
		width: 10rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.4px;
		box-shadow: var(--shadow-xs);
		transition: transform 0.12s ease-in;
		cursor: pointer;
		grid-template-columns: 1.5rem 1fr;
	}

	.option > svg {
		width: 100%;
		aspect-ratio: 1/1;
		stroke-width: 1.9;
	}

	.option:not(.selected):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		box-shadow: none;
		
	}

	.option.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: none;
	}
</style>
