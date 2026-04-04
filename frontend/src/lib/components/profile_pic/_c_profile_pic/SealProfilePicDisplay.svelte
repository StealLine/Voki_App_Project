<script lang="ts">
	interface Props {
		src: string;
	}

	let { src }: Props = $props();
</script>

<div class="border mask">
	<img {src} class="mask" alt="Profile picture" />
</div>

<style>
	.border {
		padding: var(--profile-pic-border-width);
		border-radius: var(--profile-pic-radius);
		background-color: var(--back);
		width: inherit;
		height: inherit;
		--s: var(--profile-picture-size);
	}
	img {
		--s: calc(var(--profile-picture-size) - var(--profile-pic-border-width) * 2);

		display: block;
		width: 100%;
		height: 100%;
		object-fit: cover;
		box-sizing: border-box;
		aspect-ratio: 1;
	}
	.mask {
		aspect-ratio: 1;
		--g: /calc(var(--s) * 0.118) calc(var(--s) * 0.118)
			radial-gradient(50% 50%, #000 99%, #0000 101%) no-repeat;
		mask:
			calc(50% + var(--s) * 0.422) calc(50% + var(--s) * 0) var(--g),
			calc(50% + var(--s) * 0.401) calc(50% + var(--s) * 0.13) var(--g),
			calc(50% + var(--s) * 0.341) calc(50% + var(--s) * 0.248) var(--g),
			calc(50% + var(--s) * 0.248) calc(50% + var(--s) * 0.341) var(--g),
			calc(50% + var(--s) * 0.13) calc(50% + var(--s) * 0.401) var(--g),
			calc(50% + var(--s) * 0) calc(50% + var(--s) * 0.422) var(--g),
			calc(50% + var(--s) * -0.13) calc(50% + var(--s) * 0.401) var(--g),
			calc(50% + var(--s) * -0.248) calc(50% + var(--s) * 0.341) var(--g),
			calc(50% + var(--s) * -0.341) calc(50% + var(--s) * 0.248) var(--g),
			calc(50% + var(--s) * -0.401) calc(50% + var(--s) * 0.13) var(--g),
			calc(50% + var(--s) * -0.422) calc(50% + var(--s) * 0) var(--g),
			calc(50% + var(--s) * -0.401) calc(50% + var(--s) * -0.13) var(--g),
			calc(50% + var(--s) * -0.341) calc(50% + var(--s) * -0.248) var(--g),
			calc(50% + var(--s) * -0.248) calc(50% + var(--s) * -0.341) var(--g),
			calc(50% + var(--s) * -0.13) calc(50% + var(--s) * -0.401) var(--g),
			calc(50% + var(--s) * 0) calc(50% + var(--s) * -0.422) var(--g),
			calc(50% + var(--s) * 0.13) calc(50% + var(--s) * -0.401) var(--g),
			calc(50% + var(--s) * 0.248) calc(50% + var(--s) * -0.341) var(--g),
			calc(50% + var(--s) * 0.341) calc(50% + var(--s) * -0.248) var(--g),
			calc(50% + var(--s) * 0.401) calc(50% + var(--s) * -0.13) var(--g),
			radial-gradient(calc(var(--s) * 0.467), #000 99%, #0000 101%) subtract,
			calc(50% + var(--s) * 0.508) calc(50% + var(--s) * 0.081) var(--g),
			calc(50% + var(--s) * 0.459) calc(50% + var(--s) * 0.234) var(--g),
			calc(50% + var(--s) * 0.364) calc(50% + var(--s) * 0.364) var(--g),
			calc(50% + var(--s) * 0.234) calc(50% + var(--s) * 0.459) var(--g),
			calc(50% + var(--s) * 0.081) calc(50% + var(--s) * 0.508) var(--g),
			calc(50% + var(--s) * -0.081) calc(50% + var(--s) * 0.508) var(--g),
			calc(50% + var(--s) * -0.234) calc(50% + var(--s) * 0.459) var(--g),
			calc(50% + var(--s) * -0.364) calc(50% + var(--s) * 0.364) var(--g),
			calc(50% + var(--s) * -0.459) calc(50% + var(--s) * 0.234) var(--g),
			calc(50% + var(--s) * -0.508) calc(50% + var(--s) * 0.081) var(--g),
			calc(50% + var(--s) * -0.508) calc(50% + var(--s) * -0.081) var(--g),
			calc(50% + var(--s) * -0.459) calc(50% + var(--s) * -0.234) var(--g),
			calc(50% + var(--s) * -0.364) calc(50% + var(--s) * -0.364) var(--g),
			calc(50% + var(--s) * -0.234) calc(50% + var(--s) * -0.459) var(--g),
			calc(50% + var(--s) * -0.081) calc(50% + var(--s) * -0.508) var(--g),
			calc(50% + var(--s) * 0.081) calc(50% + var(--s) * -0.508) var(--g),
			calc(50% + var(--s) * 0.234) calc(50% + var(--s) * -0.459) var(--g),
			calc(50% + var(--s) * 0.364) calc(50% + var(--s) * -0.364) var(--g),
			calc(50% + var(--s) * 0.459) calc(50% + var(--s) * -0.234) var(--g),
			calc(50% + var(--s) * 0.508) calc(50% + var(--s) * -0.081) var(--g);
	}
</style>
