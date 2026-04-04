<script lang="ts">
	interface Props {
		src: string;
	}

	const roundnessPercent = 40;
	let { src }: Props = $props();
</script>

<div class="border clip-path" style="--squircle-r: {roundnessPercent}%;">
	<img {src} class="clip-path" alt="Profile picture" />
</div>

<style>
	.clip-path {
		--_r: clamp(0%, calc(var(--squircle-r) / 2), 25%);
		--_v: calc(var(--_r) * (1 - sqrt(2) / 4));
		--_p: calc(var(--_v) - var(--_r) / 2);
		clip-path: shape(
			from var(--_v) var(--_p),
			curve to 50% 0 with var(--_r) 0,
			curve to calc(100% - var(--_v)) var(--_p) with calc(100% - var(--_r)) 0,
			curve to calc(100% - var(--_p)) var(--_v) with calc(100% - 2 * var(--_p)) calc(2 * var(--_p)),
			curve to 100% 50% with 100% var(--_r),
			curve to calc(100% - var(--_p)) calc(100% - var(--_v)) with 100% calc(100% - var(--_r)),
			curve to calc(100% - var(--_v)) calc(100% - var(--_p)) with calc(100% - 2 * var(--_p))
				calc(100% - 2 * var(--_p)),
			curve to 50% 100% with calc(100% - var(--_r)) 100%,
			curve to var(--_v) calc(100% - var(--_p)) with var(--_r) 100%,
			curve to var(--_p) calc(100% - var(--_v)) with calc(2 * var(--_p)) calc(100% - 2 * var(--_p)),
			curve to 0 50% with 0 calc(100% - var(--_r)),
			curve to var(--_p) var(--_v) with 0 var(--_r),
			curve to var(--_v) var(--_p) with calc(2 * var(--_p)) calc(2 * var(--_p))
		);
	}
	.border {
		padding: var(--profile-pic-border-width);
		border-radius: var(--profile-pic-radius);
		background-color: var(--back);
		width: inherit;
		height: inherit;
	}
	img {
		display: block;
		width: 100%;
		height: 100%;
		object-fit: cover;
	}
</style>
