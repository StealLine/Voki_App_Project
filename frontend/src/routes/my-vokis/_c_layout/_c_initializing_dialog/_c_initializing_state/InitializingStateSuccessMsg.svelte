<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';
	import GoToVokiCreationLink from '../../../_c_shared/GoToVokiCreationLink.svelte';

	interface Props {
		id: string;
		type: VokiType;
		name: string;
	}

	let { id, type, name }: Props = $props();
</script>

<div class="voki-initialized-container">
	<p class="initialized-msg">
		New {type} <span class="name">{name}</span> Voki has been initialized
	</p>
	<GoToVokiCreationLink vokiId={id} vokiType={type} />
</div>

<style>
	.voki-initialized-container {
		height: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2.5rem;
		width: 100%;
		max-width: 36rem;
		margin: 0 auto;
		padding: 0 1rem;
		text-align: center;
	}

	.initialized-msg {
		margin: 0;
		color: var(--text);
		font-size: 2.5rem;
		font-weight: 800;
		text-align: center;
		letter-spacing: -0.025em;
		line-height: 1.1;
		text-wrap: balance;
	}

	.name {
		color: var(--primary);
		padding: 0 0.25rem;
		overflow-wrap: anywhere;
		word-break: break-word;
	}
</style>
