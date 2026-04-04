<script lang="ts">
	let { vokiName }: { vokiName: string } = $props<{ vokiName: string }>();
</script>

<div class="tooltip-container">
	<span class="voki-name">{vokiName}</span>
	<div class="tooltip">{vokiName}</div>
</div>

<style>
	.tooltip-container {
		position: relative;
		display: flex;
		align-items: center;
		max-width: 18rem;
	}

	.voki-name {
		display: inline-block;
		max-width: 18rem;
		margin: 0 0.25rem;
		color: var(--primary);
		font-size: inherit;
		font-weight: 640;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
	}

	.tooltip {
		position: absolute;
		top: calc(100% + 1rem);
		left: 50%;
		z-index: 1000;
		width: max-content;
		max-width: 40rem;
		padding: 0.25rem 0.5rem;
        border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.5rem;
		background: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: normal;
        font-weight: 450;
		opacity: 0;
		transition: all 0.2s ease-in-out;
		transform: translateX(-50%);
		word-break: normal;
		overflow-wrap: anywhere;
		visibility: hidden;
		pointer-events: none;
        text-indent: 0.25em;
	}

	.tooltip::after {
		position: absolute;
		bottom: 100%;
		left: 50%;
		border: 0.5rem solid transparent;
		transform: translateX(-50%) rotate(180deg);
		content: '';
		border-top-color: var(--secondary-foreground);
	}

	.tooltip-container:hover .tooltip {
		opacity: 1;
		visibility: visible;
		transform: translateX(-50%) translateY(-0.5rem);
	}
</style>
