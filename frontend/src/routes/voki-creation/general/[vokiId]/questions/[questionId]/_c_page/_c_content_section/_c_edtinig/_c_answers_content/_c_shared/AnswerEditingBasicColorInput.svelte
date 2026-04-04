<script lang="ts">
	let { color = $bindable() }: { color: string } = $props<{
		color: string;
	}>();
</script>

<div class="inputs">
	<div class="color-input-wrap">
		<input type="color" bind:value={color} class="color-input" />
	</div>
	<input type="text" bind:value={color} class="hex-input" />
</div>

<style>
	.inputs {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		width: 8rem;
	}

	.color-input-wrap {
		position: relative;
		display: inline-block;
		width: 100%;
		height: 6rem;

		--color-input-radius: 0.5rem;
	}

	.color-input {
		width: 100%;
		height: 100%;
		padding: 0;
		border: none;
		border-radius: var(--color-input-radius);
		cursor: pointer;
		-webkit-appearance: none;
		appearance: none;
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

	.color-input-wrap::after {
		position: absolute;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: var(--color-input-radius);
		box-shadow: var(--shadow-xs);
		content: '';
		inset: 0;
		pointer-events: none;
	}

	.hex-input {
		width: 100%;
		padding: 0.125rem 0.6rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		outline: none;
	}

	.hex-input:hover,
	.hex-input:focus {
		border-color: var(--primary);
	}
</style>
