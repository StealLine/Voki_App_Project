<script lang="ts">
	interface Props {
		type?: string;
	}
	let { type }: Props = $props();
</script>

<div class="message-container">
	{#if type}
		<p>Unexpected question content type: {type}</p>
	{:else}
		<p>Question contenttype is not provided</p>
	{/if}
</div>
