<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import GeneralVokiUnfinishedSessionView from '$lib/components/voki_types_specific/general/GeneralVokiUnfinishedSessionView.svelte';
	import type { ExistingUnfinishedSessionForVokiData } from '$lib/ts/voki-taking-session';

	interface Props {
		takeVokiPageLink: string;
	}
	let { takeVokiPageLink }: Props = $props();
	export function open(data: ExistingUnfinishedSessionForVokiData) {
		sessionData = data;
		dialog.open();
	}
	let dialog: DialogWithCloseButton = $state()!;
	let sessionData = $state<ExistingUnfinishedSessionForVokiData>();
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="unfinished-session-exists-dialog">
	<p class="title">You have an unfinished session for this Voki</p>
	{#if sessionData}
		<GeneralVokiUnfinishedSessionView {sessionData} {takeVokiPageLink} replaceStateOnGoto={false} />
	{:else}
		<p>Something went wrong. Please reload the page</p>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#unfinished-session-exists-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.25rem;
		padding: 2rem 4rem;
		color: var(--text);
	}

	.title {
		margin: 0;
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 550;
		text-align: center;
	}
</style>
