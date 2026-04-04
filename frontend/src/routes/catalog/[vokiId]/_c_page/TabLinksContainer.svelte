<script lang="ts">
	import type { VokiPageTab } from '../types';

	let {
		currentTab,
		changeTab,
		commentsCount,
		ratingsCount
	}: {
		currentTab: VokiPageTab;
		changeTab: (tab: VokiPageTab) => void;
		commentsCount: number;
		ratingsCount: number;
	} = $props<{
		currentTab: VokiPageTab;
		changeTab: (tab: VokiPageTab) => void;
		commentsCount: number;
		ratingsCount: number;
	}>();
</script>

<div class="tab-links-container unselectable">
	<div class="tab-link" class:active={currentTab === 'about'} onclick={() => changeTab('about')}>
		About
	</div>

	<div
		class="tab-link"
		class:active={currentTab === 'ratings'}
		onclick={() => changeTab('ratings')}
	>
		Ratings ({ratingsCount})
	</div>
	<div
		class="tab-link"
		class:active={currentTab === 'comments'}
		onclick={() => changeTab('comments')}
	>
		Comments ({commentsCount})
	</div>
</div>

<style>
	.tab-links-container {
		display: flex;
		flex-direction: row;
		justify-content: left;
		gap: 0.5rem;
		padding: 0;
	}

	.tab-link {
		position: relative;
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.375rem 0.75rem 0.125rem;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 450;
		text-align: center;
		letter-spacing: 0.25px;
		cursor: pointer;
	}

	.tab-link.active::after {
		position: absolute;
		bottom: -1px;
		display: block;
		width: 100%;
		height: 3px;
		border-radius: 60% 60% 0.25rem 0.25rem;
		background-color: var(--primary);
		animation: underline 0.18s ease-out;
		content: '';
	}

	@keyframes underline {
		from {
			width: 40%;
		}

		to {
			width: 100%;
		}
	}
</style>
