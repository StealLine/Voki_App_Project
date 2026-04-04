<script lang="ts">
	type ReloadButtonIcon = 'arrow' | 'loading';
	type ReloadButtonContent =
		| { icon: ReloadButtonIcon; text: null }
		| { icon: null; text: string }
		| { icon: ReloadButtonIcon; text: string };

	interface Props {
		onclick: () => void;
		content?: ReloadButtonContent;
		className?: string;
	}
	let { content = { icon: null, text: 'Reload' }, onclick, className = '' }: Props = $props();
</script>

<button class={`reload-btn ${className}`} onclick={() => onclick()}>
	{#if content.icon === 'arrow'}
		<svg class="reload-arrow"><use href="#common-reload-icon" /></svg>
	{:else if content.icon === 'loading'}
		loading
	{/if}
	{#if content.text}
		{content.text}
	{/if}
</button>

<style>
	.reload-btn {
		display: flex;
		align-items: center;
		gap: 0.25rem;
		width: fit-content;
		padding: 0.25rem 1rem;
		margin: 0 auto;
		margin-bottom: 2rem;
		border: none;
		border-radius: 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 440;
		transition: transform 0.08s ease-out;
		cursor: pointer;
	}

	.reload-btn > svg {
		width: 1.125rem;
		height: 1.125rem;
		transition: transform 0.2s ease-out;
		stroke-width: 2.23;
	}

	.reload-btn:hover > svg.reload-arrow {
		transform: rotate(60deg);
	}

	.reload-btn:active {
		transform: scale(0.96);
	}

	.reload-btn:active > svg.reload-arrow {
		transform: rotate(180deg);
	}
</style>
