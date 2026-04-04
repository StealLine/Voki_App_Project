<script lang="ts">
	import HeaderSearchMsgTemplate from './_c_shared/HeaderSearchMsgTemplate.svelte';
</script>

<HeaderSearchMsgTemplate text="Start typing to search...">
	{#snippet icon()}
		<svg
			xmlns="http://www.w3.org/2000/svg"
			viewBox="0 0 24 24"
			fill="none"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		>
			<path d="M20 11V9a2 2 0 0 0 -2 -2H6a2 2 0 0 0 -2 2v5a2 2 0 0 0 2 2h5"></path>
			<path d="M15 18a3 3 0 1 0 6 0 3 3 0 1 0 -6 0"></path>
			<path d="M20.2 20.2 22 22"></path>
		</svg>
	{/snippet}
</HeaderSearchMsgTemplate>
