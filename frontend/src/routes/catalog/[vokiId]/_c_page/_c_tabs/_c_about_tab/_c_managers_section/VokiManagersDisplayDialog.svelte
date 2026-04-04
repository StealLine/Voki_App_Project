<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	interface Props {
		managerIds: string[];
		coAuthorIds: Set<string>;
		primaryAuthorId: string;
	}
	let { managerIds, coAuthorIds, primaryAuthorId }: Props = $props();
	function managersThatWereCoAuthors() {
		return managerIds.filter((managerId) => coAuthorIds.has(managerId));
	}
	function managersThatWereNotCoAuthors() {
		return managerIds.filter((managerId) => !coAuthorIds.has(managerId));
	}
	let dialog = $state<DialogWithCloseButton>()!;
	export function open() {
		dialog.open();
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	subheading="Voki managers ({managerIds.length})"
	dialogId="voki-managers-dialog"
>
	<div class="managers-table">
		<span class="column-name">Manager</span>
		<span class="column-name">Was an author before</span>

		<BasicUserDisplay userId={primaryAuthorId} interactionLevel="WholeComponentLink" />
		<svg class="status-icon primary-author"><use href="#common-check-icon" /></svg>

		{#if managersThatWereCoAuthors().length}
			{#each managersThatWereCoAuthors() as m}
				<BasicUserDisplay userId={m} interactionLevel="WholeComponentLink" />
				<svg class="status-icon">
					<use href="#common-check-icon" />
				</svg>
			{/each}
		{/if}

		{#if managersThatWereNotCoAuthors().length}
			{#each managersThatWereNotCoAuthors() as m}
				<BasicUserDisplay userId={m} interactionLevel="WholeComponentLink" />
				<span class="status-placeholder">—</span>
			{/each}
		{/if}
	</div>
</DialogWithCloseButton>

<style>
	.managers-table {
		display: grid;
		width: 40rem;
		grid-template-columns: 1fr auto;
		row-gap: 0.75rem;
		max-height: 32rem;
		overflow-y: auto;
	}

	.column-name {
		display: grid;
		padding: 0 0.5rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
		grid-template-columns: 1fr auto;
	}

	.status-icon {
		width: 1.75rem;
		height: 1.75rem;
		color: var(--secondary-foreground);
		justify-self: center;
		stroke-width: 2;
	}

	.status-icon.primary-author {
		color: var(--primary);
	}

	.status-placeholder {
		justify-self: center;
		color: var(--muted-foreground);
		font-size: 1.1rem;
	}
</style>
