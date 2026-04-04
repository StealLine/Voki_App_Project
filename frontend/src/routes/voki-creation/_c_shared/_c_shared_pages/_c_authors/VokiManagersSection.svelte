<script lang="ts">
	import VokiManagersViewState from './_c_managers_section/VokiManagersViewState.svelte';
	import VokiManagersEditingState from './_c_managers_section/VokiManagersEditingState.svelte';
	import type { VokiExpectedManagersSetting } from './types';

	interface Props {
		isViewerPrimaryAuthor: boolean;
		viewerId: string;
		expectedManagers: VokiExpectedManagersSetting;
		updateManagersSetting: (setting: VokiExpectedManagersSetting) => void;
		vokiCoAuthors: string[];
		vokiId: string;
		isEditing: boolean;
	}
	let {
		isViewerPrimaryAuthor,
		viewerId,
		expectedManagers,
		updateManagersSetting,
		vokiCoAuthors,
		vokiId,
		isEditing = $bindable()
	}: Props = $props();

	function startEditing() {
		isEditing = true;
	}

	function cancelEditing() {
		isEditing = false;
	}
</script>

<div class="managers-section" class:editing={isEditing}>
	{#if isViewerPrimaryAuthor && isEditing}
		<VokiManagersEditingState
			{vokiCoAuthors}
			{cancelEditing}
			updateParent={updateManagersSetting}
			savedSelectedUserIds={expectedManagers.userIdsToBecomeManagers}
			initialMakeAllManagers={expectedManagers.makeAllCoAuthorsManagers}
			{vokiId}
		/>
	{:else}
		<VokiManagersViewState
			setting={expectedManagers}
			{viewerId}
			{isViewerPrimaryAuthor}
			{startEditing}
		/>
	{/if}
</div>

<style>
	.managers-section {
		margin-top: 2rem;
	}

	.managers-section.editing {
		padding: 1rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs);
	}
</style>
