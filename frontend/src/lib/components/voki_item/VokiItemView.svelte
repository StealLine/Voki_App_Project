<script lang="ts">
	import type { VokiItemViewState } from './_c_voki_item/voki-item';
	import VokiItemViewErrState from './_c_voki_item/VokiItemViewErrState.svelte';
	import VokiItemViewLoadingState from './_c_voki_item/VokiItemViewLoadingState.svelte';
	import VokiItemViewOkState from './_c_voki_item/VokiItemViewOkState.svelte';

	interface Props {
		state: VokiItemViewState;
	}

	let { state }: Props = $props();
</script>

{#if state.name === 'ok'}
	<VokiItemViewOkState {...state.data} hide={state.hide} />
{:else if state.name === 'loading'}
	<VokiItemViewLoadingState hide={state.hide} />
{:else if state.name === 'errs'}
	<VokiItemViewErrState {...state.data} />
{/if}
