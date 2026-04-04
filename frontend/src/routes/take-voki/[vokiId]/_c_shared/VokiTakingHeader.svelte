<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';

	interface Props {
		vokiName: string;
		vokiType: VokiType;
	}
	let { vokiName, vokiType }: Props = $props();
</script>

<h1>Taking {vokiType}<label>{vokiName}</label>voki</h1>

<style>
	h1 {
		display: flex;
		justify-content: center;
		align-items: center;
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 440;
	}

	h1 label {
		display: inline-block;
		max-width: calc(40vw - 4rem);
		margin: 0 0.25rem;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--primary);
		font-weight: 490;
	}
</style>
