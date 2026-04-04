<script lang="ts">
	import { LanguageUtils, type Language } from '$lib/ts/language';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';
	import type { PossiblyHidden } from '../../types';

	interface Props {
		knownLanguages: Extract<PossiblyHidden<Language[]>, { showOnProfile: true }>;
	}

	let { knownLanguages }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Known languages">
	<div class="languages-container">
		{#each knownLanguages.value as lang}
			<div class="language">
				<svg><use href={LanguageUtils.icon(lang)} /></svg>
				{LanguageUtils.name(lang)}
			</div>
		{/each}
	</div>
</ProfileSidebarSectionContainer>

<style>
	.languages-container {
		display: flex;
		flex-wrap: wrap;
		gap: 0.75rem;
	}
	.language {
		display: grid;
		justify-content: center;
		align-items: center;
		gap: 0.375rem;
		width: auto;
		padding: 0.25rem 0.75rem;
		border-radius: 0.5rem;
		background-color: var(--back);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 500;
		grid-template-columns: auto 1fr;
		box-shadow: var(--shadow), var(--shadow-xs);
		cursor: default;
	}

	.language > svg {
		height: 1.25rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.25rem;
	}
</style>
