<script lang="ts">
	interface Props {
		chosenTags: Set<string>;
		removeTag: (tag: string) => void;
	}
	let { chosenTags, removeTag }: Props = $props();
</script>


	<div class="chosen-tags-list">
		{#each chosenTags as tag}
			<div class="tag">
				<span>#{tag}</span>
				<svg onclick={() => removeTag(tag)}><use href="#common-cross-icon" /></svg>
			</div>
		{/each}
	</div>

<style>

	.chosen-tags-list {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		align-items: center;
		gap: 1rem 0.5rem;
		height: fit-content;
	}

	.tag {
		display: grid;
		align-items: center;
		gap: 0.125rem;
		height: fit-content;
		padding: 0.125rem 0.675rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		cursor: default;
		grid-template-columns: 1fr auto;
	}

	.tag span {
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.1px;
	}

	.tag svg {
		width: 1.125rem;
		height: 1.125rem;
		color: var(--secondary-foreground);
		cursor: pointer;
		stroke-width: 2;
	}

	.tag svg:hover {
		color: var(--muted-foreground);
		stroke-width: 2.5;
	}

	.tag:has(> svg:hover) > span {
		animation: shake 1s ease-in-out infinite;
        animation-delay: 0.5s;
	}

	@keyframes shake {
		0%,
		100% {
			transform: scale(1);
		}

		25% {
			transform: scale(0.98);
		}

        50% {
            transform: scale(1);
        }

        75% {
            transform: scale(0.965);
        }
	}
</style>
