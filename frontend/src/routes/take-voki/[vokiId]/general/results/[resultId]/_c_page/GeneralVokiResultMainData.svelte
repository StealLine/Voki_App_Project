<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	interface Props {
		image: string | null;
		name: string;
		text: string;
	}
	let { image, name, text }: Props = $props();
</script>

{#if image}
	<img
		class="result-image"
		src={StorageBucketMain.fileSrc(image)}
		alt={`Result image for ${name}`}
	/>
{/if}

<h1 class="result-name">{name}</h1>
<p class="result-text">{text}</p>

<style>
	.result-image {
		max-width: 55rem;
		max-height: 30rem;
		margin: 0 auto;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs);
		object-fit: contain;
	}

	.result-name {
		width: auto;
		height: auto;
		margin: 0;
		color: var(--primary);
		font-size: 1.75rem;
		font-weight: 600;
	}

	.result-text {
		display: block;
		width: 100%;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 450;
		line-height: 1.1;
		text-align: justify;
		text-indent: 1rem;
	}
</style>
