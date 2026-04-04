<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	interface Props {
		src: string;
		maxWidth?: number;
		maxHeight?: number;
	}
	let { src, maxWidth = 28, maxHeight = 16 }: Props = $props();
</script>

<img
	src={StorageBucketMain.fileSrcWithVersion(src)}
	style="max-width: {maxWidth}rem; max-height: {maxHeight}rem"
/>

<style>
	img {
		border-radius: 0.5rem;
		object-fit: contain;
		box-shadow: var(--shadow-xs);
	}
</style>
