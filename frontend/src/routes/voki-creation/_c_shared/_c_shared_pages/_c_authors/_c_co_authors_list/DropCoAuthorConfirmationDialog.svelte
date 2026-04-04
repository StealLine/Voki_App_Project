<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { toast } from 'svelte-sonner';
	import { getConfirmActionDialogOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/confirm-action-dialog-context';
	import type { Err } from '$lib/ts/err';

	interface Props {
		vokiId: string;
		onSuccessfulDrop: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}
	let { vokiId, onSuccessfulDrop }: Props = $props();

	const dialog = getConfirmActionDialogOpenFunction();
	export function open(userId: string) {
		currentUserToDrop = userId;
		const dropCoAuthor = async (): Promise<Err[]> => {
			const response = await ApiVokiCreationCore.fetchJsonResponse<{
				coAuthorIds: string[];
				invitedForCoAuthorUserIds: string[];
			}>(`/vokis/${vokiId}/drop-co-author`, RJO.DELETE({ userId }));
			if (response.isSuccess) {
				onSuccessfulDrop(response.data.coAuthorIds, response.data.invitedForCoAuthorUserIds);
				toast.success('Co-author dropped');
				dialog.close();
				return [];
			} else {
				return response.errs;
			}
		};
		dialog.open({
			mainContent: dropCoAuthorSnippet,
			dialogButtons: {
				confirmBtnText: 'Drop',
				confirmBtnOnclick: () => {
					return dropCoAuthor();
				},
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: dialog.close
			}
		});
	}
	let currentUserToDrop: string | undefined = $state();
</script>

{#snippet dropCoAuthorSnippet()}
	{#if currentUserToDrop}
		<div class="main-text">
			Are you sure you want to drop <BasicUserDisplay
				userId={currentUserToDrop}
				interactionLevel={'UniqueNameGotoOnClick'}
			/> from co-authors?
		</div>
	{:else}
		<p>No co-author to drop selected</p>
	{/if}
{/snippet}

<style>
	.main-text {
		margin: 1rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 475;
		line-height: 1.375;
		text-align: justify;
		text-indent: 1em;
		text-wrap: pretty;
	}

	.main-text > :global(.user-display) {
		display: inline-grid;
		vertical-align: middle;

		--profile-pic-width: 2.375rem;

		margin: 0.125rem 0.25rem;
	}
</style>
