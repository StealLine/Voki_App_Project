<script lang="ts">
	interface Props {
		loadingText: string;
	}
	let { loadingText }: Props = $props();
</script>

<div class="loader appear-with-delay">
{loadingText}
</div>