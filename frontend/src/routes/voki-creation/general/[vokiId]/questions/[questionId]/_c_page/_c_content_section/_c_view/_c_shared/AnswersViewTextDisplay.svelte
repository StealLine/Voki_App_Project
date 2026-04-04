<script lang="ts">
	interface Props {
		text: string;
	}
	let { text }: Props = $props();
</script>

<p>{text}</p>

<style>
	p {
		width: 100%;
		box-sizing: border-box;
		font-size: 1.125rem;
		font-weight: 500;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
