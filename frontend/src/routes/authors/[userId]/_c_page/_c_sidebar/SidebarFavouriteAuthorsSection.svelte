<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		favouriteAuthorIds: Extract<PossiblyHidden<string[]>, { showOnProfile: true }>;
	}

	let { favouriteAuthorIds }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Favourite authors">
	<div class="authors-container">
		{#each favouriteAuthorIds.value as authorId}
			<BasicUserDisplay userId={authorId} interactionLevel="WholeComponentLink" />
		{/each}
	</div>
</ProfileSidebarSectionContainer>

<style>
	.authors-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
</style>
