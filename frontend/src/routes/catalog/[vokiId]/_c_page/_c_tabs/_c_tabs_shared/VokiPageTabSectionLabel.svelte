<script lang="ts">
	interface Props {
		fieldName: string;
		class?: string;
	}
	const { fieldName, class: className = '' }: Props = $props();
</script>

<label class={className}>{fieldName}</label>

<style>
	label {
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		line-height: 1.875rem;
	}
</style>
