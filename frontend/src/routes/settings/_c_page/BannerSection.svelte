<script lang="ts">
	import type { AuthorBanner } from '$lib/ts/users';

	interface Props {
		banner: AuthorBanner;
	}

	let { banner }: Props = $props();
</script>

<div>{JSON.stringify(banner)}</div>

<style>
	div {
		background-color: darkgray;
		width: 100%;
		height: 10rem;
	}
</style>
