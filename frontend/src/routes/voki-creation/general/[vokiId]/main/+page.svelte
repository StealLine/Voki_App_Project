<script lang="ts">
	import MainInfoPageComponent from '../../../_c_shared/_c_shared_pages/MainInfoPageComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiCreationInteractionSettingsSection from './_c_page/GeneralVokiCreationInteractionSettingsSection.svelte';
	import {
		setVokiCreationCurrentPageState,
		setVokiCreationCurrentPageStateAsUnableToLoad
	} from '../../../voki-creation-page-context';
	import { GeneralVokiCreationMainPageState } from './general-voki-creation-main-page-state.svelte';

	let { data }: PageProps = $props();
	let pageState: GeneralVokiCreationMainPageState | undefined = $state(undefined);
	if (data.isSuccess) {
		pageState = new GeneralVokiCreationMainPageState(
			data.data.name,
			data.data.cover,
			data.data.tags,
			data.data.details,
			data.data.interactionSettings
		);
		// svelte-ignore state_referenced_locally
		setVokiCreationCurrentPageState(pageState);
	} else {
		setVokiCreationCurrentPageStateAsUnableToLoad();
	}
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else if pageState}
	<MainInfoPageComponent {pageState} vokiId={data.vokiId!}>
		{#snippet interactionSettingsSection()}
			<GeneralVokiCreationInteractionSettingsSection
				savedInteractionSettings={pageState.savedInteractionSettings}
				bind:isEditing={pageState.isInteractionSettingsEditing}
				updateSavedInteractionSettings={(newSettings) => {
					pageState.savedInteractionSettings = newSettings;
				}}
				vokiId={data.vokiId!}
			/>
		{/snippet}
	</MainInfoPageComponent>
{:else}
	<VokiCreationPageLoadingErr
		vokiId={data.vokiId!}
		errs={[{ message: 'Could not initialize page state' }]}
	/>
{/if}
