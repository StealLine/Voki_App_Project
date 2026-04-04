<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import type { VokiType } from '$lib/ts/voki-type';
	import VokiTypeCard from './VokiTypeCard.svelte';

	interface Props {
		vokiName: string;
		selectedVokiType: VokiType;
		errs: Err[];
		onSubmit: () => void;
	}
	let { vokiName = $bindable(), selectedVokiType = $bindable(), errs, onSubmit }: Props = $props();
</script>

<p class="subheading">Choose voki type</p>
<div class="voki-type-container">
	<VokiTypeCard
		type="General"
		isSelected={selectedVokiType === 'General'}
		onclick={() => (selectedVokiType = 'General')}
	/>
	<VokiTypeCard
		type="TierList"
		isSelected={selectedVokiType === 'TierList'}
		onclick={() => (selectedVokiType = 'TierList')}
	/>
	<VokiTypeCard
		type="Scoring"
		isSelected={selectedVokiType === 'Scoring'}
		onclick={() => (selectedVokiType = 'Scoring')}
	/>
</div>
<p class="subheading">Input Voki name</p>
<input bind:value={vokiName} class="name-input" type="text" placeholder="Voki name..." />
<DefaultErrBlock errList={errs} />
<PrimaryButton onclick={onSubmit} class="initialize-voki-btn">Create</PrimaryButton>

<style>
	.subheading {
		width: fit-content;
		margin: 0 0 1rem;
		font-size: 2rem;
		font-weight: 500;
	}

	.voki-type-container {
		display: flex;
		flex-direction: row;
		gap: 4rem;
		margin-bottom: 4rem;
	}

	.name-input {
		width: 100%;
		height: 2.5rem;
		padding: 0.125rem 0.5rem;
		border: none;
		border: 0.2rem solid transparent;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		box-shadow: var(--shadow);
		transition: background-color 0.08s ease-in-out;
	}

	.name-input:focus {
		outline: none;
		border-color: var(--primary);
	}

	:global(#voki-initializing-dialog .err-block) {
		margin: 1rem;
	}

	:global(.primary-btn.initialize-voki-btn) {
		padding: 0.25rem 1.5rem;
		margin: auto auto 0;
	}
</style>
