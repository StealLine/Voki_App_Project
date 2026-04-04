<script lang="ts">
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import VokiCreationLayoutNavBar from '../../../_c_layout/VokiCreationLayoutNavBar.svelte';
	import { getVokiCreationPageContext } from '../../../voki-creation-page-context';
	import UnsavedChangesConfirmDialog from './_c_dialogs/UnsavedChangesConfirmDialog.svelte';
	import { goto } from '$app/navigation';

	interface Props {
		vokiId: string | undefined;
	}
	let { vokiId }: Props = $props();
	function withBasePath(path: string) {
		return `/voki-creation/general/${vokiId}/${path}`;
	}

	let confirmDialog = $state<UnsavedChangesConfirmDialog>()!;
	let pendingUrl = $state<string>();

	const currentPageContext = getVokiCreationPageContext();
	function handleBeforeNavigate(href: string): boolean {
		if (currentPageContext.currentPageState?.hasAnyUnsavedChanges) {
			pendingUrl = href;
			confirmDialog?.open();
			return false;
		}
		return true;
	}

	function handleConfirm() {
		if (pendingUrl) {
			goto(pendingUrl);
			pendingUrl = undefined;
		}
	}

	function handleCancel() {
		pendingUrl = undefined;
	}
</script>

{#if vokiId}
	<VokiCreationLayoutNavBar
		onBeforeNavigate={handleBeforeNavigate}
		links={[
			{ icon: authorsIcon, name: 'Authors', href: withBasePath('authors') },
			{ icon: mainIcon, name: 'Main Info', href: withBasePath('main') },
			{ icon: questionIcon, name: 'Questions', href: withBasePath('questions') },
			{ icon: resultsIcon, name: 'Results', href: withBasePath('results') },
			{ icon: publishingIcon, name: 'Publishing', href: withBasePath('publishing') }
		]}
	/>
{:else}
	<ErrView err={{ message: 'Incorrect link. Voki id is not specified' }} />
{/if}

<UnsavedChangesConfirmDialog
	bind:this={confirmDialog}
	goToPage={handleConfirm}
	onBeforeCancel={handleCancel}
/>
{#snippet authorsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M1.87399 17.625c0 0.6962 0.27656 1.3639 0.76885 1.8562 0.49228 0.4922 1.15996 0.7688 1.85615 0.7688 0.6962 0 1.36388 -0.2766 1.85616 -0.7688 0.49228 -0.4923 0.76884 -1.16 0.76884 -1.8562 0 -0.3447 -0.06789 -0.6861 -0.19981 -1.0045 -0.13192 -0.3185 -0.32528 -0.6079 -0.56903 -0.8517 -0.24376 -0.2437 -0.53313 -0.4371 -0.85161 -0.569C5.18506 15.0679 4.84371 15 4.49899 15s-0.68606 0.0679 -1.00454 0.1998c-0.31848 0.1319 -0.60786 0.3253 -0.85161 0.569 -0.24376 0.2438 -0.43711 0.5332 -0.56903 0.8517 -0.13192 0.3184 -0.19982 0.6598 -0.19982 1.0045Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M8.24899 23.25c-0.36249 -0.6797 -0.90287 -1.248 -1.56339 -1.6443C6.02508 21.2093 5.26928 21 4.49899 21c-0.77028 0 -1.52609 0.2093 -2.1866 0.6057 -0.66052 0.3963 -1.20091 0.9646 -1.563397 1.6443"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M16.874 17.625c0 0.3447 0.0679 0.6861 0.1998 1.0045 0.1319 0.3185 0.3253 0.6079 0.569 0.8517 0.2438 0.2437 0.5332 0.4371 0.8516 0.569 0.3185 0.1319 0.6599 0.1998 1.0046 0.1998 0.3447 0 0.6861 -0.0679 1.0045 -0.1998 0.3185 -0.1319 0.6079 -0.3253 0.8516 -0.569 0.2438 -0.2438 0.4372 -0.5332 0.5691 -0.8517 0.1319 -0.3184 0.1998 -0.6598 0.1998 -1.0045 0 -0.3447 -0.0679 -0.6861 -0.1998 -1.0045 -0.1319 -0.3185 -0.3253 -0.6079 -0.5691 -0.8517 -0.2437 -0.2437 -0.5331 -0.4371 -0.8516 -0.569 -0.3184 -0.1319 -0.6598 -0.1998 -1.0045 -0.1998 -0.3447 0 -0.6861 0.0679 -1.0046 0.1998 -0.3184 0.1319 -0.6078 0.3253 -0.8516 0.569 -0.2437 0.2438 -0.4371 0.5332 -0.569 0.8517 -0.1319 0.3184 -0.1998 0.6598 -0.1998 1.0045Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M23.249 23.25c-0.3625 -0.6797 -0.9029 -1.248 -1.5634 -1.6443C21.0251 21.2093 20.2693 21 19.499 21s-1.5261 0.2093 -2.1866 0.6057c-0.6605 0.3963 -1.2009 0.9646 -1.5634 1.6443"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M9.37399 3.375c0 0.34472 0.0679 0.68606 0.19982 1.00454 0.13192 0.31848 0.32527 0.60786 0.56899 0.85162 0.2438 0.24375 0.5332 0.43711 0.8516 0.56902C11.3129 5.9321 11.6543 6 11.999 6c0.3447 0 0.6861 -0.0679 1.0045 -0.19982 0.3185 -0.13191 0.6079 -0.32527 0.8516 -0.56902 0.2438 -0.24376 0.4372 -0.53314 0.5691 -0.85162 0.1319 -0.31848 0.1998 -0.65982 0.1998 -1.00454s-0.0679 -0.68606 -0.1998 -1.00454c-0.1319 -0.31848 -0.3253 -0.60786 -0.5691 -0.85162 -0.2437 -0.24375 -0.5331 -0.43711 -0.8516 -0.569024C12.6851 0.817898 12.3437 0.75 11.999 0.75c-0.3447 0 -0.6861 0.067898 -1.0046 0.199816 -0.3184 0.131914 -0.6078 0.325274 -0.8516 0.569024 -0.24372 0.24376 -0.43707 0.53314 -0.56899 0.85162 -0.13192 0.31848 -0.19982 0.65982 -0.19982 1.00454Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M15.248 8.25002c-0.4008 -0.47055 -0.8989 -0.84847 -1.4601 -1.10762 -0.5611 -0.25916 -1.1718 -0.39338 -1.7899 -0.39338 -0.6181 0 -1.2288 0.13422 -1.79 0.39338 -0.56108 0.25915 -1.05925 0.63707 -1.46001 1.10762"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M9.04999 19.707c1.91521 0.7301 4.03321 0.7237 5.94401 -0.018"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M6.34799 6c-0.82097 0.7689 -1.47533 1.69816 -1.92256 2.73023C3.9782 9.7623 3.74763 10.8752 3.74799 12c0 0.253 0.015 0.5 0.038 0.75"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M20.211 12.75c0.022 -0.248 0.038 -0.5 0.038 -0.75 0.0005 -1.1248 -0.23 -2.23779 -0.6772 -3.26988C19.1245 7.69802 18.4701 6.76879 17.649 6"
		></path>
	</svg>
{/snippet}
{#snippet mainIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path d="M3 4H7" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 10H7" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 15.5H21" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 21.5H21" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<rect
			x="11"
			y="2.5"
			width="10"
			height="8"
			rx="2"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}

{#snippet questionIcon()}
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		stroke-linecap="round"
		stroke-linejoin="round"
	>
		<path d="M1 7h16" stroke="currentColor"></path>
		<path d="M1 13H12" stroke="currentColor"></path>
		<path d="M1 19h12" stroke="currentColor"></path>
		<path d="M18.5 19v0.01" stroke="currentColor"></path>
		<path
			stroke="currentColor"
			d="M18.5 16a2.003 2.003 0 0 0 0.914 -3.782 1.98 1.98 0 0 0 -2.414 0.483"
		/>
	</svg>
{/snippet}

{#snippet resultsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<circle
			cx="18"
			cy="5"
			r="3"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<circle
			cx="6"
			cy="19"
			r="3"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M12 5H8.5C6.567 5 5 6.567 5 8.5C5 10.433 6.567 12 8.5 12H15.5C17.433 12 19 13.567 19 15.5C19 17.433 17.433 19 15.5 19H12"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}
{#snippet publishingIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M19 13C19 17.4183 15.4183 21 11 21C6.58172 21 3 17.4183 3 13C3 8.58172 6.58172 5 11 5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M14 3H18C19.4142 3 20.1213 3 20.5607 3.43934C21 3.87868 21 4.58579 21 6V10M20 4L11 13"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
{/snippet}
