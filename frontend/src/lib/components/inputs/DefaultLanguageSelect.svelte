<script lang="ts">
	import { LanguageUtils, type Language } from '$lib/ts/language';

	let { value = $bindable(), className = '' }: { value: Language; className?: string } = $props<{
		value: Language;
		className?: string;
	}>();
</script>

<select class="language-select {className}" bind:value>
	{#each LanguageUtils.values() as lang}
		<option value={lang}>{LanguageUtils.name(lang)}</option>
	{/each}
</select>

<style>
	.language-select {
		box-sizing: border-box;
		padding: 0 0.75rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 1rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 440;
		transition: border-radius 0.2s ease-out;
		appearance: none;
		outline: none;
	}

	.language-select:hover,
	.language-select:focus,
	.language-select:focus-within,
	.language-select:has(:hover) {
		border-color: var(--primary);
		border-radius: 0.5rem;
	}

	.language-select option {
		background-color: var(--secondary);
		color: var(--text);
	}

	.language-select option:hover {
		background-color: var(--accent);
	}
</style>
