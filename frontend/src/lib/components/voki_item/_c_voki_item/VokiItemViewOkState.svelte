<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { toast } from 'svelte-sonner';
	import { getVokiFlagsInfoDialogOpenFunction } from '../../../../routes/_c_layout/_ts_layout_contexts/voki-flags-info-dialog-context';
	import type { VokiItemHidableElements, VokiItemViewOkStateProps } from './voki-item';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	let {
		voki,
		link,
		onMoreBtnClick,
		flags,
		hide = []
	}: VokiItemViewOkStateProps & { hide?: VokiItemHidableElements[] } = $props();

	const openVokiFlagsInfoDialog = getVokiFlagsInfoDialogOpenFunction();
	function onFlagClick(e: MouseEvent) {
		e.preventDefault();
		openVokiFlagsInfoDialog();
	}
</script>

<a href={link} class="voki-item">
	<div class="cover-container">
		<img class="voki-cover" src={StorageBucketMain.fileSrc(voki.cover)} alt="voki cover" />
		{#if flags}
			<div class="flags-container">
				<div class="flag language interactable" onclick={onFlagClick}>
					<svg><use href="#languages-icons-{StringUtils.pascalToKebab(flags.language)}" /></svg>
				</div>
				{#if flags.hasMatureContent}
					<div class="flag mature-content interactable" onclick={onFlagClick}>
						<svg><use href="#common-mature-content-icon" /></svg>
					</div>
				{/if}
				{#if flags.authenticatedOnlyTaking}
					<div class="flag mature-content interactable" onclick={onFlagClick}>
						<svg><use href="#common-auth-only-taking-icon" /></svg>
					</div>
				{/if}
			</div>
		{/if}
	</div>
	<div class="bottom-items">
		<div class="name-line">
			{#if !hide.includes('Name')}
				<p class="voki-name">
					{voki.name}
				</p>
			{/if}
			{#if !hide.includes('MoreBtn')}
				<svg
					class="voki-more-btn interactable"
					onclick={(e) => {
						e.preventDefault();
						onMoreBtnClick(e);
					}}
				>
					<use href="#common-more-icon" />
				</svg>
			{/if}
		</div>
		{#if !hide.includes('Authors')}
			<div class="authors">
				<span class="by-label">by:</span>
				<BasicUserDisplay
					userId={voki.primaryAuthorId}
					interactionLevel="WholeComponentLink"
					class="interactable author-view"
				/>
				{#if voki.coAuthorIds.length > 0}
					<div
						class="co-authors interactable"
						onclick={(e) => {
							e.preventDefault();
							toast.error('You cannot see co-authors here yet. Please go to the voki page');
						}}
					>
						+ {voki.coAuthorIds.length}
					</div>
				{/if}
			</div>
		{/if}
	</div>
</a>

<style>
	.voki-item {
		display: flex;
		flex-direction: column;
		gap: var(--voki-cover-name-gap);
		gap: 0;
		width: 100%;
		height: fit-content;
		padding: 0.5rem;
		border-radius: calc(var(--voki-cover-border-radius) * 1.25);
		cursor: pointer;
	}

	.cover-container {
		position: relative;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		height: min-content;
	}

	.flags-container {
		position: absolute;
		top: 0.375rem;
		right: 0.375rem;
		display: flex;
		flex-direction: row-reverse;
		gap: 0.375rem;
	}

	.flag {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 1.675rem;
		height: 1.675rem;
		box-sizing: border-box;
		padding: 0.125rem;
		border-radius: 28%;
		background-color: var(--back);
		box-shadow: var(--shadow), var(--shadow-xs);
		transition:
			opacity 0.2s ease-in-out,
			transform 0.05s ease-in,
			background-color 0.08s ease-in;
	}

	.flag > svg {
		width: 100%;
		height: 100%;
	}

	.flag.language > svg {
		width: 100%;
		height: unset;
		border-radius: 0.25rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
	}

	.flag > svg:not(.language) {
		stroke-width: 1.6;
		color: var(--accent-foreground);
	}

	.voki-item:not(:has(.interactable:hover)):active {
		background-color: var(--secondary);
	}

	.voki-cover {
		width: 100%;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}

	.name-line {
		display: grid;
		grid-template-columns: 1fr auto;
		align-items: start;
	}

	.voki-name {
		display: flex;
		display: -webkit-box;
		flex-direction: row;
		margin: 0.25rem 0;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 420;
		line-height: calc(var(--voki-name-max-height) / 2);
		text-align: left;
		letter-spacing: 0.125px;
		-webkit-line-clamp: 2;
		line-clamp: 2;
		-webkit-box-orient: vertical;
		text-overflow: ellipsis;
		overflow: hidden;
		overflow-wrap: anywhere;
	}

	.voki-item:not(:has(.interactable:hover)):hover .flag {
		opacity: 0.6;
	}

	.flag:hover {
		box-shadow: var(--shadow-md), var(--shadow-xs);
		transform: scale(1.06);
	}

	.flag:active {
		background-color: var(--secondary);
		transform: scale(0.98);
	}

	.voki-item:not(:has(.interactable:hover)):hover .voki-name {
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
	}

	.voki-more-btn {
		height: calc(var(--voki-name-max-height) * 0.65);
		margin: 0.25rem 0;
		border-radius: 0.25rem;
		color: var(--text);
		aspect-ratio: 1/1;
		stroke-width: 3.2;
	}

	.voki-more-btn:hover {
		background-color: var(--muted);
	}

	.authors {
		display: grid;
		align-items: center;
		overflow: hidden;
		grid-template-columns: auto 1fr auto;
		padding: 0.125rem 0;
	}

	.authors > .by-label {
		margin-right: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 0.75rem;
		font-weight: 500;
		font-style: italic;
	}

	.authors > :global(.user-display) {
		--profile-pic-width: 2.25rem !important;
	}

	.authors > :global(.author-view.ok) {
		background-color: transparent;
	}

	.co-authors {
		padding: 0 0.25rem;
		margin: 0.125rem 0.25rem 0.125rem 0;
		border-radius: 0.25rem;
		color: var(--secondary-foreground);
		font-weight: 440;
		letter-spacing: -1.2px;
		box-shadow: var(--shadow);
		transition: all 0.06s ease-in;
	}

	.co-authors:hover {
		background-color: var(--secondary);
	}
</style>
