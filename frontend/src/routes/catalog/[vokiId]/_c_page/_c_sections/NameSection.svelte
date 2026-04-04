<script lang="ts">
	let { name }: { name: string } = $props<{ name: string }>();
	let reduceFontSize1 = name.length > 110;
	let reduceFontSize2 = name.length > 160;
</script>

<h1
	class="voki-name"
	class:reduce-font-size-1={reduceFontSize1}
	class:reduce-font-size-2={reduceFontSize2}
>
	{name}
</h1>

<style>
	.voki-name {
		margin: 0;
		font-size: 2.25rem;
		font-weight: 500;
		text-indent: 0.5em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.reduce-font-size-1 {
		font-size: 2rem;
	}

	.reduce-font-size-2 {
		font-size: 1.75rem;
	}
</style>
