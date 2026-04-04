<script lang="ts">
	import SessionErrDisplay from './SessionErrDisplay.svelte';

	let { vokiId }: { vokiId: string } = $props();
</script>

<SessionErrDisplay {vokiId}>
	It seems like you wanted to terminate an existing Voki taking session and start a new one, but
	something went wrong. Please go back to the catalog page and try again from there
</SessionErrDisplay>
