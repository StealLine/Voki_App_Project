<script lang="ts">
	interface Props {
		isMultipleChoice: boolean;
		isChosen: boolean;
		onClick?: () => void;
	}
	let { isMultipleChoice, isChosen, onClick = () => {} }: Props = $props();
</script>

<div
	class="indicator"
	class:chosen={isChosen}
	class:single={!isMultipleChoice}
	class:multiple={isMultipleChoice}
	onclick={onClick}
>
	{#if isMultipleChoice}
		<svg viewBox="0 0 12 10"> <polyline points="1.5 6 4.5 9 10.5 1" /></svg>
	{:else}
		<span></span>
	{/if}
</div>

<style>
	.indicator {
		display: inline-block;
		width: 1.25rem;
		height: 1.25rem;
		border: 0.125rem solid var(--secondary-foreground);
		transition:
			all 0.2s ease,
			border-radius 0s;
		vertical-align: middle;
	}

	.multiple {
		border-radius: 0.25rem;
	}

	.multiple svg {
		width: 100%;
		height: 100%;
		box-sizing: border-box;
		padding: 0.125rem;
		transition: all 0.3s ease;
		fill: none;
		stroke: var(--primary-foreground);
		stroke-width: 2;
		stroke-linecap: round;
		stroke-linejoin: round;
		stroke-dasharray: 1rem;
		stroke-dashoffset: 1rem;
		transition-delay: 0.1s;
	}

	.multiple.chosen {
		border-color: var(--primary);
		background: var(--primary);
	}

	.multiple.chosen svg {
		stroke-dashoffset: 0;
	}

	.single {
		display: flex;
		justify-content: center;
		align-items: center;
		border-radius: 50%;
	}

	.single span {
		display: inline-block;
		width: 0%;
		border-radius: 50%;
		opacity: 0;
		transition: inherit;
		aspect-ratio: 1;
	}

	.single.chosen {
		border-color: var(--primary);
	}

	.single.chosen span {
		width: 0.675rem;
		background: var(--primary);
		opacity: 1;
		transform: scale(1);
	}
</style>
