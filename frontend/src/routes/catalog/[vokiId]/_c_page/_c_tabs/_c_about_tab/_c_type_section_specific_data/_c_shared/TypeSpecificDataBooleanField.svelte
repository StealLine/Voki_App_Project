<script lang="ts">
	interface Props {
		fieldName: string;
		value: boolean;
		trueLabel?: string;
		falseLabel?: string;
	}
	let { fieldName, value, trueLabel = 'Yes', falseLabel = 'No' }: Props = $props();
</script>

<div class="field-line">
	<div class="name">
		<span>{fieldName}</span>
	</div>
	<div class="value">
		{value ? trueLabel : falseLabel}
	</div>
</div>

<style>
	.field-line {
		display: flex;
		flex-direction: row;
		align-items: center;
		cursor: default;
	}

	.name {
		display: flex;
		flex-direction: row;
		align-items: center;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 425;
	}

	.value {
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 450;
	}
</style>
