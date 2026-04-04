<script lang="ts">
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import GeneralVokiResultPreviewImage from '../../_c_shared/GeneralVokiResultPreviewImage.svelte';

	interface Props {
		result: {
			id: string;
			name: string;
			image: string | null;
			takings: { id: string; start: Date; finish: Date }[];
		};
		vokiId: string;
	}
	let isOpen = $state(false);
	let { result, vokiId }: Props = $props();
</script>

<li class="result">
	<div class="result-img">
		<GeneralVokiResultPreviewImage resultImage={result.image} resultName={result.name} />
	</div>
	<div class="result-main">
		<h3 class="result-name" title={result.name}>{result.name}</h3>
		<div class="buttons-container">
			<a class="result-page-link" href={`/take-voki/${vokiId}/general/results/${result.id}`}
				>Result page</a
			>

			<button
				class="toggle-takings-btn"
				class:open={isOpen}
				onclick={() => (isOpen = !isOpen)}
				title="Show all my takings with this result"
			>
				Received {result.takings.length}
				{result.takings.length === 1 ? 'time' : 'times'}
				<svg aria-hidden="true">
					<use href="#caret-up-icon" />
				</svg>
			</button>
		</div>

		<div class="takings-container" class:open={isOpen}>
			<div class="takings-column-names">
				<span>Start</span>
				<span>Finish</span>
				<span>Duration</span>
			</div>

			{#each result.takings as t}
				<div class="taking-row">
					<label>
						{DateUtils.toLocale(t.start)}
					</label>
					<label>
						{DateUtils.toLocale(t.finish)}
					</label>
					<label>
						{DateUtils.formatDuration(t.start, t.finish)}
					</label>
				</div>
			{/each}
		</div>
	</div>
</li>

<style>
	.result {
		display: grid;
		gap: 1rem;
		padding: 0.75rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs);
		grid-template-columns: 8rem 1fr;
	}

	.result-img {
		height: fit-content;
	}

	.result-main {
		display: grid;
		align-content: start;
		gap: 0.5rem;
	}

	.result-name {
		margin: 0.125rem 0 0;
		font-size: 1.125rem;
		font-weight: 650;
		line-height: 1.25;
		text-indent: 0.5em;
		word-break: normal;
		overflow-wrap: anywhere;
	}

	.buttons-container {
		display: flex;
		flex-direction: row;
		justify-content: right;
		gap: 0.75rem;
	}

	.buttons-container > * {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		width: fit-content;
		height: 2rem;
		padding: 0 1rem;
		border: none;
		border-radius: 0.375rem;
		font-size: 0.875rem;
		font-weight: 550;
		transition: background-color 0.12s ease;
	}

	.buttons-container > *:active {
		transform: scale(0.985);
	}

	.result-page-link {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.result-page-link:hover {
		background: var(--accent);
		color: var(--accent-foreground);
	}

	.toggle-takings-btn {
		gap: 0.25rem;
		min-width: 10rem;
		background: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
		user-select: none;
	}

	.toggle-takings-btn:hover {
		background-color: var(--primary-hov);
	}

	.toggle-takings-btn svg {
		display: inline-block;
		width: 1.125rem;
		height: 1.125rem;
		transition: transform 0.15s ease;
		transform: rotate(0deg);
	}

	.toggle-takings-btn.open svg {
		transform: rotate(-180deg);
	}

	.takings-container {
		max-height: 0;
		margin-top: 0.5rem;
		opacity: 0;
		transition:
			max-height 0.2s ease,
			opacity 0.2s ease,
			padding-top 0.2s ease;
		overflow: hidden;
	}

	.takings-container.open {
		max-height: 30rem;
		padding-top: 0.5rem;
		opacity: 1;
	}

	.takings-column-names {
		display: grid;
		align-content: center;
		gap: 0.5rem;
		margin-bottom: 0.375rem;
		grid-template-columns: 1fr 1fr 1fr;
		justify-items: center;
		border-top: 0.125rem solid var(--secondary);
		border-bottom: 0.125rem solid var(--secondary) !important;
	}

	.takings-column-names > span {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 550;
	}

	.takings-container > div {
		display: grid;
		gap: 0.5rem;
		padding: 0.375rem 0.25rem;
		color: var(--text);
		font-size: 1rem;
		font-weight: 550;
		grid-template-columns: 1fr 1fr 1fr;
	}

	.taking-row label {
		text-align: center;
	}
</style>
