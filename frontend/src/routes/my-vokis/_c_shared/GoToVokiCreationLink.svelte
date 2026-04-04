<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki-type';

	interface Props {
		vokiId: string;
		vokiType: VokiType;
		class?: string;
	}
	let { vokiId, vokiType, class: className }: Props = $props();
	let link = $derived(`/voki-creation/${StringUtils.pascalToKebab(vokiType)}/${vokiId}`);
</script>

<a href={link} class={className} aria-label="Open Voki creation page"
	>Go to Voki creation <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M11.0991 3.00012C7.45013 3.00669 5.53932 3.09629 4.31817 4.31764C3.00034 5.63568 3.00034 7.75704 3.00034 11.9997C3.00034 16.2424 3.00034 18.3638 4.31817 19.6818C5.63599 20.9999 7.75701 20.9999 11.9991 20.9999C16.241 20.9999 18.3621 20.9999 19.6799 19.6818C20.901 18.4605 20.9906 16.5493 20.9972 12.8998"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			class="arrow"
			d="M20.556 3.49612L11.0487 13.0586M20.556 3.49612C20.062 3.00151 16.7343 3.04761 16.0308 3.05762M20.556 3.49612C21.05 3.99074 21.0039 7.32273 20.9939 8.02714"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg></a
>

<style>
	a {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.25rem;
		width: fit-content;
		box-sizing: border-box;
		padding: 0.25rem 1.5rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 400;
		letter-spacing: 0.5px;
		transition: background-color 0.08s ease-in-out;
		transition: all 0.12s ease;
		cursor: pointer;
	}

	a svg {
		width: 1.5rem;
		height: 1.5rem;
		padding-top: 0.125rem;
		stroke-width: 2;
	}

	a:hover {
		background-color: var(--primary-hov);
	}

	.arrow {
		transition: all 0.12s ease;
		transform-origin: center;
	}

	a:hover .arrow {
		transform: translate(2px, -2px);
	}

	a:active .arrow {
		transform: translate(2px, -2px) scale(0.9);
	}
</style>
