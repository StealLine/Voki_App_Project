<script lang="ts">
	interface Props {
		subtitle: string;
	}
	let { subtitle }: Props = $props();
</script>

<div class="no-answers-container">
	<div class="text-content">
		<span class="title">No answers yet</span>
		<span class="subtitle">{subtitle}</span>
	</div>
</div>

<style>
	.no-answers-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		box-sizing: border-box;
		padding: 1rem 3rem;
		margin: 0 auto;
		border-radius: 1rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.text-content {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		text-align: center;
	}

	.title {
		color: var(--muted-foreground);
		font-size: 1.5rem;
		font-weight: 500;
	}

	.subtitle {
		color: var(--secondary-foreground);
		font-size: 1rem;
	}
</style>
