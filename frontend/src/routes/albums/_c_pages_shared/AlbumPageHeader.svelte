<script lang="ts">
	type AutoAlbumHeaderContent = { type: 'auto'; albumName: string };
	type UserAlbumHeaderContent = {
		type: 'user';
		icon: { href: string; mainColor: string; secondaryColor: string };
		albumName: string;
	};
	interface Props {
		content: AutoAlbumHeaderContent | UserAlbumHeaderContent;
	}
	let { content }: Props = $props();
</script>

<h1>
	<a href="/albums" class="back-link">
		<svg><use href="#caret-left-icon" /></svg>
	</a>
	{#if content.type === 'auto'}
		Your {content.albumName} auto album:
	{:else if content.type === 'user'}
		<svg
			class="album-icon"
			style="

			--icon-color-1: {content.icon.mainColor};
			--icon-color-2: {content.icon.secondaryColor};"
		>
			<use href="#{content.icon.href}" />
		</svg>
		<span class="album-name" style="color: {content.icon.mainColor};">{content.albumName}</span> album
	{:else}
		<span class="error">Could not determine album type</span>
	{/if}
</h1>

<style>
	h1 {
		display: flex;
		align-items: center;
		margin: 2rem 0 0;
		color: var(--text);
		font-size: 2rem;
		font-weight: 575;
	}

	.back-link {
		display: flex;
		justify-content: center;
		align-items: center;
		margin-top: 0.25rem;
		margin-right: 0.5rem;
		border-radius: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.back-link > svg {
		width: 1.5rem;
		height: 1.5rem;
		color: inherit;
		stroke-width: 2.2;
		transition: transform 0.15s ease-out;
	}

	.back-link:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.back-link:hover > svg {
		transform: scale(1.02) translateX(-1px);
	}

	.album-icon {
		width: 2rem;
		height: 2rem;
		stroke-width: 2;
		margin: 0.25rem 0.125rem 0 0.5rem;
	}

	.album-name {
		margin-right: 0.675rem;
		font-weight: 525;
		letter-spacing: 0.25px;
	}

	.error {
		color: var(--red-5);
		font-weight: 550;
	}
</style>
