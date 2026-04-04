<script lang="ts">
	interface Props {
		src: string;
		radiusPercent: number;
	}

	let { src, radiusPercent }: Props = $props();
</script>

<img {src} style="--profile-pic-radius: {radiusPercent}%;" />

<style>
	img {
		display: block;
		width: 100%;
		height: 100%;
		object-fit: cover;
		border-radius: var(--profile-pic-radius);
		border: var(--profile-pic-border-width) solid var(--profile-pic-border-color);
	}
</style>
