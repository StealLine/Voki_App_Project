<script lang="ts">
	import type { Snippet } from 'svelte';
	import { onMount } from 'svelte';
	import { VokiIgnoreSpoilerCookie } from '$lib/ts/cookies/voki-ignore-spoiler-cookie';

	interface Props {
		vokiId: string;
		hasUserTaken: boolean;
		text: string;
		children: Snippet;
	}
	let { vokiId, hasUserTaken, text, children }: Props = $props();

	let spoilerIgnored = $state(false);
	let showVokiNotTaken = $derived(!(hasUserTaken || spoilerIgnored));

	onMount(() => {
		if (VokiIgnoreSpoilerCookie.isIgnored(vokiId)) {
			spoilerIgnored = true;
		}
	});

	function handleIgnoreSpoiler() {
		spoilerIgnored = true;
		VokiIgnoreSpoilerCookie.markAsIgnoredFor12Hours(vokiId);
	}

	const catalogHref = $derived(`/catalog/voki/${vokiId}`);
</script>

{#if showVokiNotTaken}
	<div>
		<div class="content appear-with-delay">
			<div class="icon" aria-hidden="true">
				<svg viewBox="0 0 24 24">
					<use href="#common-warning-icon" />
				</svg>
			</div>

			<h2 class="title">Spoiler warning</h2>
			<p class="text">{text}</p>

			<div class="actions">
				<a class="primary" href={catalogHref}>See Voki in catalog</a>
				<button class="secondary" type="button" onclick={handleIgnoreSpoiler}>See anyway</button>
			</div>
		</div>
	</div>
{:else}
	{@render children()}
{/if}

<style>
	.content {
		width: 100%;
		max-width: 34rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		text-align: center;
		gap: 0.75rem;
		margin: 4rem auto;
	}

	.icon {
		display: grid;
		place-items: center;
		width: 4rem;
		height: 4rem;
		color: var(--warn-foreground);
		background-color: var(--warn-back);
		border-radius: 50%;
		padding: 0.75rem;
		border: 0.125rem solid var(--warn-foreground);
		stroke-width: 1.5;
	}

	.icon > svg {
		width: 100%;
		height: 100%;
	}

	.title {
		color: var(--text);
		font-size: 1.5rem;
		line-height: 1.25;
		font-weight: 700;
		letter-spacing: -0.01em;
	}

	.text {
		color: var(--secondary-foreground);
		font-size: 1rem;
		line-height: 1.45;
		max-width: 30rem;
	}

	.actions {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		margin-top: 0.5rem;
	}

	.primary,
	.secondary {
		width: 100%;
		display: inline-flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		padding: 0.85rem 1rem;
		border-radius: 0.75rem;
		font-size: 1rem;
		font-weight: 600;
		line-height: 1;
		cursor: pointer;
		border: none;
	}

	.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.primary:hover {
		background: var(--primary-hov);
	}

	.secondary {
		background: var(--secondary);
		color: var(--text);
	}

	.secondary:hover {
		background: var(--accent);
		color: var(--accent-foreground);
	}

	.primary:focus-visible,
	.secondary:focus-visible {
		outline: 0.125rem solid var(--accent);
		outline-offset: 0.125rem;
	}
</style>
