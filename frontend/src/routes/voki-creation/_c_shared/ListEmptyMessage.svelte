<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	interface Props {
		messageText: string;
		btnText: string;
		onBtnClick: () => void;
		className?: string;
	}
	let { messageText, btnText, onBtnClick, className = '' }: Props = $props();
</script>

<div class="message-container {className}">
	<h1>{messageText}</h1>
	<PrimaryButton onclick={onBtnClick}>{btnText}</PrimaryButton>
</div>

<style>
	.message-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		width: fit-content;
		padding: 3rem 6rem;
		margin: 2rem auto;
		border: var(--primary) 0.25rem dashed;
		border-radius: 1rem;
		background-color: var(--secondary);
	}

	.message-container h1 {
		color: var(--secondary-foreground);
		font-size: 2rem;
		font-weight: 540;
		text-align: center;
	}
</style>
