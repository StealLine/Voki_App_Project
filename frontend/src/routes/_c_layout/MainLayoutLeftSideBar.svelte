<script lang="ts">
	import AlbumsLink from './_c_side_bar/_c_links/AlbumsLink.svelte';
	import AuthorsLink from './_c_side_bar/_c_links/AuthorsLink.svelte';
	import CatalogLink from './_c_side_bar/_c_links/CatalogLink.svelte';
	import MyVokisLink from './_c_side_bar/_c_links/MyVokisLink.svelte';
	import NotificationsLink from './_c_side_bar/_c_links/NotificationsLink.svelte';
	import ProfileLink from './_c_side_bar/_c_links/ProfileLink.svelte';
	let isCollapsed = $state(false);
</script>

<div class="links">
	<ProfileLink {isCollapsed} />
	<CatalogLink {isCollapsed} />
	<AuthorsLink {isCollapsed} />
	<MyVokisLink {isCollapsed} />
	<AlbumsLink {isCollapsed} />
	<NotificationsLink {isCollapsed} />
</div>

<style>
	.links {
		display: flex;
		flex-direction: column;
		justify-content: center;
		gap: 0.5rem;
		width: var(--sidebar-width);
		background-color: var(--back);
	}
</style>
