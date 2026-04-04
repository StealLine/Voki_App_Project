<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		title: string;
		children: Snippet;
	}

	let { title, children }: Props = $props();
</script>

<div class="section">
	<h2 class="profile-sidebar-subheader">{title}</h2>
	{@render children()}
</div>

<style>
	.section {
		display: flex;
		flex-direction: column;
		gap: 0.125rem;
	}
	.profile-sidebar-subheader {
		font-size: 1.375rem;
		font-weight: 700;
		margin: 0;
		cursor: default;
	}
</style>
