<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiTags } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { useDebounce } from 'runed';
	interface Props {
		isTagChosen: (tag: string) => boolean;
		chooseTag: (tag: string) => void;
		maxTagLength: number;
	}
	let { isTagChosen, chooseTag, maxTagLength }: Props = $props();

	let searchedTags: string[] = $state([]);
	let errs: Err[] = $state([]);
	let searchInput: string = $state('');
	let inputEl: HTMLInputElement;

	function onResetClick(event: MouseEvent) {
		setTimeout(() => {
			inputEl.focus();
		});
		searchInput = '';
		event.stopPropagation();
		errs = [];
	}

	const performSearch = useDebounce(async () => {
		const value = searchInput.trim();

		if (value === '') {
			searchedTags = [];
			errs = [];
			return;
		}
		const response = await ApiTags.fetchJsonResponse<{ tags: string[] }>(
			`/search?count=8&searchValue=${value}`,
			{ method: 'GET' }
		);

		if (response.isSuccess) {
			searchedTags = response.data.tags;
			errs = [];
		} else {
			errs = response.errs;
		}
	}, 260);
</script>

<div class="tags-searching">
	<div class="search-bar">
		<svg class="search-icon" viewBox="0 0 24 24" fill="none">
			<use href="#common-search-icon" />
		</svg>
		<input
			placeholder="Search for tag..."
			bind:value={searchInput}
			bind:this={inputEl}
			oninput={() => performSearch()}
			name={'tag-search-' + StringUtils.rndStr(12)}
			maxlength={maxTagLength}
		/>
		<svg class="reset-button" fill="none" viewBox="0 0 24 24" onclick={onResetClick}>
			<use href="#common-cross-icon" />
		</svg>
	</div>
	<div class="searched-tags-list">
		{#each searchedTags as tag}
			<div class="tag">
				#{tag}
				{#if isTagChosen(tag)}
					<svg class="added-icon"><use href="#common-check-icon" /></svg>
				{:else}
					<svg class="add-icon" onclick={() => chooseTag(tag)}><use href="#common-plus-icon" /></svg
					>
				{/if}
			</div>
		{/each}
		<label class="continue-typing-label">
			If you don't find the tag you need continue entering the name of the tag
		</label>
	</div>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.tags-searching {
		display: grid;
		gap: 0.5rem;
		width: 100%;
		grid-template-rows: auto 1fr auto;
	}

	.search-bar {
		display: grid;
		align-items: center;
		width: calc(100% - 2rem);
		height: 2.25rem;
		box-sizing: border-box;
		padding: 0 0.5rem 0 0.75rem;
		border: 0.125rem solid var(--secondary);
		border-radius: 2rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs), var(--shadow-md);
		transition: border-color 0.05s ease-out;
		justify-self: center;
		grid-template-columns: auto 1fr auto;
	}

	.search-bar:hover {
		border-color: var(--secondary-foreground);
		box-shadow: var(--shadow-md);
	}

	.search-bar:focus-within {
		border-color: var(--primary);
		box-shadow: none;
	}

	.search-bar > input {
		z-index: 2;
		width: 100%;
		height: 100%;
		box-sizing: border-box;
		padding-left: 0.125rem;
		border: none;
		background-color: transparent;
		font-size: 1rem;
		outline: none;
	}

	input::placeholder {
		color: var(--secondary-foreground);
	}

	.search-bar > svg {
		height: 1.125rem;
		stroke-width: 2.5;
		color: var(--secondary-foreground);
		transition: 0.2s ease;
	}

	.search-bar:focus-within .search-icon {
		color: var(--primary);
	}

	.reset-button {
		box-sizing: border-box;
		padding: 0.0625rem;
		border-radius: 0.25rem;
		color: var(--secondary-foreground);
		opacity: 0;
		cursor: pointer;
		visibility: hidden;
	}

	.reset-button:hover {
		background-color: var(--muted);
	}

	.search-bar > input:not(:placeholder-shown) ~ .reset-button {
		opacity: 1;
		visibility: visible;
	}

	.searched-tags-list {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		max-height: 20rem;
		overflow-y: auto;
		scrollbar-gutter: stable;
	}

	.tag {
		display: grid;
		align-items: center;
		width: 100%;
		padding: 0.25rem 0.5rem;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		box-shadow: var(--shadow-xs) inset;
		grid-template-columns: 1fr auto;
	}

	.tag svg {
		width: 1.375rem;
		height: 1.375rem;
		border-radius: 0.25rem;
		color: var(--secondary-foreground);
		stroke-width: 2.5;
	}

	.tag .add-icon:hover,
	.tag .add-icon:focus,
	.tag .add-icon:active {
		background-color: var(--muted);
		color: var(--primary);
	}

	.tag .added-icon {
		color: var(--primary);
		stroke-width: 2.875;
	}

	.continue-typing-label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		text-align: center;
		text-wrap: balance;
	}
</style>
