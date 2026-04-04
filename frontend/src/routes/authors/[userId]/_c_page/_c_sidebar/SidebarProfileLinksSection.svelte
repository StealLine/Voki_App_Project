<script lang="ts">
	import { IconUtils } from '$lib/ts/utils/icons-utils';
	import type { PossiblyHidden, AuthorLink } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';

	interface Props {
		links: Extract<PossiblyHidden<AuthorLink[]>, { showOnProfile: true }>;
	}
	let { links }: Props = $props();

	function getDisplayText(url: string): string {
		try {
			const parsedUrl = new URL(url);
			return parsedUrl.hostname.replace('www.', '');
		} catch {
			return url;
		}
	}
</script>

<ProfileSidebarSectionContainer title="Links">
	<div class="links-list">
		{#each links.value as link}
			<a class="link" href={link.value} target="_blank" rel="noopener noreferrer">
				<svg class="link-icon"><use href={IconUtils.getProfileLinkIconIdByType(link.type)} /></svg>
				<span class="link-text">{getDisplayText(link.value)}</span>
			</a>
		{/each}
	</div>
</ProfileSidebarSectionContainer>

<style>
	.links-list {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.link {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		padding: 0.375rem 0.5rem;
		border-radius: 0.5rem;
		color: var(--text);
		text-decoration: none;
		transition: all 0.2s ease;
		background-color: transparent;
		position: relative;
		overflow: hidden;
	}

	.link:hover {
		background-color: var(--muted);
	}

	.link:active {
		background-color: var(--accent);
		color: var(--accent-foreground);
		transform: scale(0.98);
		transition: all 0.1s ease;
	}

	.link:focus-visible {
		outline: 0.125rem solid var(--primary);
		outline-offset: 0.125rem;
	}

	.link-icon {
		height: 1.5rem;
		width: 1.5rem;
		display: block;
		opacity: 1;
		flex-shrink: 0;
		transition: all 0.2s ease;
		stroke-width: 1.75;
	}

	.link-text {
		font-size: 1rem;
		font-weight: 500;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}
</style>
