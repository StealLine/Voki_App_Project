<script lang="ts">
	interface Props {
		links: {
			name: string;
			key: string;
			href: string;
		}[];
		activeLinkKey: string | undefined;
	}
	const { links, activeLinkKey }: Props = $props();
</script>

<div class="nav-bar">
	{#each links as link}
		<a
			href={link.href}
			data-sveltekit-preload-data="off"
			class="link"
			class:active={link.key === activeLinkKey}
		>
			{link.name}
		</a>
	{/each}
</div>

<style>
	.nav-bar {
		display: grid;
		gap: 1rem;
		width: 100%;
		padding: 0.25rem;
		margin: 1rem 0;
		border-radius: 100vw;
		box-shadow: var(--shadow-xs);
		grid-template-columns: repeat(auto-fit, minmax(0, 1fr));
	}

	.link {
		display: block;
		width: 100%;
		padding: 0.375rem 0;
		border-radius: 100vw;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-align: center;
		text-decoration: none;
	}

	.link:not(.active):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.link:not(.active):active {
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
	}

	.link.active {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
