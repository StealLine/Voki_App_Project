<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import type { GeneralVokiQuestionContentType } from '$lib/ts/voki';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import QuestionContentTypeSelectionCard from './_c_initializing_dialog/QuestionContentTypeSelectionCard.svelte';

	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let selectedType: GeneralVokiQuestionContentType = $state('TextOnly');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
	}
	async function submitCreate() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ id: string }>(
			`/vokis/${vokiId}/questions/add-new`,
			RJO.POST({ questionContentType: selectedType })
		);
		if (response.isSuccess) {
			goto(`/voki-creation/general/${vokiId}/questions/${response.data.id}`);
		} else {
			errs = response.errs;
		}
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-question-initializing-dialog"
	bind:this={dialog}
	subheading="Choose answers type of the new question"
>
	<div class="types-container">
		<div class="type-subset-column">
			<svg><use href="#text-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<QuestionContentTypeSelectionCard type="TextOnly" bind:selectedType />
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#color-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<QuestionContentTypeSelectionCard type="ColorOnly" bind:selectedType />
				<QuestionContentTypeSelectionCard type="ColorAndText" bind:selectedType />
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#image-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<QuestionContentTypeSelectionCard type="ImageOnly" bind:selectedType />
				<QuestionContentTypeSelectionCard type="ImageAndText" bind:selectedType />
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#audio-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<QuestionContentTypeSelectionCard type="AudioOnly" bind:selectedType />
				<QuestionContentTypeSelectionCard type="AudioAndText" bind:selectedType />
			</div>
		</div>
	</div>

	<DefaultErrBlock errList={errs} />

	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>

<style>
	.types-container {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr auto 1fr;
		gap: 1.25rem;
		padding: 0 2rem;
	}

	.columns-sep {
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--secondary);
	}

	.type-subset-column {
		display: grid;
		grid-template-rows: auto 1fr;
		gap: 2rem;
		justify-items: center;
	}

	.type-subset-column > svg {
		width: 3.25rem;
		height: 3.25rem;
		padding: 0.375rem;
		border: 0.125rem solid var(--primary);
		border-radius: 1.375rem;
		color: var(--primary);
		box-shadow: var(--shadow-md);
		stroke-width: 1.5;
	}

	.subset-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	:global(#general-voki-question-initializing-dialog .dialog-content) {
		display: flex;
		flex-direction: column;
	}

	:global(#general-voki-question-initializing-dialog .err-block) {
		margin: 1rem 0;
	}

	:global(#general-voki-question-initializing-dialog .primary-btn) {
		margin: 0 auto;
	}
</style>
