<script lang="ts">
	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();
</script>

<div class="exists-failed-container">
	<h3>Voki Initialized with Delay</h3>
	<p>
		Your voki has been initialized, but it might take a bit of time for it to become accessible to
		edit. You can access it from your <a href="/my-vokis/draft-vokis?vokiHighlight={vokiId}"
			>Drafts</a
		>
	</p>
</div>

<style>
	.exists-failed-container {
		flex-direction: column;
		display: flex;
		align-items: center;
		height: 100%;
	}

	.exists-failed-container h3 {
		font-size: 2rem;
		font-weight: 700;
		color: var(--text);
		margin-bottom: 2rem;
	}

	.exists-failed-container p {
		color: var(--muted-foreground);
		font-size: 1.5rem;
		text-align: center;
		text-wrap: balance;
		line-height: 1.25;
	}

	.exists-failed-container p a {
		color: var(--primary);
		text-decoration: none;
		font-weight: 600;
		text-decoration: underline;
		text-decoration-thickness: 2px;
	}
</style>
