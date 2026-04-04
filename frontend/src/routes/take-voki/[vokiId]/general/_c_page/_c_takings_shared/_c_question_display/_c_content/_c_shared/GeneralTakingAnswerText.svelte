<script lang="ts">
	let { text }: { text: string } = $props<{ text: string }>();
</script>

<label>
	{text}
</label>

<style>
	label {
		font-size: 1.125rem;
		font-weight: 450;
		cursor: default;
		word-break: normal;
		overflow-wrap: anywhere;
		text-indent: 0.5em;
	}
</style>
