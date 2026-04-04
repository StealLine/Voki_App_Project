<script lang="ts">
	import { onMount } from 'svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';

	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => {},
			invitesPage: {
				exists: true,
				path: 'invites'
			}
		});
	});
</script>
