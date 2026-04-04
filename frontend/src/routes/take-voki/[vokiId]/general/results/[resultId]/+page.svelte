<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import GeneralVokiResultPagesHeader from '../_c_shared/GeneralVokiResultPagesHeader.svelte';
	import GeneralVokiResultPagesVokiNameSpan from '../_c_shared/GeneralVokiResultPagesVokiNameSpan.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiResultMainData from './_c_page/GeneralVokiResultMainData.svelte';
	import GeneralVokiResultViewActions from './_c_page/GeneralVokiResultViewActions.svelte';
	import VokiNotTakenSpoilerWrapper from '../_c_shared/VokiNotTakenSpoilerWrapper.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<VokiNotTakenSpoilerWrapper
		hasUserTaken={data.response.data.hasUserTakenThisVoki}
		vokiId={data.vokiId!}
		text="You have not taken this Voki yet. The result can spoiler the content of the Voki for you. Are you sure you want to see it?"
	>
		<GeneralVokiResultPagesHeader>
			Result of the
			<GeneralVokiResultPagesVokiNameSpan vokiName={data.response.data.vokiName} />
			general Voki
		</GeneralVokiResultPagesHeader>
		<div class="view-result">
			<GeneralVokiResultMainData
				image={data.response.data.image}
				name={data.response.data.name}
				text={data.response.data.text}
			/>
			<GeneralVokiResultViewActions
				resultsCount={data.response.data.resultsCount}
				resultsVisibility={data.response.data.resultsVisibility}
				vokiId={data.vokiId!}
			/>
		</div>
	</VokiNotTakenSpoilerWrapper>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load voki result "
		errs={data.response.errs}
		additionalParams={[
			{ name: 'vokiId', value: data.vokiId },
			{ name: 'resultId', value: data.resultId }
		]}
	/>
{/if}

<style>
	.view-result {
		display: flex;
		flex-direction: column;
		gap: 1.5rem;
		width: calc(100% - 10rem);
		min-width: 50rem;
		padding: 2rem;
		margin: 2rem auto;
		border-radius: 1.5rem;
		background: var(--back);
		text-align: center;
		box-shadow: var(--shadow-md), var(--shadow-xs);
	}
</style>
