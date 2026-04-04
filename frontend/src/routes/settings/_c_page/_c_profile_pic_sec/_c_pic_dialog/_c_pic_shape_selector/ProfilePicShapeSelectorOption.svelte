<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		onclick: () => void;
		name: string;
		children: Snippet;
	}
	let { onclick, name, children: icon }: Props = $props();
</script>

<div class="option" {onclick}>
	<label>{name}</label>
	{@render icon()}
</div>

<style>
	.option {
		height: 5rem;
		aspect-ratio: 1;
	}
</style>
