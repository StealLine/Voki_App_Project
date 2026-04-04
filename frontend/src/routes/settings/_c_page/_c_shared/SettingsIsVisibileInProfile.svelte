<script lang="ts">
	interface Props {
		showOnProfileToogleEnabled: boolean;
	}
	let { showOnProfileToogleEnabled }: Props = $props();
</script>

<div class="marker"></div>

<style>
	.marker {
		display: inline;
		height: 1.25rem;
		width: 1.25rem;
		background-color: var(--primary);
		border-radius: 100%;
	}
	.marker:hover {
		background-color: var(--primary-hov);
	}
</style>
