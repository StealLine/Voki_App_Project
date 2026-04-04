<script lang="ts">
	let {
		tag,
		isTagAdded,
		isTagRemovingState,
		btnOnClick
	}: {
		tag: string;
		isTagAdded: boolean;
		isTagRemovingState: boolean;
		btnOnClick: () => void;
	} = $props<{
		tag: string;
		isTagAdded: boolean;
		isTagRemovingState: boolean;
		btnOnClick: () => void;
	}>();
</script>

<div class="tag-display">
	<span>#{tag}</span>
	{#if isTagRemovingState}
		<svg class="remove-btn" onclick={() => btnOnClick()}>
			<use href="#common-cross-icon" />
		</svg>
	{:else if isTagAdded}
		<svg>
			<use href="#common-check-icon" />
		</svg>
	{:else}
		<svg class="add-btn" onclick={() => btnOnClick()} viewBox="0 0 24 24" fill="none">
			<use href="#common-plus-icon" />
		</svg>
	{/if}
</div>

<style>
	.tag-display {
		display: grid;
		align-items: center;
		width: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.5rem;
		border-radius: 0.25rem;
		color: var(--text);
		grid-template-columns: 1fr auto;
	}

	.tag-display:hover {
		background-color: var(--secondary);
	}

	.tag-display span {
		font-size: 1.125rem;
		cursor: default;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.tag-display svg {
		height: 1.5rem;
		box-sizing: border-box;
		padding: 0.25rem;
		border-radius: 16%;
		background-color: var(--muted);
		stroke-width: 3;
		color: var(--muted-foreground);
		transition: all 0.04s ease-in;
		cursor: pointer;
		aspect-ratio: 1/1;
	}

	.add-btn:hover {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.remove-btn:hover {
		background-color: var(--red-5);
		color: var(--red-1);
	}
</style>
