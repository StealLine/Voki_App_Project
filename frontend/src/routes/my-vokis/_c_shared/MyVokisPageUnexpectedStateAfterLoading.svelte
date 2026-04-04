<script lang="ts">
	interface Props {
		reloadPage: () => void;
	}
	let { reloadPage }: Props = $props();
</script>

<h1>Something went wrong</h1>
<p>Try reloading the page. If the problem persists, contact support</p>
<button onclick={reloadPage}>Reload page</button>
