<script lang="ts">
	interface Props {
		authenticatedUserId: string;
	}

	let { authenticatedUserId }: Props = $props();
</script>

<div class="setup-saved-container">
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		stroke="currentColor"
		stroke-linecap="round"
		stroke-linejoin="round"
		class="user-round-check-icon"
	>
		<path d="M2 21a8 8 0 0 1 13.292-6" />
		<circle cx="10" cy="8" r="5" />
		<path d="m16 19 2 2 4-4" class="check-path" />
	</svg>

	<h1>You profile changes have been successfully saved</h1>
	<p>You can customize your profile even more here</p>
	<a class="profile-link" href={`/authors/${authenticatedUserId}`}>My profile</a>
</div>

<style>
	.setup-saved-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		margin: 10vh auto 2rem;
	}

	.setup-saved-container h1 {
		color: var(--muted-foreground);
		font-size: 1.75rem;
		font-weight: 650;
		letter-spacing: 0.1px;
	}

	.setup-saved-container p {
		margin-top: 2rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
	}

	.profile-link {
		padding: 0.25rem 1rem;
		margin-top: 0.375rem;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-weight: 450;
		letter-spacing: 0.2px;
	}

	.profile-link:hover,
	.profile-link:active {
		background-color: var(--primary-hov);
	}

	.user-round-check-icon {
		width: 10rem;
		height: 10rem;
		color: var(--primary);
		stroke-width: 2;
	}

	.check-path {
		stroke-dasharray: 9;
		stroke-dashoffset: 0;
		transition:
			stroke-dashoffset 0.125s ease-out,
			opacity 0.125s ease-out;
		animation: check-anim 0.75s ease-out backwards;
		animation-delay: 0.25s;
	}

	@keyframes check-anim {
		0% {
			stroke-dashoffset: 9;
			opacity: 0;
		}

		33% {
			stroke-dashoffset: 9;
			opacity: 0;
		}

		100% {
			stroke-dashoffset: 0;
			opacity: 1;
		}
	}
</style>
