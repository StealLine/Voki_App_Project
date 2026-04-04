<script lang="ts">
	let { text }: { text: string } = $props<{ text: string }>();
</script>

<h1 class="part-header">{text}</h1>

<style>
	.part-header {
		margin: 0 0 0.5rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 500;
		text-align: center;
	}
</style>
