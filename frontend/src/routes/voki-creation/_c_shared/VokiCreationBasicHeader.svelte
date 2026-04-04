<script lang="ts">
	interface Props {
		header: string;
		className?: string;
	}
	const { header, className = '' }: Props = $props();
</script>

<h1 class={className}>{header}</h1>

<style>
	h1 {
		display: block;
		width: 100%;
		margin: 0.75rem 0 1rem;
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 550;
		text-align: center;
	}
</style>
