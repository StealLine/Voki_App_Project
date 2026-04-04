<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiType } from '$lib/ts/voki-type';
	import GoToVokiCreationLink from '../../../../_c_shared/GoToVokiCreationLink.svelte';

	interface Props {
		vokiType: VokiType;
		vokiId: string;
		vokiName: string;
		vokiCover: string;
		closeDialog: () => void;
	}

	let { vokiType, vokiId, vokiName, vokiCover, closeDialog }: Props = $props();
</script>

<div class="confirmed-content">
	<img class="cover" src={StorageBucketMain.fileSrc(vokiCover)} />
	<p class="main-text">You now a co-author of <span class="voki-name">{vokiName}</span> Voki</p>
</div>
<div class="buttons unselectable">
	<GoToVokiCreationLink {vokiId} {vokiType} />
	<button class="close-btn" onclick={() => closeDialog()}>Close</button>
</div>

<style>
	.confirmed-content {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		width: 100%;
		margin: auto 0;
	}

	.cover {
		min-height: 10rem;
		max-height: 14rem;
		border-radius: var(--voki-cover-border-radius);
		box-shadow: var(--shadow-xs);
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.main-text {
		max-height: 20rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 475;
		line-height: 1.375;
		text-align: justify;
		text-indent: 1em;
		overflow-wrap: anywhere;
		text-wrap: pretty;
		overflow-y: auto;
	}

	.main-text > .voki-name {
		margin-bottom: 1rem;
		border-radius: 0.5rem;
		color: var(--muted-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		text-decoration: underline;
		text-indent: 0;
	}

	.buttons {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		margin-top: auto;
	}

	.close-btn {
		width: fit-content;
		padding: 0.25rem 2rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		cursor: pointer;
	}

	.close-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
