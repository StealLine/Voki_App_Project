<script lang="ts">
	interface Props {
		text: string;
	}
	let { text = $bindable() }: Props = $props();
</script>

<header class="header">
	<h1 class="title">{text}</h1>
	<p class="note">
		<span>Note:</span> every choice can be changed later
	</p>
</header>

<style>
	.header {
		display: grid;
		place-content: center center;
		align-items: center;
		gap: 0.5rem;
		height: 4.5rem;
	}

	.title {
		color: var(--text);
		font-size: 1.75rem;
		font-weight: 700;
		line-height: 1.35;
		letter-spacing: 0.25px;
	}

	.note {
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-align: center;
	}

	.note span {
		font-weight: 580;
	}
</style>
