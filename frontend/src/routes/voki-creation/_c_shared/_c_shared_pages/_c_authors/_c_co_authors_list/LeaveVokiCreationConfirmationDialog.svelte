<script lang="ts">
	import { goto } from '$app/navigation';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { getConfirmActionDialogOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/confirm-action-dialog-context';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();

	const dialog = getConfirmActionDialogOpenFunction();
	export function open() {
		const leaveVokiCreation = async (): Promise<Err[]> => {
			const response = await ApiVokiCreationCore.fetchVoidResponse(
				`/vokis/${vokiId}/leave-voki-creation`,
				RJO.DELETE({})
			);
			if (response.isSuccess) {
				goto('/my-vokis/draft');
				dialog.close();
				return [];
			} else {
				return response.errs;
			}
		};
		dialog.open({
			mainContent: {
				mainText: 'Are you sure you want to leave the creation of this Voki? You will no longer be considered as its co-author',
				subheading: "You won't be able to undo this action."
			},
			dialogButtons: {
				confirmBtnText: 'Leave',
				confirmBtnOnclick: () => {
					return leaveVokiCreation();
				},
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: dialog.close
			}
		});
	}
</script>
