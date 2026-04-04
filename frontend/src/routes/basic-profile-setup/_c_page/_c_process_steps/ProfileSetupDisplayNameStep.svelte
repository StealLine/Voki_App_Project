<script lang="ts">
	interface Props {
		displayName: string;
		maxLength: number;
	}
	let { displayName = $bindable(), maxLength = 30 }: Props = $props();
</script>

<div class="display-name-container">
	<svg class="input-icon" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
		<use href="#caret-right-icon" />
	</svg>
	<input
		type="text"
		class="name-input"
		bind:value={displayName}
		style="width: {maxLength + 2}ch"
		maxlength={maxLength}
	/>
	<div class="len-count">
		<span class="current" class:err={displayName.length > maxLength}>{displayName.length}</span>
		<span class="max">/{maxLength}</span>
	</div>
</div>

<style>
	.display-name-container {
		position: relative;
		display: grid;
		width: fit-content;
		margin-inline: auto;
	}

	.input-icon {
		position: absolute;
		top: 50%;
		left: 0.375rem;
		width: 1.675rem;
		height: 1.675rem;
		color: var(--secondary-foreground);
		font-size: 1.5rem;
		font-weight: 600;
		transform: translateY(-50%);
		pointer-events: none;
		stroke-width: 2.2;
	}

	.name-input {
		--border-color: var(--secondary-foreground);

		max-width: calc(var(--width-limit) - var(--sidebar-width) - 8rem);
		padding: 0.675rem 1rem 0.675rem 1.75rem;
		border: 0.125rem solid var(--border-color);
		border-radius: 0.5rem;
		background-color: var(--back);
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 1px;
		outline: none;
	}

	.name-input:hover,
	.name-input:focus {
		--border-color: var(--primary);
	}

	.display-name-container:has(input:focus) .input-icon {
		color: var(--primary);
		font-weight: 700;
	}

	.len-count {
		position: absolute;
		top: calc(100%);
		right: 1rem;
		display: grid;
		padding: 0 0.125rem;
		background-color: var(--back);
		font-size: 0.875rem;
		font-weight: 450;
		transform: translateY(-50%);
		grid-template-columns: auto auto;
		pointer-events: none;
		font-variant-numeric: tabular-nums;
	}

	.len-count .current {
		text-align: right;
	}

	.len-count .current.err {
		color: var(--red-5);
		font-weight: 500;
	}

	.len-count .max {
		text-align: left;
	}
</style>
