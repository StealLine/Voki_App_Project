<script lang="ts">
	interface Props {
		value: string;
		label: string;
	}
	let { value = $bindable(), label }: Props = $props();
</script>

<div class="color-field">
	<span class="label">{label}</span>
	<input class="color-input" type="color" bind:value />
	<input class="text-input" type="text" placeholder="#RRGGBB" spellcheck="false" bind:value />
</div>

<style>
	.color-field {
		display: grid;
		justify-content: center;
		gap: 0.375rem;
		width: 10rem;
		animation: appear-animation 0.2s ease-in;
		grid-template-rows: auto auto auto;
		justify-items: center;
	}

	.label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
	}

	.color-input {
		--color-input-radius: 0.5rem;

		width: calc(100% - 2rem);
		height: 6rem;
		padding: 0;
		border: none;
		border: 0.075rem solid var(--muted);
		border-radius: var(--color-input-radius);
		transition: all 0.12s ease;
		cursor: pointer;
		-webkit-appearance: none;
		appearance: none;
	}

	.color-input:hover,
	.color-input:focus,
	.color-input:active {
		transform: scale(1.04);
	}

	.color-input::-webkit-color-swatch-wrapper {
		padding: 0;
		border-radius: var(--color-input-radius);
	}

	.color-input::-webkit-color-swatch {
		border: none;
		border-radius: var(--color-input-radius);
	}

	.color-input::-moz-color-swatch {
		border: none;
		border-radius: var(--color-input-radius);
	}

	.text-input {
		width: 100%;
		padding: 0.375rem 0.5rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.375rem;
background-color: var(--back);
		font-size: 1rem;
		outline: none;
	}

	.text-input:hover:not(:focus) {
		border-color: var(--secondary-foreground);
	}

	.text-input:focus {
		border-color: var(--primary);
	}

	@keyframes appear-animation {
		from {
			opacity: 0;
			transform: scale(0.6);
		}

		to {
			opacity: 1;
			transform: scale(1);
		}
	}
</style>
