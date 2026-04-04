<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import AboutTabDescriptionSection from './_c_about_tab/AboutTabDescriptionView.svelte';
	import AboutTabTagsSection from './_c_about_tab/AboutTabTagsList.svelte';
	import VokiPageTabSectionLabel from './_c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import AboutTabManagersSection from './_c_about_tab/AboutTabManagersSection.svelte';
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import { LanguageUtils, type Language } from '$lib/ts/language';
	import type { VokiTypeWithSpecificData } from '../../types';
	import AboutTabTypeSection from './_c_about_tab/AboutTabTypeSection.svelte';

	interface Props {
		vokiId: string;
		primaryAuthorId: string;
		coAuthorIds: string[];
		managerIds: string[];
		tags: string[];
		description: string;
		publicationDate: Date;
		language: Language;
		hasMatureContent: boolean;
		signedInOnlyTaking: boolean;
		typeWithData: VokiTypeWithSpecificData;
	}
	let {
		vokiId,
		primaryAuthorId,
		coAuthorIds,
		managerIds,
		tags,
		description,
		publicationDate,
		language,
		hasMatureContent,
		signedInOnlyTaking,
		typeWithData
	}: Props = $props();
</script>

<div class="about-tab">
	<AboutTabTypeSection {typeWithData} />
	<div class="field-line language">
		<VokiPageTabSectionLabel fieldName="Access:" />
		<span class="value"
			>{signedInOnlyTaking ? 'Signed-in users only' : ' Available for anyone'}</span
		>
	</div>
	<div class="field-line language">
		<VokiPageTabSectionLabel fieldName="Language:" />
		<div class="language-value">
			<label class="value">{LanguageUtils.name(language)}</label>
			<svg><use href={LanguageUtils.icon(language)} /></svg>
		</div>
	</div>
	<AboutTabTagsSection {vokiId} {tags} />
	<AboutTabDescriptionSection {description} />
	<div class="field-line">
		<VokiPageTabSectionLabel fieldName="Content notice:" />
		<span class="value"
			>{hasMatureContent ? 'Voki contains mature content' : 'No mature content'}</span
		>
	</div>
	<div class="field-line publication-date">
		<VokiPageTabSectionLabel fieldName="Published:" /><span
			class="value"
			use:relativeTime={{
				date: publicationDate
			}}
		/>
		<label class="exact">({DateUtils.toLocaleDateOnly(publicationDate)})</label>
	</div>
	<AboutTabManagersSection {managerIds} {coAuthorIds} {primaryAuthorId} />
</div>

<style>
	.about-tab {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.field-line {
		margin: 0;
	}

	.field-line .value {
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 500;
	}

	.language-value {
		display: inline-flex;
		vertical-align: middle;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
	}

	.language-value > svg {
		height: 1.25rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.375rem;
		stroke-width: 1.9;
	}

	.publication-date > .exact {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 400;
	}
</style>
