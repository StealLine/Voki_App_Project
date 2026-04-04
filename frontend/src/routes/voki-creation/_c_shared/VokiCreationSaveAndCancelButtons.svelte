<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';

	interface Props {
		onCancel: () => void;
		onSave: () => void;
		isSaveLoading: boolean;
	}
	let { onCancel, onSave, isSaveLoading }: Props = $props();
</script>

<div class="btns-container" class:loading={isSaveLoading}>
	<button onclick={onCancel} class="cancel-btn">Cancel</button>
	<button onclick={onSave} class="save-btn" class:loading={isSaveLoading}
		>{#if isSaveLoading}
			<LinesLoader color="var(--primary-foreground)" sizeRem={1.25} strokePx={2} />
		{:else}
			Save
		{/if}</button
	>
</div>

<style>
	.btns-container {
		display: flex;
		flex-direction: row;
		justify-content: right;
		gap: 0.5rem;
		width: 100%;
		margin-top: 0.5rem;
	}

	.btns-container button {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 7rem;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.2px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
	}

	.btns-container.loading button {
		pointer-events: none;
	}

	.cancel-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 480;
	}

	.cancel-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.save-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.save-btn:hover {
		background-color: var(--primary-hov);
	}

	.save-btn.loading {
		opacity: 0.9;
	}
</style>
