<script lang="ts">
	interface Props {
		title: string;
		subtitle: string;
	}
	let { title, subtitle }: Props = $props();
</script>

<div class="container">
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		stroke-linecap="round"
		stroke-linejoin="round"
		class="ghost-icon"
	>
		<path
			d="M5 11a7 7 0 0 1 14 0v7a1.78 1.78 0 0 1 -3.1 1.4 1.65 1.65 0 0 0 -2.6 0 1.65 1.65 0 0 1 -2.6 0 1.65 1.65 0 0 0 -2.6 0A1.78 1.78 0 0 1 5 18v-7"
			stroke="currentColor"
		></path>
		<path d="M10 10h0.01" stroke="currentColor"></path>
		<path d="M14 10h0.01" stroke="currentColor"></path>
	</svg>

	<svg
		class="folder-icon"
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 16 16"
		fill="currentColor"
		aria-hidden="true"
	>
		<path
			d="M15.62980625 6.50481875c-0.21086875 -0.2923375 -0.54954375 -0.465325 -0.90999375 -0.46479375h-1.11998125V4.9200375c0 -0.61855 -0.5014375 -1.11998125 -1.1199875 -1.11998125h-4.8530375L5.68573125 2.344075c-0.19419375 -0.144775 -0.429775 -0.2233 -0.67199375 -0.224h-3.73375c-0.61855 0 -1.1199875 0.5014375 -1.1199875 1.1199875v10.07986875c0 0.3092875 0.25070625 0.56000625 0.55999375 0.55999375h12.5368375c0.24105 0.00001875 0.4550625 -0.15421875 0.53129375 -0.38289375l1.994275 -5.982825c0.1137625 -0.34148125 0.05701875 -0.71679375 -0.15259375 -1.0093875ZM5.0137375 3.2400625l2.090175 1.567975c0.096925 0.0727 0.2148375 0.11199375 0.33599375 0.112h5.0399375v1.1199875H3.3638625c-0.4821 -0.0000375 -0.910125 0.3084375 -1.0625875 0.7657875l-1.0212875 3.0631625V3.2400625Zm7.8399 9.519875H1.49698125l1.86688125 -5.599925H14.7198125Z"
			stroke-width="0.0625"
		></path>
	</svg>

	<h1>{title}</h1>
	<p class="subtitle">{subtitle}</p>

	<a href="/albums" class="back-link">
		<svg viewBox="0 0 24 24" aria-hidden="true"><use href="#caret-left-icon" /></svg>
		<span>Go back to all albums</span>
	</a>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		padding: 2rem 1.5rem;
		margin: 8vh auto 0;
		text-align: center;
		place-items: center;
	}

	.ghost-icon {
		height: 10rem;
		margin-bottom: -1.5rem;
		color: var(--primary);
		stroke-width: 2;
	}

	.folder-icon {
		stroke-width: 2;
		height: 12rem;
		color: var(--secondary-foreground);
	}

	h1 {
		margin: 0.5rem 0;
		color: var(--text);
		font-size: 2rem;
		font-weight: 650;
		line-height: 1;
	}

	.subtitle {
		margin: 0 auto;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
	}

	.back-link {
		display: inline-flex;
		align-items: center;
		padding: 0.375rem 0.75rem;
		margin-top: 1rem;
		border-radius: 0.6rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		text-decoration: none;
		letter-spacing: 0.25px;

		--icon-shift: 0;
	}

	.back-link svg {
		width: 1.5rem;
		height: 1.5rem;
		transition: transform 0.12s ease-in-out;
		transform: translateX(var(--icon-shift));
		stroke-width: 2.5;
	}

	.back-link:hover,
	.back-link:active,
	.back-link:focus {
		background-color: var(--primary-hov);

		--icon-shift: -0.25rem;
	}

	@media (prefers-reduced-motion: no-preference) {
		.ghost-icon {
			animation:
				ghost-appear 1.5s ease-in-out forwards,
				ghost-bob 3s ease-in-out infinite 4s;
		}

		@keyframes ghost-appear {
			0%,
			10% {
				color: var(--secondary-foreground);
				opacity: 0;
				transform: translate(0, 7rem) scale(0.4);
			}

			30% {
				opacity: 0.1;
			}

			60% {
				opacity: 1;
			}

			100% {
				color: var(--primary);
				opacity: 1;
				transform: translate(0, 0) scale(1);
			}
		}

		@keyframes ghost-bob {
			0%,
			100% {
				transform: translateY(0) scale(1);
			}

			50% {
				transform: translateY(-0.5rem) scale(0.96);
			}
		}
	}
</style>
