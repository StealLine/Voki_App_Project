<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';
	import TagItemChip from '$lib/components/TagItemChip.svelte';

	interface Props {
		favouriteTags: Extract<PossiblyHidden<string[]>, { showOnProfile: true }>;
	}

	let { favouriteTags }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Favourite tags">
	<div class="tags-container">
		{#each favouriteTags.value as tag}
			<TagItemChip {tag} />
		{/each}
	</div>
</ProfileSidebarSectionContainer>

<style>
	.tags-container {
		display: flex;
		flex-wrap: wrap;
		gap: 0.5rem;
	}
</style>
