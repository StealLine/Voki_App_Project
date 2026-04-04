<script lang="ts">
	import { ErrUtils, type Err, type ErrType } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { Snippet } from 'svelte';
	import { getSignInDialogOpenFunction } from '../../routes/_c_layout/_ts_layout_contexts/sign-in-dialog-context';
	import DefaultErrBlock from './errs/DefaultErrBlock.svelte';

	const openSignInDialog = getSignInDialogOpenFunction();

	interface Props {
		errs: Err[];
		defaultMessage?: string;
		authRequiredMessage?: string;
		additionalParams?: { name: string; value: any }[];
		children?: Snippet;
	}
	let {
		errs,
		defaultMessage = undefined,
		authRequiredMessage = undefined,
		additionalParams = [],
		children
	}: Props = $props();

	function chooseMessageType(): ErrType | 'ListEmpty' | 'ErrList' {
		if (errs.length === 0) {
			return 'ListEmpty';
		}
		if (errs.length === 1 && errs[0]) {
			return ErrUtils.getErrTypeByCode(errs[0]);
		}
		return 'ErrList';
	}
	const messageType = chooseMessageType();
</script>

{#snippet errContentView(err: Err)}
	<div class="err-content-view">
		<p>Message: {err.message}</p>
		{#if !StringUtils.isNullOrWhiteSpace(err.details)}
			<p>Details: {err.details}</p>
		{/if}
		{#if err.code}
			<p>Code: {err.code}</p>
		{/if}
	</div>
{/snippet}
<div class="page-load-err">
	{#if messageType === 'ErrList' || messageType === 'Other' || messageType === 'Unspecified'}
		<h1>{defaultMessage ?? 'Something went wrong. Please, try again later'}</h1>
		<DefaultErrBlock errList={errs} />
	{:else if messageType === 'ListEmpty'}
		<h1>Something went wrong. Please, try again later</h1>
	{:else if messageType === 'AuthRequired'}
		<h1>{authRequiredMessage ?? 'Page sign in required'}</h1>
		{@render errContentView(errs[0])}
		<div class="btns-container">
			<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
			<button class="signup-btn" onclick={() => openSignInDialog('signup')}>Sign up</button>
		</div>
	{:else if messageType === 'NoAccess'}
		{#if defaultMessage}
			<h1>{defaultMessage}</h1>
		{/if}
		<h1>You don't have access to this page</h1>
		{@render errContentView(errs[0])}
	{:else if messageType === 'NotFound'}
		{#if defaultMessage}
			<h1>{defaultMessage}</h1>
		{/if}
		<h1>404</h1>
		{@render errContentView(errs[0])}
	{:else if messageType === 'NotImplemented'}
		{#if defaultMessage}
			<h1>{defaultMessage}</h1>
		{/if}
		<h1>Resource you are trying to access is not implemented yet</h1>
		{@render errContentView(errs[0])}
	{/if}
	<div class="additional-params">
		{#each additionalParams ?? [] as param}
			<p>{param.name}: {param.value}</p>
		{/each}
	</div>
	<div class="children">
		{@render children?.()}
	</div>
</div>

<style>
	.page-load-err {
		display: flex;
		flex-direction: column;
		margin: 4rem auto 0;
	}

	.err-content-view {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.err-content-view > p {
		color: var(--text);
		font-size: 1rem;
		font-weight: 420;
	}

	.additional-params {
		display: flex;
		flex-direction: column;
		margin-top: 1rem;
	}

	.additional-params > p {
		color: var(--text);
		font-size: 1rem;
		font-weight: 420;
	}

	.children {
		margin-top: 1rem;
	}
</style>
