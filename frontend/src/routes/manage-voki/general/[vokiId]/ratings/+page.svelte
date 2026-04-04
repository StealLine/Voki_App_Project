<script lang="ts">
	import ManageVokiPageLoadingErr from '../../../_c_shared/ManageVokiPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import VokiRatingsTabContent from './_c_page/VokiRatingsTabContent.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<ManageVokiPageLoadingErr vokiId={data.vokiId} errs={data.response.errs} />
{:else}
	<VokiRatingsTabContent
		vokiId={data.vokiId}
		vokiPublicationDate={data.response.data.vokiPublicationDate}
		snapshots={data.response.data.snapshots}
	/>
{/if}
