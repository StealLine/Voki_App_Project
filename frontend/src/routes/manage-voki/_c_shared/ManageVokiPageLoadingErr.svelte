<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
</script>

<div class="container">
	<h1>Unable to load page data</h1>
	<h1>Voki id: {vokiId}</h1>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		width: 60rem;
		margin: 4rem auto;
	}
</style>
