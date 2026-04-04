<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { relativeTime } from 'svelte-relative-time';
	import type { Snippet } from 'svelte';
	import BasicStarsDisplay from '../../../../../../../lib/components/BasicStarsDisplay.svelte';

	interface Props {
		userId: string;
		content: { name: 'default'; ratingValue: number } | { name: 'custom'; children: Snippet };
		dateTime?: Date;
		highlight?: { enabled: false } | { enabled: true; text: string };
	}
	let { userId, content, dateTime = undefined, highlight = { enabled: false } }: Props = $props();

	let user = UsersStore.Get(userId);
</script>

<div class="rating-item" class:highlighted={highlight.enabled}>
	{#if highlight.enabled}
		<label class="highlight-label"> {highlight.text}</label>
	{/if}

	<a href="/auhtors/{userId}" class="profile-pic-part profile-link-el">
		{#if user.state === 'loading'}
			<div class="profile-pic skeleton" aria-hidden="true" />
		{:else if user.state === 'errs'}
			<div class="profile-pic err">
				<svg><use href="#common-crossed-circle-icon" /></svg>
			</div>
		{:else if user.state === 'ok'}
			<img
				class="profile-pic"
				src={StorageBucketMain.fileSrc(user.data.profilePic)}
				alt="user profile pic"
			/>
		{/if}
	</a>
	<div class="content-part">
		<div class="name-with-date-content">
			{#if user.state === 'loading'}
				<div class="display-name loading skeleton" aria-hidden="true"></div>
			{:else if user.state === 'errs'}
				<a class="display-name profile-link-el error" href="/authors/{userId}">
					Could not load user name</a
				>
			{:else if user.state === 'ok'}
				<a class="display-name profile-link-el" href="/authors/{userId}">
					{user.data.displayName}</a
				>
			{/if}

			{#if dateTime}
				<span class="date" use:relativeTime={{ date: dateTime }} />
			{/if}
		</div>
		<a
			class="unique-name-content profile-link-el"
			href="/authors/{userId}"
			class:not-loaded={user.state !== 'ok'}
		>
			{#if user.state === 'ok'}
				@{user.data.uniqueName}
			{:else}
				go to user profile
			{/if}
		</a>
		<div class="main-content">
			{#if content.name === 'default'}
				<BasicStarsDisplay value={content.ratingValue} />
			{:else if content.name === 'custom'}
				{@render content.children()}
			{:else}
				<h1>Something went wrong. Content name: {(content as any).name}</h1>
			{/if}
		</div>
	</div>
</div>

<style>
	.rating-item {
		position: relative;
		display: grid;
		min-height: 4rem;
		padding: 0 0.5rem;
		border: 0.125rem solid var(--back);
		border-radius: 1rem;
		animation: var(--default-fade-in);
		grid-template-columns: auto 1fr;
		grid-template-columns: auto 1fr auto;
	}

	.rating-item.highlighted {
		padding-top: 0.375rem;
		padding-bottom: 0.25rem;
		border-color: var(--primary);
	}

	.highlight-label {
		position: absolute;
		top: -0.125rem;
		left: 1.25rem;
		padding: 0 0.125rem;
		background-color: var(--back);
		color: var(--primary);
		font-size: 0.875rem;
		font-weight: 600;
		transform: translateY(-50%);
	}

	.profile-link-el {
		cursor: pointer;
	}

	.profile-pic-part {
		padding: 0.125rem 0;
		align-self: flex-start;
		margin-top: 0.125rem;
	}

	.profile-pic {
		display: grid;
		width: 3rem;
		height: 3rem;
		border-radius: 100vw;
		background: var(--muted);
		aspect-ratio: 1/1;
		object-fit: cover;
		place-items: center;
	}

	img.profile-pic {
		box-shadow: var(--shadow-xs) inset;
	}

	.profile-pic.err {
		background: var(--muted);
	}

	.profile-pic.err svg {
		width: 2rem;
		height: 2rem;
		color: var(--muted-foreground);
	}

	.content-part {
		display: flex;
		flex-direction: column;
		min-width: 0;
		overflow: hidden;
	}

	.name-with-date-content {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		min-width: 0;
		overflow: hidden;
	}

	.display-name {
		height: 1.125rem;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 550;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	.display-name.loading {
		width: 12rem;
		margin-left: 0.25rem;
		border-radius: 0.5rem;
		color: transparent;
	}

	.display-name.error {
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.date {
		height: fit-content;
		padding-right: 0.25rem;
		margin-left: auto;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		white-space: nowrap;
	}

	.unique-name-content {
		max-width: 100%;
		min-height: 0.25rem;
		margin-left: 0.25rem;
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 480;
		align-self: flex-start;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	.rating-item:has(.profile-link-el:hover) .unique-name-content {
		color: var(--primary);
	}

	.unique-name-content.not-loaded {
		margin-top: -0.125rem;
		font-size: 0.75rem;
		opacity: 0;
		transition: opacity 0.06s ease-in;
	}

	.rating-item:has(.profile-link-el:hover) .unique-name-content.not-loaded {
		opacity: 1;
	}

	.skeleton {
		position: relative;
		width: 100%;
		background: var(--muted);
		overflow: hidden;
	}

	.skeleton::before {
		position: absolute;
		width: 150%;
		background: linear-gradient(
			-65deg,
			var(--muted) 0%,
			var(--muted) 15%,
			color-mix(in oklab, var(--secondary) 35%, var(--muted)) 30%,
			color-mix(in oklab, var(--back) 70%, var(--muted)) 50%,
			color-mix(in oklab, var(--secondary) 35%, var(--muted)) 70%,
			var(--muted) 85%,
			var(--muted) 100%
		);
		animation: skeleton-sweep 2s infinite;
		content: '';
		inset: 0;
	}

	@keyframes skeleton-sweep {
		0% {
			transform: translateX(-120%);
		}

		100% {
			transform: translateX(120%);
		}
	}
</style>
