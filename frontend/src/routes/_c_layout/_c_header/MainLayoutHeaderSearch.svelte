<script lang="ts">
	import { goto } from '$app/navigation';
	import { StringUtils } from '$lib/ts/utils/string-utils';

	import { onMount } from 'svelte';
	import HeaderSearchStartTypingMsg from './_c_search/HeaderSearchStartTypingMsg.svelte';
	import HeaderSearchInputEmptyMsg from './_c_search/HeaderSearchInputEmptyMsg.svelte';
	import HeaderSearchSuggestionsList from './_c_search/HeaderSearchSuggestionsList.svelte';
	import HeaderSearchInputNoSuggestionsMsg from './_c_search/HeaderSearchInputNoSuggestionsMsg.svelte';

	let inputEl = $state<HTMLInputElement>()!;
	let inputValue = $state('');
	let isFocused = $state(false);
	let headerSearchContainer = $state<HTMLDivElement>()!;
	let filteredUsers = $state<any[]>([]);
	let filteredTags = $state<any[]>([]);
	let filteredVokis = $state<any[]>([]);

	const dropdownId = StringUtils.rndStrWithPref('header-search-dropdown-');

	function handleKeydown(e: KeyboardEvent) {
		if ((e.altKey || e.metaKey) && e.code === 'KeyK') {
			e.preventDefault();
			if (inputValue.trim().length === 0) {
				inputValue = '';
			}
			inputEl.focus();
		}
		if (e.key === 'Enter' && isFocused) {
			submitSearch();
		}
		if (e.key === 'Escape' && isFocused) {
			inputEl.blur();
		}
	}

	function handleInput() {
		const val = inputValue.trim();
		if (!val) {
			filteredUsers = [];
			filteredTags = [];
			filteredVokis = [];
			return;
		}

		const lowerVal = val.toLowerCase();
		// const response =;
	}

	function submitSearch() {
		if (!inputValue.trim()) {
			return;
		}
		const q = inputValue.trim();
		goto(`/search-results?q=${encodeURIComponent(q)}`);
		isFocused = false;
	}
	let isInputEmpty = $derived(inputValue.trim().length === 0);

	onMount(() => {
		window.addEventListener('keydown', handleKeydown);
		return () => {
			window.removeEventListener('keydown', handleKeydown);
		};
	});
</script>

<div class="header-search" bind:this={headerSearchContainer}>
	<div
		class="input-wrapper"
		onclick={() => {
			inputEl.focus();
		}}
	>
		{#if isInputEmpty}
			<svg class="search-icon">
				<use href="#common-search-icon" />
			</svg>
		{:else}
			<svg
				class="cross-icon"
				onclick={(e: MouseEvent) => {
					e.stopPropagation();
					inputValue = '';
					inputEl.blur();
				}}
				aria-label="Clear search"
			>
				<use href="#common-cross-icon" />
			</svg>
		{/if}
		<input
			bind:this={inputEl}
			bind:value={inputValue}
			oninput={handleInput}
			onblur={() => {
				isFocused = false;
			}}
			onfocus={() => {
				isFocused = true;
			}}
			placeholder="Search Vokis, @users, #tags"
			name={StringUtils.rndStrWithPref('header-search-input-')}
			type="text"
			autocomplete="off"
			autocorrect="off"
			spellcheck="false"
			aria-autocomplete="list"
			aria-controls={dropdownId}
			aria-expanded={isFocused}
			role="combobox"
		/>
		{#if !isInputEmpty}
			<svg class="arrow-btn" onclick={submitSearch} aria-label="Search">
				<use href="#caret-right-icon" />
			</svg>
		{/if}
	</div>
	{#if isFocused}
		<div class="dropdown" id={dropdownId} role="listbox">
			{#if inputValue.length === 0}
				<HeaderSearchStartTypingMsg />
			{:else if inputValue.trim().length === 0}
				<HeaderSearchInputEmptyMsg />
			{:else if filteredUsers.length === 0 && filteredTags.length === 0 && filteredVokis.length === 0}
				<HeaderSearchInputNoSuggestionsMsg />
			{:else}
				<HeaderSearchSuggestionsList
					removeFocus={() => inputEl.blur()}
					{filteredUsers}
					{filteredTags}
					{filteredVokis}
				/>
			{/if}
			<button
				class="search-page-btn"
				onmousedown={(e: MouseEvent) => {
					e.stopPropagation();
					submitSearch();
				}}
			>
				Go to search page
				<svg>
					<use href="#caret-right-icon" />
				</svg>
			</button>
		</div>
	{/if}
</div>

<style>
	.header-search {
		position: relative;
		display: flex;
		align-items: center;
		width: 100%;
		max-width: 36rem;
		height: 100%;
		margin: 0 auto;
	}

	.input-wrapper {
		display: grid;
		align-items: center;
		width: 100%;
		height: var(--layout-header-search-height);
		padding: 0 0.75rem;
		border: 0.125rem solid transparent;
		border-radius: 100vw;
		background-color: var(--back);
		box-shadow: var(--shadow-xs);
		transition: border-color 0.08s ease;
		grid-template-columns: 1.5rem 1fr 1.5rem;
	}

	.input-wrapper:has(input:focus) {
		border-color: var(--primary);
		box-shadow: none;
	}

	input {
		min-width: 0;
		padding: 0 0.5rem;
		border: none;
		background: transparent;
		color: var(--text);
		font-size: 1rem;
		font-weight: 425;
		outline: none;
	}

	input::placeholder {
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 400;
	}

	.input-wrapper svg {
		width: 100%;
		aspect-ratio: 1/1;
		border-radius: 50%;
		transition: all 0.08s ease;
	}

	.search-icon {
		padding: 0.125rem;
		border-radius: 0 !important;
		color: var(--primary);
		stroke-width: 2.75;
		pointer-events: none;
	}

	.cross-icon {
		padding: 0.25rem;
		color: var(--muted-foreground);
		stroke-width: 2;
	}

	.cross-icon:hover {
		background-color: var(--secondary);
	}

	.arrow-btn {
		padding: 0;
		color: var(--primary);
		stroke-width: 2.5;
	}

	.arrow-btn:hover {
		padding: 0.125rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.dropdown {
		position: absolute;
		top: calc(100% + 0.25rem);
		left: 0;
		z-index: 9999;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		width: 100%;
		max-height: 20rem;
		padding: 0.5rem;
		border-radius: 0.75rem;
		background-color: var(--back);
		box-shadow: var(--shadow-xs), var(--shadow-md);
		overflow-y: auto;
		animation: dropdown-fade-in 0.1s ease;
	}

	.search-page-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		padding: 0.375rem 0.5rem;
		border: none;
		border-radius: 0.5rem;
		background: transparent;
		color: var(--primary);
		font-size: 0.875rem;
		font-weight: 500;
		cursor: pointer;
	}

	.search-page-btn:hover {
		background-color: var(--accent);
	}

	.search-page-btn > svg {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}

	@keyframes dropdown-fade-in {
		from {
			transform: scale(0.86);
		}

		to {
			transform: scaleX(1);
		}
	}

	@media (prefers-reduced-motion: reduce) {
		.dropdown {
			animation: none;
		}
	}
</style>
