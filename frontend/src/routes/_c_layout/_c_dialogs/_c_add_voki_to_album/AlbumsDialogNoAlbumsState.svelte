<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	interface Props {
		changeToCreateNewAlbum: () => void;
	}
	let { changeToCreateNewAlbum }: Props = $props();
</script>

<div class="no-albums">
	<h1>You don't have any albums</h1>
	<p>Start organizing your Vokis now</p>
	<PrimaryButton onclick={() => changeToCreateNewAlbum()}>Create first album</PrimaryButton>
</div>

<style>
	.no-albums {
		display: grid;
		justify-content: center;
		gap: 1rem;
		height: 100%;
		grid-template-rows: 1fr auto auto;
		justify-items: center;
	}

	h1 {
		margin: auto 0;
		color: var(--muted-foreground);
		font-size: 2rem;
		font-weight: 600;
		letter-spacing: 0.5px;
	}

	p {
		margin-bottom: 0;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
	}
</style>
