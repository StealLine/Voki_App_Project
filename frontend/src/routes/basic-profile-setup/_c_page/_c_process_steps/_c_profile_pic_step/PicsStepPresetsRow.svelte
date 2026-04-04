<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	type Props = {
		picsArr: string[];
		rowName: string;
		onPicClick: (pic: string) => void;
	};
	let { picsArr, rowName, onPicClick }: Props = $props();
</script>

<div class="predefined-row">
	<div class="row-name">{rowName}</div>
	<div class="pics-container">
		{#each picsArr.slice(0, 4) as pic}
			<img
				class="img unselectable"
				onclick={() => onPicClick(pic)}
				src={StorageBucketMain.fileSrc(pic)}
				alt="Current profile pic"
			/>
		{/each}
	</div>
</div>

<style>
	.predefined-row {
		display: grid;
		gap: 0.25rem;
		width: fit-content;
		height: fit-content;
		grid-template-rows: auto 1fr;
	}

	.row-name {
		margin-left: 2rem;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.pics-container {
		display: grid;
		grid-template-columns: repeat(4, 1fr);
		gap: 0.75rem;
	}

	.img {
		display: grid;
		width: 100%;
		border-radius: 99rem;
		box-shadow: var(--shadow-xs), var(--shadow);
		transition: all 0.12s ease-out;
		cursor: pointer;
		aspect-ratio: 1/1;
		object-fit: cover;
		place-content: center;
	}

	.img:hover {
		box-shadow: var(--shadow-xs), var(--shadow-lg);
		transform: scale(1.08);
	}
</style>
