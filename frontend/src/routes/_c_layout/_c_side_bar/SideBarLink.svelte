<script lang="ts">
	import type { Snippet } from 'svelte';
	interface SideBarLinkProps {
		href: string;
		text: string;
		children: Snippet;
		isCollapsed: boolean;
		animateIcon: () => void;
	}
	let { href, text, children, isCollapsed, animateIcon }: SideBarLinkProps = $props();
</script>

<a
	{href}
	class:collapsed={isCollapsed}
	class="link"
	onmouseenter={() => animateIcon()}
	onfocus={() => animateIcon()}
>
	{@render children()}
	<label>{text}</label>
</a>

<style>
	.link {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.25rem;
		background-color: var(--secondary);
		color: var(--primary);
		font-size: 1.125rem;
		font-weight: 450;
		transition: transform 0.12s ease-in;
		cursor: pointer;
		grid-template-columns: 1.675rem 1fr;
		outline-color: var(--primary);
	}

	.link:hover {
		background-color: var(--accent);
	}

	.link * {
		cursor: inherit;
	}

	.link.collapsed {
		display: flex;
	}

	.link.collapsed > label {
		display: none;
	}
</style>
