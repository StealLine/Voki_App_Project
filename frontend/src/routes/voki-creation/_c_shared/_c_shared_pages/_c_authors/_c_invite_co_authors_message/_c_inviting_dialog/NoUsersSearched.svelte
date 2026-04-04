<div class="empty-state">
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M14 8.5C14 5.73858 11.7614 3.5 9 3.5C6.23858 3.5 4 5.73858 4 8.5C4 11.2614 6.23858 13.5 9 13.5C11.7614 13.5 14 11.2614 14 8.5Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M16 20.5C16 16.634 12.866 13.5 9 13.5C5.13401 13.5 2 16.634 2 20.5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M22 9L19.5 11.5M19.5 11.5L17 14M19.5 11.5L22 14M19.5 11.5L17 9"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>

	<p class="empty-state-title">No matches found</p>
	<p class="empty-state-subtitle">Try to search with other part of the name</p>
</div>

<style>
	.empty-state {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 2rem;
		text-align: center;
		animation: fade-in-from-zero 0.15s ease-in;
	}

	.empty-state > svg {
		width: 4rem;
		height: 4rem;
		color: var(--primary);
		stroke-width: 1.5;
	}

	.empty-state-title {
		margin: -0.25rem 0 0.25rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
	}

	.empty-state-subtitle {
		max-width: 30rem;
		color: var(--muted-foreground);
		font-size: 0.9375rem;
		line-height: 1.5;
	}
</style>
