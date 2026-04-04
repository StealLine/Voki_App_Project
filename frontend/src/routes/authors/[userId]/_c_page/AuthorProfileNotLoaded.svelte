<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';

	const { userId, errs }: { userId: string; errs: Err[] } = $props<{
		userId: string;
		errs: Err[];
	}>();
</script>

<div class="container">
	<h1>Could not load user profile</h1>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		width: 60rem;
		margin: 4rem auto;
	}
</style>
