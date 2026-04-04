<script lang="ts">
	interface Props {
		sizeRem?: number;
		speedSec?: number;
		color: string;
	}
	let { sizeRem = 3, speedSec = 1.5, color }: Props = $props();
</script>

<div
	class="container"
	style="

		--size: {sizeRem}rem;
		--speed: {speedSec}s;
		--loader-color: {color};
	"
>
	<div class="cube"><div class="cube-inner"></div></div>
	<div class="cube"><div class="cube-inner"></div></div>
	<div class="cube"><div class="cube-inner"></div></div>
</div>

<style>
	.container {
		display: flex;
		justify-content: space-between;
		align-items: flex-end;
		width: var(--size);
		height: calc(var(--size) * 0.6);
	}

	.cube {
		flex-shrink: 0;
		width: calc(var(--size) * 0.2);
		height: calc(var(--size) * 0.2);
		animation: jump var(--speed) ease-in-out infinite;
	}

	.cube-inner {
		display: block;
		width: 100%;
		height: 100%;
		border-radius: 25%;
		background-color: var(--loader-color);
		transition: background-color 0.3s ease;
		animation: morph var(--speed) ease-in-out infinite;
		transform-origin: center bottom;
	}

	.cube:nth-child(2) {
		animation-delay: calc(var(--speed) * -0.36);

		.cube-inner {
			animation-delay: calc(var(--speed) * -0.36);
		}
	}

	.cube:nth-child(3) {
		animation-delay: calc(var(--speed) * -0.2);

		.cube-inner {
			animation-delay: calc(var(--speed) * -0.2);
		}
	}

	@keyframes jump {
		0% {
			transform: translateY(0);
		}

		30% {
			transform: translateY(0);
			animation-timing-function: ease-out;
		}

		50% {
			transform: translateY(-200%);
			animation-timing-function: ease-in;
		}

		75% {
			transform: translateY(0);
			animation-timing-function: ease-in;
		}
	}

	@keyframes morph {
		0% {
			transform: scaleY(1);
		}

		10% {
			transform: scaleY(1);
		}

		20%,
		25% {
			transform: scaleY(0.6) scaleX(1.3);
			animation-timing-function: ease-in-out;
		}

		30% {
			transform: scaleY(1.15) scaleX(0.9);
			animation-timing-function: ease-in-out;
		}

		40% {
			transform: scaleY(1);
		}

		70%,
		85%,
		100% {
			transform: scaleY(1);
		}

		75% {
			transform: scaleY(0.8) scaleX(1.2);
		}
	}
</style>
