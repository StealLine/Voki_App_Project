<script lang="ts">
	import { onClickOutside } from 'runed';

	export type DropdownAction =
		| { type: 'btn'; onclick: () => void }
		| { type: 'link'; href: string };

	export type DropdownItem = {
		iconHref: string;
		text: string;
		action: DropdownAction;
		variant?: 'default' | 'red';
	};

	interface Props {
		items: DropdownItem[];
	}

	let { items }: Props = $props();

	let isOpen = $state(false);
	let containerRef = $state<HTMLElement | null>(null);

	export function open() {
		isOpen = !isOpen;
	}

	function close() {
		isOpen = false;
	}

	onClickOutside(() => containerRef, close);
</script>

{#if isOpen}
	<div class="dropdown" bind:this={containerRef}>
		{#each items as item}
			{#if item.action.type === 'link'}
				<a class="item" class:red={item.variant === 'red'} href={item.action.href} onclick={close}>
					<div class="icon-wrapper">
						<svg><use href={item.iconHref} /></svg>
					</div>
					<span>{item.text}</span>
				</a>
			{:else if item.action.type === 'btn'}
				<button
					class="item"
					class:red={item.variant === 'red'}
					onclick={() => {
						(item.action as { type: 'btn'; onclick: () => void }).onclick();
						close();
					}}
				>
					<div class="icon-wrapper">
						<svg><use href={item.iconHref} /></svg>
					</div>
					<span>{item.text}</span>
				</button>
			{/if}
		{/each}
	</div>
{/if}

<style>
	.dropdown {
		position: absolute;
		top: calc(100% + 0.375rem);
		left: 0;
		right: 0;
		background: var(--back);
		border-radius: 0.25rem;
		box-shadow: var(--shadow-xs);
		padding: 0.125rem;
		z-index: 9999;
		animation: fade-in 0.04s ease-out;
	}

	.item {
		display: grid;
		align-items: center;
		grid-template-columns: auto 1fr;
		gap: 0.375rem;
		width: 100%;
		padding: 0.25rem 1.25rem 0.25rem 0.5rem;
		border-radius: inherit;
		font-size: 1rem;
		font-weight: 425;
		color: var(--muted-foreground);
		background: transparent;
		border: none;
		cursor: default;
		text-decoration: none;
		transition: all 0.15s ease;
		text-align: left;
	}

	.item:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.item.red:hover {
		background-color: var(--red-1);
		color: var(--red-3);
	}

	.icon-wrapper {
		width: 1.125rem;
		height: 1.125rem;
		display: flex;
		align-items: center;
		justify-content: center;
	}

	.icon-wrapper svg {
		width: 100%;
		height: 100%;
		stroke-width: 1.75;
	}

	@keyframes fade-in {
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
