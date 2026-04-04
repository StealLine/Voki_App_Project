<script lang="ts">
	import VokiCreationBasicHeader from '../../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../types';
	import QuestionContentEditingState from './_c_content_section/QuestionContentEditingState.svelte';
	import type { QuestionPageResultsState } from '../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewState from './_c_content_section/QuestionContentViewState.svelte';

	interface Props {
		savedTypeSpecificContent: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		updateSavedTypeSpecificContent: (
			newTypeSpecificContent: GeneralVokiCreationQuestionContent
		) => void;
		isEditing: boolean;
		resultsIdToName: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		maxAnswersForQuestionCount: number;
		fetchResultNames: () => void;
	}
	let {
		savedTypeSpecificContent,
		questionId,
		vokiId,
		updateSavedTypeSpecificContent,
		isEditing = $bindable(),
		resultsIdToName,
		maxResultsForAnswerCount,
		maxAnswersForQuestionCount,
		fetchResultNames
	}: Props = $props();
	function startEditing() {
		isEditing = true;
	}
	function cancelEditing() {
		isEditing = false;
	}
	function updateParentOnSave(newContent: GeneralVokiCreationQuestionContent) {
		savedTypeSpecificContent = newContent;
		updateSavedTypeSpecificContent(newContent);
		isEditing = false;
	}
</script>

<VokiCreationBasicHeader header="{isEditing ? '*' : ''}Question type specific content" />
{#if isEditing}
	<QuestionContentEditingState
		savedContent={savedTypeSpecificContent}
		{questionId}
		{vokiId}
		{cancelEditing}
		{updateParentOnSave}
		{maxAnswersForQuestionCount}
		resultsIdToNameState={resultsIdToName}
		{maxResultsForAnswerCount}
		{fetchResultNames}
	/>
{:else}
	<QuestionContentViewState content={savedTypeSpecificContent} {startEditing} {resultsIdToName} />
{/if}
