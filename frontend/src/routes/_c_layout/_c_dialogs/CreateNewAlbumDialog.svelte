<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import SignInRequiredToUseAlbums from '$lib/components/SignInRequiredToUseAlbums.svelte';
	import CreateNewAlbumContent from './_c_create_album_dialog/CreateNewAlbumContent.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	let onAfterSave: (newAlbumId: string) => void = $state(() => {});
	export function open(onAfterNewAlbumCreated: (newAlbumId: string) => void) {
		onAfterSave = (id) => {
			dialog.close();
			onAfterNewAlbumCreated(id);
		};
		dialog.open();
		if(inputsContent){
			inputsContent.reset();
		}
	}
	let inputsContent = $state<CreateNewAlbumContent>();
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="album-creation-dialog">
	<AuthView>
		{#snippet children(authState)}
			{#if authState.name === 'loading'}{:else if authState.isAuthenticated}
				<CreateNewAlbumContent bind:this={inputsContent} {onAfterSave} />
			{:else}
				<SignInRequiredToUseAlbums closeDialog={dialog.close} />
			{/if}
		{/snippet}
	</AuthView>
</DialogWithCloseButton>
