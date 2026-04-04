<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import VokiPageTabSectionLabel from '../_c_tabs_shared/VokiPageTabSectionLabel.svelte';

	let { description }: { description: string } = $props<{ description: string }>();
</script>

<p class="description" class:no-value={StringUtils.isNullOrWhiteSpace(description)}>
	<VokiPageTabSectionLabel fieldName="Description:" />
	{#if StringUtils.isNullOrWhiteSpace(description)}
		<FieldNotSetLabel text="No description" />
	{:else}
		<label class="value">
			{description}
		</label>
	{/if}
</p>

<style>
	.description {
		padding-right: 0.125rem;
		padding-left: 0;
		margin: 0;
		text-align: justify;
	}

	.description.no-value {
		display: flex;
		align-items: center;
	}

	.description > .value {
		width: 100%;
		font-size: 1.25rem;
		font-weight: 440;
		text-indent: 0.5em;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
