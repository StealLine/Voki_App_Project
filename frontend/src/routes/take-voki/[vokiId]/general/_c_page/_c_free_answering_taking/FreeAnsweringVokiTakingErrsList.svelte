<script lang="ts">
	import { ErrUtils, type Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	interface Props {
		errs: (Err & { questionOrder?: number })[];
		jumpToSpecificQuestion: (questionOrder: number) => Err[];
	}
	let { errs, jumpToSpecificQuestion }: Props = $props();
	function onErrClick(err: Err & { questionOrder?: number }) {
		if (isErrWithOrder(err)) {
			const resErrs = jumpToSpecificQuestion(err.questionOrder!);
			if (resErrs.length > 0) toast.error(resErrs[0].message);
		}
	}
	function isErrWithOrder(err: Err & { questionOrder?: number }): boolean {
		return err.questionOrder !== undefined && err.questionOrder !== null;
	}
</script>

<div class="errs">
	{#each errs as err}
		<div class="err" class:can-jump={isErrWithOrder(err)} onclick={() => onErrClick(err)}>
			<label class="message">
				{err.message}
			</label>
			<label class="details">
				{#if ErrUtils.hasNonEmptyDetails(err)}
					Details: {err.details}
				{/if}
			</label>
		</div>
	{/each}
</div>

<style>
	.errs {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
		margin: 1rem 0;
	}

	.err {
		display: flex;
		flex-direction: column;
		width: 100%;
		padding: 0.125rem 0.25rem;
		border-radius: 0.25rem;
		background-color: var(--red-1);
		color: var(--red-5);
		box-shadow: var(--err-shadow);
	}

	.message {
		font-size: 1.125rem;
		font-weight: 450;
		text-indent: 0.5em;
	}

	.details {
		padding: 0 0.25rem;
		font-size: 1rem;
		font-weight: 420;
		text-indent: 0.5em;
	}

	.err.can-jump:hover {
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
	}
</style>
