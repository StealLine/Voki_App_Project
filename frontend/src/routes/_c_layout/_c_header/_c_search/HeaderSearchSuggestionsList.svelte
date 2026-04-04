<script lang="ts">
	import { goto } from '$app/navigation';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		removeFocus: () => void;
		filteredUsers: any[];
		filteredTags: any[];
		filteredVokis: any[];
	}
	let { removeFocus, filteredUsers, filteredTags, filteredVokis }: Props = $props();
	function navigateToUser(userId: string) {
		goto(`/authors/${userId}`);
		removeFocus();
	}

	function navigateToTag(t: string) {
		goto(`/tags/${t}`);
		removeFocus();
	}

	function navigateToVoki(vokiId: string) {
		goto(`/catalog/${vokiId}`);
		removeFocus();
	}

	function handleKeydown(e: KeyboardEvent, action: () => void) {
		if (e.key === 'Enter') {
			action();
		}
	}
</script>

<div class="suggestions-container">
	{#if filteredUsers.length > 0}
		<div class="section-label">Users</div>
		{#each filteredUsers as user}
			<button
				class="suggestion-item user"
				onclick={() => navigateToUser(user.id)}
				onkeydown={(e) => handleKeydown(e, () => navigateToUser(user.id))}
			>
				<BasicUserDisplay userId={user.id} interactionLevel="JustDisplay" />
				<svg class="chevron" viewBox="0 0 24 24" fill="none" stroke="currentColor">
					<path d="M9 18l6-6-6-6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
				</svg>
			</button>
		{/each}
	{/if}

	{#if filteredTags.length > 0}
		<div class="section-label">Tags</div>
		{#each filteredTags as tag}
			<button
				class="suggestion-item tag"
				onclick={() => navigateToTag(tag.name)}
				onkeydown={(e) => handleKeydown(e, () => navigateToTag(tag.name))}
			>
				<div class="icon-box tag-icon">
					<span class="hash">#</span>
				</div>
				<div class="text-content">
					<span class="main-text">{tag.name}</span>
					<span class="sub-text">{tag.usageCount} uses</span>
				</div>
			</button>
		{/each}
	{/if}

	{#if filteredVokis.length > 0}
		<div class="section-label">Vokis</div>
		{#each filteredVokis as voki}
			<button
				class="suggestion-item voki"
				onclick={() => navigateToVoki(voki.id)}
				onkeydown={(e) => handleKeydown(e, () => navigateToVoki(voki.id))}
			>
				<div class="icon-box voki-icon">
					<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
						<path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20" />
						<path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z" />
					</svg>
				</div>
				<span class="main-text voki-title">{voki.title}</span>
			</button>
		{/each}
	{/if}
</div>

<style>
	.suggestions-container {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.section-label {
		padding: 0.5rem 0.75rem 0.25rem;
		margin-top: 0.25rem;
		color: var(--muted-foreground);
		font-size: 0.75rem;
		font-weight: 600;
		text-transform: uppercase;
		letter-spacing: 0.05em;
	}

	.section-label:first-child {
		margin-top: 0;
	}

	.suggestion-item {
		position: relative;
		display: flex;
		align-items: center;
		gap: 0.75rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		border: none;
		border-radius: 0.5rem;
		background: transparent;
		color: var(--foreground);
		text-align: left;
		transition: all 0.15s ease;
		cursor: pointer;
		outline: none;
	}

	.suggestion-item:hover,
	.suggestion-item:focus-visible {
		background-color: var(--muted);
	}

	.icon-box {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 2rem;
		height: 2rem;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		color: var(--foreground);
		flex-shrink: 0;
	}

	.hash {
		font-size: 1.125rem;
		font-weight: bold;
	}

	.voki-icon svg {
		width: 1.125rem;
		height: 1.125rem;
	}

	.text-content {
		display: flex;
		flex-direction: column;
		min-width: 0;
	}

	.main-text {
		font-size: 0.9375rem;
		font-weight: 500;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.voki-title {
		flex: 1;
	}

	.sub-text {
		color: var(--muted-foreground);
		font-size: 0.75rem;
	}

	.chevron {
		width: 1rem;
		height: 1rem;
		margin-left: auto;
		color: var(--muted-foreground);
		opacity: 0;
		transition: opacity 0.1s;
	}

	.suggestion-item:hover .chevron {
		opacity: 1;
	}
</style>
