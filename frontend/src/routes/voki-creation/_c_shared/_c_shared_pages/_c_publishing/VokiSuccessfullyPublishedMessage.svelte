<script lang="ts">
	import BaseDialog from '$lib/components/dialogs/BaseDialog.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiSuccessfullyPublishedData } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiPublishedDialogConfettiIcon from './_c_successfully_published/VokiPublishedDialogConfettiIcon.svelte';
	interface Props {
		vokiData: VokiSuccessfullyPublishedData;
	}
	let { vokiData }: Props = $props();
</script>

<div class="voki-published-container">
	<h1 class="title">Congratulations <VokiPublishedDialogConfettiIcon /></h1>
	<img class="cover" src={StorageBucketMain.fileSrc(vokiData.cover)} />
	<p class="message">
		Voki
		<span>{vokiData.name}</span>
		has been successfully published
	</p>
	<a href={`/catalog/${vokiData.id}`} class="view-button">View published Voki page</a>
</div>

<style>
	.voki-published-container {
		display: flex;
		flex-direction: column;
		justify-items: center;
		width: fit-content;
		margin: 2rem auto;
		
	}

	.title {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		width: 100%;
		margin: 0;
		color: var(--primary);
		font-size: 2rem;
		font-weight: 500;
		letter-spacing: 0.25px;
	}

	.title > :global(svg) {
		width: 2.5rem;
		height: 2.5rem;
		margin-bottom: 0.25rem;
		color: var(--primary);
		stroke-width: 2;
	}

	.cover {
		width: 32rem;
		margin: 1rem auto 0;
		border-radius: 0.75rem;
		box-shadow: var(--shadow-xs);
		aspect-ratio: var(--voki-cover-aspect-ratio);

	}

	.message {
		margin: 1rem 0 2rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 450;
		letter-spacing: 0.25px;
	}

	.message > span {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 520;
		letter-spacing: 0.4px;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.view-button {
		padding: 0.25rem 1.25rem;
		margin: 0 auto;
		margin-top: auto;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.5rem;
		font-weight: 450;
		text-align: center;
		letter-spacing: 0.5px;
		transition: all 0.12s ease-in-out;
		cursor: pointer;
	}

	.view-button:hover {
		padding: 0.25rem 1.5rem;
		background-color: var(--primary-hov);
		letter-spacing: 1px;
	}
</style>
