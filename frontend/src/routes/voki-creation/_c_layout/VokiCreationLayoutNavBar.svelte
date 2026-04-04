<script lang="ts">
	import type { Snippet } from 'svelte';

	type NavBarLink = {
		icon: Snippet;
		name: string;
		href: string;
	};
	interface Props {
		links: NavBarLink[];
		onBeforeNavigate?: (targetHref: string) => boolean;
	}
	const { links, onBeforeNavigate }: Props = $props();

	function handleLinkClick(event: MouseEvent, href: string) {
		// Allow middle mouse button click (open in new tab)
		if (event.button === 1) {
			return;
		}

		// Only intercept left mouse button clicks
		if (event.button === 0 && onBeforeNavigate) {
			const shouldNavigate = onBeforeNavigate(href);
			if (!shouldNavigate) {
				event.preventDefault();
			}
		}
	}
</script>

<div class="nav-bar">
	{#each links as link}
		<a
			href={link.href}
			data-sveltekit-preload-data="off"
			onmousedown={(e) => handleLinkClick(e, link.href)}
		>
			{@render link.icon()}
			{link.name}
		</a>
	{/each}
</div>

<style>
	.nav-bar {
		display: flex;
		flex-direction: row;
		justify-content: center;
		gap: 2.5rem;
		width: 100%;
		margin-bottom: 1rem;
	}

	.nav-bar a {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.375rem;
		padding: 0.25rem 1.25rem;
		border-radius: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.nav-bar a:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.nav-bar a :global(svg) {
		height: 1.375rem;
		stroke-width: 2;
	}
</style>
