<script lang="ts">
	import { page } from '$app/state';
	import type { Snippet } from 'svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';

	let { children }: { children: Snippet } = $props();
</script>

{#if page.data.response.isSuccess}
	{@render children()}
{:else}
	<div class="msg-container">
		<h1>Could not load your {page.data.albumName} auto album</h1>
		<a href="/albums" class="go-back-link">Go back to albums</a>
		<DefaultErrBlock errList={page.data.response.errs} />
	</div>
{/if}

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		margin: 6rem auto 4rem;
	}

	.msg-container > h1 {
		color: var(--muted-foreground);
		font-size: 2.25rem;
		font-weight: 600;
		text-align: center;
		letter-spacing: 0.5px;
		text-wrap: pretty;
	}

	.go-back-link {
		padding: 0.25rem 1.5rem;
		margin: 1.5rem 0 3rem;
		border-radius: 5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		letter-spacing: 0.125px;
		transition: all 0.06s ease-in;
	}

	.go-back-link:hover,
	.go-back-link:focus,
	.go-back-link:active {
		padding: 0.25rem 1.675rem;
		background-color: var(--primary-hov);
		letter-spacing: 0.4px;
	}

	.msg-container > :global(.err-block) {
		max-width: 60vw;
	}
</style>
