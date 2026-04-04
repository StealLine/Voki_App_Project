<script lang="ts">
	import PublishingPageComponent from '../../../_c_shared/_c_shared_pages/PublishingPageComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import { GeneralVokiCreationPublishingPageState } from './general-voki-creation-publishing-page-state.svelte';
	import { setVokiCreationCurrentPageState } from '../../../voki-creation-page-context';

	let { data }: PageProps = $props();
	setVokiCreationCurrentPageState(new GeneralVokiCreationPublishingPageState());
</script>

{#if !data.response.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId} errs={data.response.errs} />
{:else}
	<PublishingPageComponent
		vokiId={data.vokiId}
		primaryAuthorId={data.response.data.primaryAuthorId}
		coAuthorIds={data.response.data.coAuthorIds}
		userIdsToBecomeManagers={data.response.data.userIdsToBecomeManagers}
		initialIssues={data.response.data.issues}
	/>
{/if}
