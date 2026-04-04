<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';

	interface Props {
		state: 'initLoading' | 'existsCheckLoading' | 'existsCheckFailed' | 'success';
	}
	let { state }: Props = $props();

	type CircleType = 'loading' | 'success' | 'failed' | 'empty';

	const circle1Type = $derived<CircleType>(state === 'initLoading' ? 'loading' : 'success');
	const circle2Type = $derived<CircleType>(
		state === 'initLoading'
			? 'empty'
			: state === 'existsCheckLoading'
				? 'loading'
				: state === 'existsCheckFailed'
					? 'failed'
					: 'success'
	);
</script>

<div class="stepper">
	<div class="connector" class:active={true}></div>

	<div class="step">
		{@render circle(circle1Type)}
		<span class="step-label">Initialize</span>
	</div>

	<div class="connector" class:active={state !== 'initLoading'}></div>

	<div class="step">
		{@render circle(circle2Type)}
		<span class="step-label">Verify</span>
	</div>

	<div class="connector" class:active={state === 'success'}></div>
</div>

{#snippet circle(type: CircleType)}
	<div
		class="circle"
		class:loading={type === 'loading'}
		class:success={type === 'success'}
		class:failed={type === 'failed'}
		class:empty={type === 'empty'}
	>
		{#if type === 'loading'}
			<LinesLoader sizeRem={1.75} strokePx={2} color="var(--primary)" speedSec={1.2} />
		{:else if type === 'success'}
			<svg class="icon"><use href="#common-check-icon" /></svg>
		{:else if type === 'failed'}
			<svg class="icon"><use href="#common-warning-icon" /></svg>
		{:else if type === 'empty'}
			<span class="step-number">2</span>
		{/if}
	</div>
{/snippet}

<style>
	.stepper {
		display: flex;
		align-items: flex-start;
		justify-content: center;
		width: 100%;
		max-width: 24rem;
		margin: 0 auto;
		padding: 2rem 0;
	}

	.step {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		flex-shrink: 0;
	}

	.step-label {
		font-size: 0.6875rem;
		font-weight: 600;
		color: var(--muted-foreground);
		letter-spacing: 0.06em;
		text-transform: uppercase;
		user-select: none;
	}

	.connector {
		flex: 1;
		height: 0.125rem;
		margin-top: 1.5625rem;
		background-color: var(--muted);
		border-radius: 100vmax;
		transition: background-color 0.6s ease;
	}

	.connector.active {
		background-color: var(--primary);
	}

	.circle {
		width: 3.5rem;
		height: 3.5rem;
		border-radius: 50%;
		border: 0.125rem solid var(--muted);
		display: flex;
		align-items: center;
		justify-content: center;
		background-color: var(--back);
		color: var(--muted-foreground);
		transition:
			background-color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			border-color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			box-shadow 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			transform 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			opacity 0.45s ease;
		position: relative;
		z-index: 2;
		flex-shrink: 0;
	}

	.circle.loading {
		border-color: var(--primary);
		box-shadow: 0 0 0 0.3rem var(--accent);
		transform: scale(1.1);
	}

	.circle.success {
		background-color: var(--primary);
		border-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: var(--shadow-md);
		transform: scale(1.05);
	}

	.circle.failed {
		background-color: var(--warn-back);
		color: var(--warn-foreground);
		border-color: var(--warn-foreground);
	}

	.circle.empty {
		opacity: 0.45;
		transform: scale(0.88);
		border-style: dashed;
	}
	.icon {
		width: 1.75rem;
		height: 1.75rem;
		flex-shrink: 0;
		stroke-width: 1.75;
	}

	.circle.success .icon {
		animation: popIn 0.45s cubic-bezier(0.175, 0.885, 0.32, 1.275) both;
	}

	.circle.failed .icon {
		animation: popIn 0.45s cubic-bezier(0.175, 0.885, 0.32, 1.275) both;
	}

	.step-number {
		font-size: 1rem;
		font-weight: 600;
		color: var(--muted-foreground);
	}

	@keyframes popIn {
		from {
			transform: scale(0) rotate(-30deg);
			opacity: 0;
		}
		to {
			transform: scale(1) rotate(0);
			opacity: 1;
		}
	}
</style>
