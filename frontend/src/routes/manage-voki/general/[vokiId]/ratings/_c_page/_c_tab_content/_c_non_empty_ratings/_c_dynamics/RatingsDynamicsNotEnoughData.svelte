<script lang="ts">
</script>

<div class="empty-state">
	<div class="icon-container">
		<svg class="placeholder-icon"><use href="#common-info-icon" /></svg>
	</div>

	<div class="text-content">
		<h3 class="title">Dynamics will appear tomorrow</h3>
		<p class="description">
			This Voki was published recently. You will be able to track rating dynamics once the first
			daily snapshot is taken, which will happen no later than tomorrow.
		</p>
	</div>
</div>

<style>
	.empty-state {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		text-align: center;
		padding: 4rem 2rem;
		background: var(--back);
		border-radius: 1rem;
		border: 0.125rem dashed var(--muted);
		margin-top: 2rem;
		gap: 1.5rem;
	}

	.icon-container {
		display: flex;
		align-items: center;
		justify-content: center;
		width: 4rem;
		height: 4rem;
		border-radius: 50%;
		background: var(--secondary);
		color: var(--muted-foreground);
	}

	.placeholder-icon {
		width: 2rem;
		height: 2rem;
		stroke-width: 2;
	}

	.text-content {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		max-width: 400px;
	}

	.title {
		font-size: 1.25rem;
		font-weight: 600;
		color: var(--text);
		margin: 0;
	}

	.description {
		font-size: 1rem;
		color: var(--muted-foreground);
		line-height: 1.5;
		margin: 0;
		width: 42ch;
		text-wrap: pretty;
	}
</style>
