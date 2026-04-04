<script lang="ts">
	import { type Snippet } from 'svelte';

	interface Props {
		icon?: Snippet;
		title: string | null;
		subtitle: Snippet;
		actionItem:
			| { type: 'button'; text: string; onClick: () => void }
			| { type: 'custom'; actionItem: Snippet };
	}
	let { icon = undefined, title, subtitle, actionItem }: Props = $props();
</script>

<div class="co-authors-message-card">
	{@render icon?.()}
	{#if title}
		<h2 class="title">{title}</h2>
	{/if}
	<p class="subtitle">
		{@render subtitle?.()}
	</p>
	{#if actionItem.type === 'button'}
		<button class="action-btn" onclick={actionItem.onClick}>{actionItem.text}</button>
	{:else if actionItem.type === 'custom'}
		{@render actionItem.actionItem?.()}
	{/if}
</div>

<style>
	.co-authors-message-card {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 100%;
		margin-top: 1rem;
		border-radius: 1rem;
		background-color: var(--back);
		text-align: center;
	}

	.co-authors-message-card > :global(svg) {
		width: 4rem;
		height: 4rem;
		line-height: 1;
		stroke-width: 1.5;
	}

	.title {
		margin: -0.25rem 0 0.25rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
	}

	.subtitle {
		max-width: 26rem;
		color: var(--muted-foreground);
		font-size: 1rem;
		line-height: 1.1;
	}

	.action-btn {
		display: flex;
		align-items: center;
		gap: 0.375rem;
		padding: 0.5rem 1.5rem;
		margin-top: 1rem;
		border: none;
		border-radius: 10rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1rem;
		font-weight: 500;
		letter-spacing: 0.4px;
		transition: background-color 0.15s ease;
		transition: padding 0.15s ease;
		cursor: pointer;
		outline: none;
	}

	.action-btn:hover,
	.action-btn:active,
	.action-btn:focus {
		padding: 0.5rem 1.75rem;
		background-color: var(--primary-hov);
	}
</style>
