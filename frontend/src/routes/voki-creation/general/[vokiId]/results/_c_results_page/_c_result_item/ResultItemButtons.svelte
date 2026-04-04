<script lang="ts">
	interface Props {
		mainBtnText: string;
		mainBtnOnClick: () => void;
		secondaryBtnIconId: string;
		secondaryBtnOnClick: () => void;
	}
	let { mainBtnText, mainBtnOnClick, secondaryBtnIconId, secondaryBtnOnClick }: Props = $props();
</script>

<div class="buttons-container">
	<button class="main-btn" onclick={() => mainBtnOnClick()}>{mainBtnText}</button>
	<button class="secondary-btn" onclick={() => secondaryBtnOnClick()}>
		<svg><use href={secondaryBtnIconId} /></svg>
	</button>
</div>

<style>
	.buttons-container {
		display: flex;
		flex-direction: row;
		justify-content: flex-end;
		gap: 0.5rem;
		width: 100%;
		margin: 0.125rem 0.25rem 0.25rem 0;
	}

	.buttons-container > * {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		box-shadow: var(--shadow);
		outline: none;
		cursor: pointer;
	}

	.main-btn {
		padding: 0 1.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 480;
		letter-spacing: 1.5px;
	}

	.main-btn:hover {
		background-color: var(--primary-hov);
	}

	.secondary-btn {
		width: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		stroke-width: 2.1;
	}

	.secondary-btn:hover {
		background-color: var(--red-5);
		color: var(--primary-foreground);
		stroke-width: 1.8;
	}

	.secondary-btn svg {
		width: 1.5rem;
		height: 1.5rem;
	}
</style>
