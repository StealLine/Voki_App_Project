<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { TextareaAutosize } from 'runed';

	let { text = $bindable() }: { text: string } = $props<{ text: string }>();
	let textarea = $state<HTMLTextAreaElement>(null!);
	new TextareaAutosize({ element: () => textarea, input: () => text });
</script>

<textarea
	class="answer-textarea"
	bind:this={textarea}
	bind:value={text}
	name={StringUtils.rndStr()}
/>

<style>
	.answer-textarea {
		width: 100%;
		min-height: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.answer-textarea:hover {
		outline-color: var(--secondary-foreground);
	}

	.answer-textarea:focus {
		outline-color: var(--primary);
	}
</style>
