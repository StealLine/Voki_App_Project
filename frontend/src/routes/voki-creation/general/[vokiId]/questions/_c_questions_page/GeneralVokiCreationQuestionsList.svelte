<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { toast } from 'svelte-sonner';
	import type { QuestionBriefInfo } from '../types';
	import GeneralVokiCreationQuestionItem from './GeneralVokiCreationQuestionItem.svelte';
	import VokiCreationBasicHeader from '../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import { getConfirmActionDialogOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/confirm-action-dialog-context';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	interface Props {
		questionsProps: QuestionBriefInfo[];
		vokiId: string;
	}
	let { questionsProps, vokiId }: Props = $props();
	let questions = $state(questionsProps);
	async function moveQuestionUpInOrder(questionId: string) {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			questions: QuestionBriefInfo[];
		}>(`/vokis/${vokiId}/questions/${questionId}/move-up-in-order`, RJO.PATCH({}));
		if (response.isSuccess) {
			questions = response.data.questions;
		} else {
			toast.error(response.errs[0].message);
		}
	}
	async function moveQuestionDownInOrder(questionId: string) {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			questions: QuestionBriefInfo[];
		}>(`/vokis/${vokiId}/questions/${questionId}/move-down-in-order`, RJO.PATCH({}));
		if (response.isSuccess) {
			questions = response.data.questions;
		} else {
			toast.error(response.errs[0].message);
		}
	}
	const { open: openConfirmationDialog, close: closeConfirmationDialog } =
		getConfirmActionDialogOpenFunction();

	function openQuestionDeleteConfirmationDialog(questionId: string) {
		const deleteQuestion = async (questionId: string) => {
			const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
				questions: QuestionBriefInfo[];
			}>(`/vokis/${vokiId}/questions/${questionId}/delete`, RJO.DELETE({}));
			if (response.isSuccess) {
				questions = response.data.questions;
				return [];
			} else {
				return response.errs;
			}
		};
		const q = questions.find((q) => q.id === questionId)!;
		const questionPreview = q.text.length > 40 ? q.text.slice(0, 30) + '...' : q.text;
		openConfirmationDialog({
			mainContent: {
				mainText: `Are you sure you want to delete the "${questionPreview}" question?`,
				subheading: undefined
			},
			dialogButtons: {
				confirmBtnText: 'Delete',
				confirmBtnOnclick: () => deleteQuestion(questionId),
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: closeConfirmationDialog
			}
		});
	}
</script>

<VokiCreationBasicHeader header={`Voki questions (${questions.length})`} />
<div class="questions">
	{#each questions as question}
		<GeneralVokiCreationQuestionItem
			{vokiId}
			{question}
			questionsCount={questions.length}
			moveQuestionUpInOrder={() => {
				moveQuestionUpInOrder(question.id);
			}}
			moveQuestionDownInOrder={() => {
				moveQuestionDownInOrder(question.id);
			}}
			deleteQuestion={() => {
				openQuestionDeleteConfirmationDialog(question.id);
			}}
		/>
	{/each}
</div>

<style>
	.questions {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
	}
</style>
