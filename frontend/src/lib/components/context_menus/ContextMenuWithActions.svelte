<script lang="ts">
	import type { Snippet } from 'svelte';
	import BaseContextMenu from './BaseContextMenu.svelte';

	export type ActionsContextMenuSnippetContent = { type: 'snippet'; snippet: Snippet };
	export type ActionsContextMenuMessageContent = {
		type: 'message';
		message: string;
		iconHref: string | null;
	};
	export type ActionsContextMenuActionsContent = {
		type: 'actions';
		items: ActionsContextMenuActionContentItem[];
	};

	type ActionType = 'default' | 'red';
	export type ActionsContextMenuActionContentItem = 'divider' | ActionsContextMenuAction;
	export type ActionsContextMenuAction = {
		label: string;
		iconHref: string | null;
		action: { isLink: true; href: string } | { isLink: false; onclick: () => void };
		type: ActionType;
	};

	interface Props {
		content:
			| ActionsContextMenuSnippetContent
			| ActionsContextMenuActionsContent
			| ActionsContextMenuMessageContent;
		onAfterClose?: () => void;
		class?: string;
		id?: string;
	}
	let { content, onAfterClose, class: className = '', id }: Props = $props();
	let menu = $state<BaseContextMenu>()!;
	export function open(x: number, y: number, ox = 0, oy = 0) {
		menu.open(x, y, ox, oy);
	}
	export function close() {
		menu.close();
	}
</script>

<BaseContextMenu
	bind:this={menu}
	class="context-menu-with-actions unselectable {className}"
	{onAfterClose}
	{id}
>
	{#if content.type === 'actions'}
		{#each content.items as item}
			{#if item === 'divider'}
				<div class="divider" />
			{:else if item.action.isLink}
				<a class="action {item.type === 'red' ? 'red' : ''}" href={item.action.href}>
					{@render actionContent(item)}
				</a>
			{:else if !item.action.isLink}
				<div
					class="action {item.type === 'red' ? 'red' : ''}"
					onclick={() =>
						(item.action as { onclick: (ctxMenu: BaseContextMenu) => void }).onclick(menu)}
				>
					{@render actionContent(item)}
				</div>
			{/if}
		{/each}
	{:else if content.type === 'snippet'}
		{@render content.snippet()}
	{:else if content.type === 'message'}
		<div class="message-container">
			{#if content.iconHref}
				<svg><use href={content.iconHref} /></svg>
			{/if}
			<label>{content.message}</label>
		</div>
	{:else}
		<span>Unexpected content type: {(content as any).type}</span>
	{/if}
</BaseContextMenu>
{#snippet actionContent(a: { label: string; iconHref: string | null })}
	<div class="icon-container">
		{#if a.iconHref}
			<svg><use href={a.iconHref} /></svg>
		{/if}
	</div>
	<span>{a.label}</span>
{/snippet}

<style>
	:global(.context-menu-with-actions) {
		display: grid;
		width: max-content;
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--back);
		color: var(--muted-foreground);
		box-shadow: var(--shadow-xs);
	}

	.action {
		display: grid;
		align-items: center;
		gap: 0.375rem;
		padding: 0.25rem 1.25rem 0.25rem 0.5rem;
		border-radius: inherit;
		color: inherit;
		font-size: 1rem;
		font-weight: 410;
		text-decoration: none;
		cursor: default;
		grid-template-columns: auto 1fr;
	}

	.icon-container {
		width: 1.125rem;
		height: 1.125rem;
	}

	.icon-container > svg {
		width: 100%;
		height: 100%;
		stroke-width: 1.75;
		color: inherit;
	}

	.action:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.action.red:hover {
		background-color: var(--red-1);
		color: var(--red-3);
	}

	:global(.context-menu-with-actions:has(.message-container)) {
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 0.75rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs), var(--shadow);
		grid-template-columns: auto 1fr;
	}

	.message-container {
		display: flex;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 450;
	}

	.message-container > svg {
		display: inline;
		width: 1.25rem;
		height: 1.25rem;
		margin-right: 0.25rem;
		stroke-width: 1.75;
	}
</style>
