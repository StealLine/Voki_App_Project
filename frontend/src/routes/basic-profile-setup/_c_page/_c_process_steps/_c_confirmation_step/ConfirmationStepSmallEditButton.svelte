<script lang="ts">
	interface Props {
		onclick: () => void;
		className?: string;
	}
	let { onclick, className = '' }: Props = $props();
</script>

<svg class="edit-btn {className}" {onclick} aria-label="Edit profile picture"
	><use href="#common-edit-icon" /></svg
>

<style>
	.edit-btn {
		width: 1.375rem;
		height: 1.375rem;
		padding: 0.25rem;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		stroke-width: 2;
	}

	.edit-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
