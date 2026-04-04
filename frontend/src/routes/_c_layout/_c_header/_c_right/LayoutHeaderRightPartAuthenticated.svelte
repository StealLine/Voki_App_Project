<script lang="ts">
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { toast } from 'svelte-sonner';
	import LogoutConfirmationDialog from './LogoutConfirmationDialog.svelte';
	import { onClickOutside } from 'runed';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		currentUserId: string;
	}
	let { currentUserId }: Props = $props();
	let showCtxMenu = $state(false);
	let containerRef = $state<HTMLDivElement>()!;
	let logoutConfirmationDialog = $state<LogoutConfirmationDialog>()!;
	async function logoutBtnPressed() {
		if (logoutConfirmationDialog) {
			logoutConfirmationDialog.open();
		} else {
			toast.error("Logout dialog couldn't be opened");
		}
	}
	onClickOutside(
		() => containerRef,
		() => (showCtxMenu = false)
	);
</script>

<div class="authenticated" bind:this={containerRef} onclick={() => (showCtxMenu = !showCtxMenu)}>
	<BasicUserDisplay
		userId={currentUserId}
		interactionLevel="JustDisplay"
		class="user-display unselectable"
	/>
	<div class="ctx-menu" class:show={showCtxMenu}>
		<button class="logout-btn" onclick={() => logoutBtnPressed()}>Logout</button>
	</div>
	<LogoutConfirmationDialog bind:this={logoutConfirmationDialog} />
</div>

<style>
	.authenticated {
		position: relative;
		display: grid;
		display: flex;
		width: fit-content;
		height: 100%;
		padding: 0 0.5rem;
		margin-left: auto;
		border-radius: var(--radius);
		grid-template-columns: 1fr auto;
	}

	.authenticated:not(:has(.ctx-menu:hover)):hover {
		background-color: var(--muted);
	}

	.authenticated > :global(.user-display) {
		background-color: transparent;

		--profile-pic-width: 2.25rem;
	}

	.ctx-menu {
		position: absolute;
		top: calc(100% + 0.125rem);
		right: 0;
		display: none;
		width: 100%;
		min-width: 7rem;
		padding: 0.25rem;
		border-radius: var(--radius);
		background-color: var(--back);
		box-shadow: var(--shadow-xs), var(--shadow);
	}

	.ctx-menu.show {
		display: block;
	}

	.logout-btn {
		width: 100%;
		padding: 0.25rem 1rem;
		border: none;
		border-radius: var(--radius);
		background-color: var(--red-3);
		color: var(--red-1);
		font-size: 1rem;
		font-weight: 500;
	}

	.logout-btn:hover {
		background-color: var(--red-4);
	}
</style>
