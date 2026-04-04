<script lang="ts">
	import { ApiUserProfiles } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { useDebounce } from 'runed';
	import type { UserPreviewWithInvitesSettings } from '../../types';

	interface Props {
		searchedUsers: UserPreviewWithInvitesSettings[];
		searchBarInputVal: string;
	}

	let { searchedUsers = $bindable(), searchBarInputVal = $bindable() }: Props = $props();

	let errs: Err[] = $state([]);
	let inputEl: HTMLInputElement;

	function onResetClick(event: MouseEvent) {
		setTimeout(() => {
			inputEl.focus();
		});
		searchBarInputVal = '';
		searchedUsers = [];
		event.stopPropagation();
	}

	const performSearch = useDebounce(async () => {
		const value = searchBarInputVal.trim();

		if (value === '') {
			searchedUsers = [];
			errs = [];
			return;
		}
		const response = await ApiUserProfiles.fetchJsonResponse<{
			users: UserPreviewWithInvitesSettings[];
		}>(`/users/search-to-invite?searchValue=${value}&limit=20`, { method: 'GET' });

		if (response.isSuccess) {
			searchedUsers = response.data.users;
			errs = [];
		} else {
			searchedUsers = [];
			errs = response.errs;
		}
	}, 300);
</script>

<div class="search-bar">
	<svg class="search-icon" viewBox="0 0 24 24" fill="none">
		<path
			d="M17.5 17.5L22 22"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M20 11C20 6.02944 15.9706 2 11 2C6.02944 2 2 6.02944 2 11C2 15.9706 6.02944 20 11 20C15.9706 20 20 15.9706 20 11Z"
			stroke="currentColor"
			stroke-linejoin="round"
		/>
	</svg>
	<input
		placeholder="Search for users..."
		bind:value={searchBarInputVal}
		bind:this={inputEl}
		oninput={() => performSearch()}
		name={'users-search-' + StringUtils.rndStr(12)}
	/>
	<svg class="reset-button" fill="none" viewBox="0 0 24 24" onclick={onResetClick}>
		<use href="#common-cross-icon" />
	</svg>
</div>

<style>
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
		transition:
			border-radius 0.15s ease-out,
			width 0.15s ease-out;
		grid-template-columns: auto 1fr auto;
		place-self: center center;
		place-self: center;
	}

	.search-bar:hover,
	.search-bar:focus-within {
		border-color: var(--primary);
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
		transition: inherit;
	}

	.search-bar:hover .search-icon,
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
</style>
