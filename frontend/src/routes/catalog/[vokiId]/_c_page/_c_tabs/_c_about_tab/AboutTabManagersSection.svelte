<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import VokiPageTabSectionLabel from '../_c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import VokiManagersDisplayDialog from './_c_managers_section/VokiManagersDisplayDialog.svelte';

	interface Props {
		managerIds: string[];
		coAuthorIds: string[];
		primaryAuthorId: string;
	}
	let { managerIds, coAuthorIds, primaryAuthorId }: Props = $props();
	let vokiManagersDisplayDialog = $state<VokiManagersDisplayDialog>()!;
</script>

<VokiManagersDisplayDialog
	bind:this={vokiManagersDisplayDialog}
	{managerIds}
	coAuthorIds={new Set(coAuthorIds)}
	{primaryAuthorId}
/>
<p class="managed-by-line">
	<VokiPageTabSectionLabel fieldName="Managed by:" />
	{#if managerIds.length === 0}
		<FieldNotSetLabel showIcon={false} text="Only primary author" />
	{:else}
		<label class="managers-dialog-open-label" onclick={() => vokiManagersDisplayDialog.open()}
			>primary author and <span>{managerIds.length}</span>
			manager{managerIds.length === 1 ? '' : 's'}</label
		>
	{/if}
</p>

<style>
	.managed-by-line {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		margin: 0;
	}

	.managers-dialog-open-label {
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 475;
		text-decoration: underline;
		cursor: pointer;
		text-decoration-thickness: 2px;
	}

	.managers-dialog-open-label:hover {
		color: var(--primary);
	}
</style>
