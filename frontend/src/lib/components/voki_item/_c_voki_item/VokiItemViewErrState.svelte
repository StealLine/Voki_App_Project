<script lang="ts">
	import type { Err } from '$lib/ts/err';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
</script>

<div class="unable-to-load">
	<h1 class="unable-to-load-msg">Unable to load voki data</h1>
	{#if errs.find((err) => err.code === 23010)}
		<h1 class="voki-not-found">Voki not found</h1>
		<label>Voki id: {vokiId}</label>
	{:else}
		{#each errs as err}
			<label>{err.code}</label>
		{/each}
	{/if}
</div>

<style>
	.unable-to-load {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 100%;
		padding: 2rem 1rem;
		border-radius: 0.5rem;
		background-color: var(--red-1);
		color: var(--red-5);
		place-self: center center;
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.unable-to-load * {
		cursor: default;
	}

	.unable-to-load-msg {
		font-size: 1.125rem;
		font-weight: 450;
	}

	.voki-not-found {
		font-size: 2rem;
		font-weight: 500;
	}

	.unable-to-load label {
		margin-top: 0.5rem;
		font-size: 1rem;
		font-weight: 450;
	}
</style>
