<script lang="ts">
	interface Props {
		text: string;
	}
	let { text }: Props = $props();
</script>

<p>{text}</p>

<style>
	p {
		margin-bottom: 0.5rem;
		color: var(--secondary-foreground);
		font-weight: 450;
		cursor: default;
		font-size: 1rem;
	}
</style>
