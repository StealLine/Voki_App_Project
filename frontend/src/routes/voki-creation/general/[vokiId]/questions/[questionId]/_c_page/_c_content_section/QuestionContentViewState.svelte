<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';
	import type { QuestionPageResultsState } from '../../general-voki-creation-specific-question-page-state.svelte';
	import IncorrectContentTypeMessage from './_c_shared/IncorrectContentTypeMessage.svelte';
	import QuestionTextOnlyContentView from './_c_view/QuestionTextOnlyContentView.svelte';
	import QuestionImageOnlyContentView from './_c_view/QuestionImageOnlyContentView.svelte';
	import QuestionImageAndTextContentView from './_c_view/QuestionImageAndTextContentView.svelte';
	import QuestionColorOnlyContentView from './_c_view/QuestionColorOnlyContentView.svelte';
	import QuestionColorAndTextContentView from './_c_view/QuestionColorAndTextContentView.svelte';
	import QuestionAudioAndTextContentView from './_c_view/QuestionAudioAndTextContentView.svelte';
	import QuestionAudioOnlyContentView from './_c_view/QuestionAudioOnlyContentView.svelte';

	interface Props {
		content: GeneralVokiCreationQuestionContent;
		startEditing: () => void;
		resultsIdToName: QuestionPageResultsState;
	}
	let { content, startEditing, resultsIdToName }: Props = $props();
</script>

{#if content.$type === 'TextOnly'}
	<QuestionTextOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ImageOnly'}
	<QuestionImageOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ImageAndText'}
	<QuestionImageAndTextContentView {content} {resultsIdToName} />
{:else if content.$type === 'ColorOnly'}
	<QuestionColorOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ColorAndText'}
	<QuestionColorAndTextContentView {content} {resultsIdToName} />
{:else if content.$type === 'AudioOnly'}
	<QuestionAudioOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'AudioAndText'}
	<QuestionAudioAndTextContentView {content} {resultsIdToName} />
{:else}
	<IncorrectContentTypeMessage type={(content as any).$type} />
{/if}

<VokiCreationDefaultButton text="Edit content" onclick={startEditing} />
