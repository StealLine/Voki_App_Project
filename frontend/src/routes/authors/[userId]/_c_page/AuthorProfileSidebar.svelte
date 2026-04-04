<script lang="ts">
	import type { Language } from '$lib/ts/language';
	import type { AuthorLink, PossiblyHidden } from '../types';
	import SidebarAboutMeSection from './_c_sidebar/SidebarAboutMeSection.svelte';
	import SidebarLanguagesSection from './_c_sidebar/SidebarLanguagesSection.svelte';
	import SidebarProfileLinksSection from './_c_sidebar/SidebarProfileLinksSection.svelte';
	import SidebarFavouriteTagsSection from './_c_sidebar/SidebarFavouriteTagsSection.svelte';
	import SidebarFavouriteAuthorsSection from './_c_sidebar/SidebarFavouriteAuthorsSection.svelte';

	interface Props {
		aboutMe: PossiblyHidden<string>;
		knownLanguages: PossiblyHidden<Language[]>;
		links: PossiblyHidden<AuthorLink[]>;
		favouriteTags: PossiblyHidden<string[]>;
		favouriteAuthorIds: PossiblyHidden<string[]>;
	}

	let { aboutMe, knownLanguages, links, favouriteTags, favouriteAuthorIds }: Props = $props();
</script>

<div class="author-sidebar">
	{#if aboutMe.showOnProfile}
		<SidebarAboutMeSection {aboutMe} />
	{/if}
	{#if knownLanguages.showOnProfile}
		<SidebarLanguagesSection {knownLanguages} />
	{/if}
	{#if links.showOnProfile}
		<SidebarProfileLinksSection {links} />
	{/if}
	{#if favouriteTags.showOnProfile}
		<SidebarFavouriteTagsSection {favouriteTags} />
	{/if}
	{#if favouriteAuthorIds.showOnProfile}
		<SidebarFavouriteAuthorsSection {favouriteAuthorIds} />
	{/if}
</div>

<style>
	.author-sidebar {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}
</style>
