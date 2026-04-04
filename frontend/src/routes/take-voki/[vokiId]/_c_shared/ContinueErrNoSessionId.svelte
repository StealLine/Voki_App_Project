<script lang="ts">
	import SessionErrDisplay from './SessionErrDisplay.svelte';

	let { vokiId }: { vokiId: string } = $props();
</script>

<SessionErrDisplay {vokiId}>
	It seems like you wanted to continue an existing Voki taking session, but its data was not
	provided. Please go back to the catalog page and try to continue it from there
</SessionErrDisplay>
