<script lang="ts">
	type option = {
		name: string;
		selected: boolean;
		disabled: boolean;
		onclick: () => void;
	};
	interface Props {
		options: option[];
		containerClass?: string;
	}
	let { options, containerClass = '' }: Props = $props();
</script>

<div class="options {containerClass}">
	{#each options as option}
		<div
			class="option unselectable"
			class:selected={option.selected}
			class:disabled={option.disabled}
			onclick={option.onclick}
		>
			{option.name}
		</div>
	{/each}
</div>

<style>
	.options {
		display:flex ;
		flex-direction: row;
		justify-content: space-between;
		width: 100%;
	}

	.option {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.25rem;
		border-radius: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		transition: all 0.04s ease-in;
		cursor: default;
	}

	.option:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.option:active {
		transform: scale(0.98);
	}

	.option.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.option.disabled {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		opacity: 0.7;
		box-shadow: var(--shadow-xs);
		pointer-events: none;
	}
</style>
