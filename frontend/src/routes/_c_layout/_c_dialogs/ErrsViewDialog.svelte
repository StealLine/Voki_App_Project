<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ErrUtils, type Err } from '$lib/ts/err';

	let dialog = $state<DialogWithCloseButton>()!;
	let errs = $state<Err[]>([]);

	export function open(list: Err[]) {
		errs = Array.isArray(list) ? list : [];
		dialog.open();
	}

	export function close() {
		dialog.close();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="errs-view-dialog" subheading="Errors">
	{#if errs.length === 0}
		<p class="empty">No errors to display.</p>
	{:else}
		{#each errs as e, i}
			<div class="err-card">
				<div class="err-heading">
					<span class="idx">#{i + 1}</span>
					<p class="msg">{e.message}</p>
				</div>

				<div class="fields">
					{#if ErrUtils.hasNonEmptyDetails(e)}
						<div class="field">
							<span class="label">Details</span>
							<span class="value">{e.details}</span>
						</div>
					{/if}
					{#if ErrUtils.hasSpecifiedCode(e)}
						<div class="field">
							<span class="label">Code</span>
							<span class="value">{e.code}</span>
						</div>
					{/if}
				</div>
			</div>
		{/each}
	{/if}
</DialogWithCloseButton>

<style>
	:global(#errs-view-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
		box-sizing: border-box;
	}

	.empty {
		color: var(--muted-foreground);
		font-size: 1rem;
	}

	.err-card {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		padding: 0.75rem;
		border-radius: 0.75rem;
		background-color: var(--red-1);
		color: var(--red-5);
		box-shadow: var(--err-shadow);
	}

	.err-heading {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: start;
		column-gap: 0.5rem;
	}

	.idx {
		font-size: 1rem;
		font-weight: 520;
		opacity: 0.9;
	}

	.msg {
		margin: 0;
		color: inherit;
		font-size: 1rem;
		font-weight: 500;
		line-height: 1.25rem;
		overflow-wrap: anywhere;
	}

	.fields {
		display: flex;
		flex-direction: column;
		gap: 0.375rem;
	}

	.field {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: start;
		column-gap: 0.5rem;
	}

	.label {
		min-width: 4.5rem;
		color: var(--muted-foreground);
		font-size: 1rem;
	}

	.value {
		color: inherit;
		font-size: 1rem;
		overflow-wrap: anywhere;
	}
</style>
