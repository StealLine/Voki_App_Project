<script lang="ts">
	const { fieldName, className = '' }: { fieldName: string; className?: string } = $props<{
		fieldName: string;
		className?: string;
	}>();
</script>

<label class={className}>{fieldName}</label>

<style>
	label {
		margin-right: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 420;
		text-decoration: underline;
		text-underline-offset: 0.125rem;
	}
</style>
