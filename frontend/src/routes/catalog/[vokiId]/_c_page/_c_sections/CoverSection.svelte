<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiType } from '$lib/ts/voki-type';
	import CoverSectionAddToAlbumBtn from './_c_cover_section/CoverSectionAddToAlbumBtn.svelte';
	import CoverSectionManageVokiBtn from './_c_cover_section/CoverSectionManageVokiBtn.svelte';
	import CoverSectionTakeVokiBtn from './_c_cover_section/CoverSectionTakeVokiBtn.svelte';

	interface Props {
		vokiId: string;
		cover: string;
		vokiType: VokiType;
		signedInOnlyTaking: boolean;
		canUserManageVoki: (userId: string) => boolean;
	}
	let { vokiId, cover, vokiType, signedInOnlyTaking, canUserManageVoki }: Props = $props();
</script>

<div class="voki-cover-section">
	<img class="voki-cover" src={StorageBucketMain.fileSrc(cover)} alt="voki cover" />
	<div class="buttons-container">
		<CoverSectionTakeVokiBtn {vokiId} {vokiType} {signedInOnlyTaking} />
		<CoverSectionAddToAlbumBtn {vokiId} />
		<AuthView>
			{#snippet children(authState)}
				{#if authState.name === 'authenticated' && canUserManageVoki(authState.userId)}
					<CoverSectionManageVokiBtn {vokiId} {vokiType} />
				{/if}
			{/snippet}
		</AuthView>
	</div>
</div>

<style>
	.voki-cover-section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		align-self: flex-start;
	}

	.voki-cover-section > img {
		width: 25rem;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}

	.buttons-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}

	.buttons-container > :global(button),
	.buttons-container > :global(a) {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		transition: transform 0.08s ease-out;
		cursor: default;
		outline: none;
	}

	.buttons-container > :global(button:focus),
	.buttons-container > :global(a:focus) {
		transform: scale(1.02);
	}

	.buttons-container > :global(a > .icon-item:nth-child(1)),
	.buttons-container > :global(button > .icon-item:nth-child(1)) {
		width: 1.75rem;
		height: 1.75rem;
		margin-right: 0.5rem;
		stroke-width: 2;
	}

	.buttons-container > :global(a > .btn-text),
	.buttons-container > :global(button > .btn-text) {
		width: 8.5rem;
		font-size: 1.25rem;
		font-weight: 450;
		text-align: start;
		letter-spacing: 0.2px;
		cursor: inherit;
	}
</style>
