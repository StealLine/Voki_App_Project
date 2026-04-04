<script lang="ts">
	import type { PossiblyHidden } from '../types';
	interface Props {
		displayName: string;
		uniqueName: string;
		pronouns: PossiblyHidden<string>;
		status: PossiblyHidden<string>;
	}
	let { displayName, uniqueName, pronouns, status }: Props = $props();
</script>

<div class="identity-block">
	<div class="display-name-row">
		<h1>{displayName}</h1>

		{#if pronouns.showOnProfile}
			<span class="pronouns">{pronouns.value}</span>
		{/if}
	</div>

	<p class="unique-name">@{uniqueName}</p>

	{#if status.showOnProfile}
		<p class="status">{status.value}</p>
	{/if}
</div>

<style>
	.identity-block {
		display: flex;
		align-items: start;
		flex-direction: column;
		min-width: 0;
		gap: 0.375rem;
		margin-top: 0.5rem;
	}

	.display-name-row {
		display: flex;
		flex-wrap: wrap;
		align-items: center;
		gap: 0.5rem;
	}

	.display-name-row h1 {
		margin: 0;
		font-size: 2rem;
		font-weight: 800;
		word-break: break-word;
	}

	.pronouns {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		padding: 0.25rem 0.675rem;
		border-radius: 999rem;
		background: var(--accent);
		color: var(--accent-foreground);
		font-size: 0.875rem;
		margin-top: 0.375rem;
		font-weight: 600;
	}

	.unique-name {
		font-size: 1rem;
		font-weight: 600;
		color: var(--muted-foreground);
		word-break: break-word;
	}

	.status {
		font-size: 1rem;
		line-height: 1.25;
		color: var(--muted-foreground);
		word-break: break-word;
		font-weight: 450;
	}
</style>
