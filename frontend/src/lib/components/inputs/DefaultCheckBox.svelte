<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	interface Props {
		checked: boolean;
		id?: string;
		parentOnlyControl?: boolean;
	}
	let {
		checked = $bindable(),
		id = StringUtils.rndStrWithPref('checkbox-'),
		parentOnlyControl = false
	}: Props = $props();
</script>

{#if parentOnlyControl}
	<input type="checkbox" {id} class="input" {checked} />
	<label class="cbx unselectable">
		<span><svg viewBox="0 0 12 10"> <polyline points="1.5 6 4.5 9 10.5 1" /></svg></span>
	</label>
{:else}
	<label class="cbx unselectable">
		<input type="checkbox" {id} class="input" bind:checked />
		<span><svg viewBox="0 0 12 10"> <polyline points="1.5 6 4.5 9 10.5 1" /></svg></span>
	</label>
{/if}

<style>
	input {
		display: none;
		visibility: hidden;
	}

	.cbx {
		display: inline;
	}

	.cbx span {
		display: inline-block;
		display: flex;
		place-content: center center;
		width: 1.25rem;
		height: 1.25rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.25rem;
		transition: all 0.2s ease;
	}

	.cbx span svg {
		width: 100%;
		height: 100%;
		box-sizing: border-box;
		padding: 0.125rem;
		transition: all 0.3s ease;
		fill: none;
		stroke: var(--primary-foreground);
		stroke-width: 2;
		stroke-linecap: round;
		stroke-linejoin: round;
		stroke-dasharray: 1rem;
		stroke-dashoffset: 1rem;
		transition-delay: 0.1s;
	}

	.cbx:hover span {
		border-color: var(--primary);
		background-color: var(--secondary);
	}

	.input:checked + .cbx span,
	.input:checked +  span {
		border-color: var(--primary);
		background: var(--primary);
	}

	.input:checked + .cbx span svg,
	.input:checked + span svg {
		stroke-dashoffset: 0;
	}
</style>
