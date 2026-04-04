<script lang="ts">
	import type { Snippet } from 'svelte';
	import ConfirmationStepSmallEditButton from './ConfirmationStepSmallEditButton.svelte';

	interface Props {
		listName: string;
		onEditClick: () => void;
		children: Snippet;
	}
	let { listName, onEditClick, children }: Props = $props();
</script>

<div class="items-list-container">
	<div class="list-name">
		{listName}
		<ConfirmationStepSmallEditButton onclick={onEditClick} />
	</div>

	{@render children()}
</div>

<style>
	.list-name {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		padding: 0 1rem;
		margin-bottom: 0.5rem;
		font-size: 1.375rem;
		font-weight: 550;
	}
</style>
