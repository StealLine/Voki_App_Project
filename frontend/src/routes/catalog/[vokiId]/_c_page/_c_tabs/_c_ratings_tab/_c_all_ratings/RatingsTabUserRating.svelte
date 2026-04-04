<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import type { Err } from '$lib/ts/err';
	import type { VokiRatingData } from '../../../../types';
	import RatingsListItem from './RatingsListItem.svelte';
	import UserRatingStarsInput from './UserRatingStarsInput.svelte';

	interface Props {
		ratingState: ({ name: 'rated' } & VokiRatingData) | { name: 'not-rated'; userId: string };
		saveNewRating: (value: number) => Promise<ResponseResult<VokiRatingData>>;
	}

	let { ratingState, saveNewRating }: Props = $props();
	let ratingView = $derived(ratingState.name === 'rated' ? ratingState.value : 0);

	let savingErrs = $state<Err[]>([]);
	let updatingRatingValue = $state(false);
	let t: ReturnType<typeof setTimeout> | null = null;

	async function saveRating(newValue: number) {
		if (newValue < 1 || newValue > 5) {
			savingErrs = [{ message: `Rating must be between 1 and 5. Chosen value: ${newValue}` }];
			return;
		}
		updatingRatingValue = true;
		const response = await saveNewRating(newValue);
		if (response.isSuccess) {
			ratingState = { name: 'rated', ...response.data };
		} else {
			savingErrs = response.errs;
		}
		updatingRatingValue = false;
	}

	function onRatingInputClick(newVal: number) {
		savingErrs = [];
		ratingView = newVal;
		if (t) clearTimeout(t);
		t = setTimeout(() => {
			void saveRating(newVal);
			t = null;
		}, 350);
	}
</script>

{#snippet listItemContent()}
	<div class="list-item-content">
		<UserRatingStarsInput current={ratingView} onClick={(newVal) => onRatingInputClick(newVal)} />
		{#if updatingRatingValue}
			<div class="loader">
				<LinesLoader sizeRem={1.4} strokePx={2} color="var(--secondary-foreground)" />
			</div>
		{:else if ratingState.name === 'not-rated'}
			<label class="not-rated-label">{'< Rate this Voki'}</label>
		{/if}
	</div>
{/snippet}
<RatingsListItem
	userId={ratingState.userId}
	content={{ name: 'custom', children: listItemContent }}
	highlight={{ enabled: true, text: 'Your rating' }}
	dateTime={ratingState.name === 'rated' ? ratingState.dateTime : undefined}
/>
{#if savingErrs.length > 0}
	<div class="errs-list">
		<svg class="clear-errs-btn" onclick={() => (savingErrs = [])}
			><use href="#common-cross-icon" /></svg
		>
		{#each savingErrs as err}
			<div class="err-item">{err.message}</div>
		{/each}
	</div>
{/if}

<style>
	.list-item-content {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
	}

	.loader > :global(.container) {
		animation: fade-in-from-zero-with-delay 1s ease;
	}

	.not-rated-label {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
	}

	.errs-list {
		position: relative;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
		padding: 0.125rem 0.75rem;
		margin-top: 0.5rem;
		margin-bottom: 1rem;
		border-radius: 0.675rem;
		background-color: var(--red-1);
		box-shadow: var(--err-shadow);
	}

	.clear-errs-btn {
		position: absolute;
		top: 0.125rem;
		right: 0.25rem;
		width: fit-content;
		height: 1.25rem;
		padding: 0.125rem;
		border-radius: 1rem;
		color: var(--red-5);
		transition: all 0.04s ease-in;
		cursor: pointer;
		aspect-ratio: 1/1;
		stroke-width: 2.2;
	}

	.clear-errs-btn:hover {
		background-color: var(--red-5);
		color: var(--red-1);
	}

	.err-item {
		color: var(--red-5);
		font-size: 1rem;
		font-weight: 500;
	}
</style>
