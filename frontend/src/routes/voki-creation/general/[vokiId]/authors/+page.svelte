<script lang="ts">
	import AuthorsPageComponent from '../../../_c_shared/_c_shared_pages/AuthorsPageComponent.svelte';
	import { CoAuthorsPageState } from '../../../_c_shared/_c_shared_pages/_c_authors/co-authors-page-state.svelte';

	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import {
		setVokiCreationCurrentPageState,
		setVokiCreationCurrentPageStateAsUnableToLoad
	} from '../../../voki-creation-page-context';

	let { data }: PageProps = $props();

	let pageState: CoAuthorsPageState | undefined = $state(undefined);
	if (data.isSuccess) {
		const d = data.data;
		pageState = new CoAuthorsPageState(
			data.vokiId!,
			d.primaryAuthorId,
			d.coAuthorIds,
			d.invitedForCoAuthorUserIds,
			d.maxVokiCoAuthors,
			d.expectedManagers
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
	<AuthorsPageComponent {pageState} vokiCreationDate={data.data.vokiCreationDate} />
{:else}
	<VokiCreationPageLoadingErr
		vokiId={data.vokiId!}
		errs={[{ message: 'Could not initialize page state' }]}
	/>
{/if}
