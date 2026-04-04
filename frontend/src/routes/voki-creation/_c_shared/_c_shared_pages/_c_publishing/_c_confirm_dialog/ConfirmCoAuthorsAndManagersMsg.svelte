<script lang="ts">
	import UsersCountWithExpandableHint from './_c_confirm_users_msg/UsersCountWithExpandableHint.svelte';

	interface Props {
		coAuthorIds: string[];
		userIdsToBecomeManagers: string[];
	}
	let { coAuthorIds, userIdsToBecomeManagers }: Props = $props();
</script>

<div class="co-authors-label">
	{#if coAuthorIds.length === 0 && userIdsToBecomeManagers.length === 0}
		You will be specified as the single author and manager of this Voki
	{:else if coAuthorIds.length > 0 && userIdsToBecomeManagers.length === 0}
		After publishing <UsersCountWithExpandableHint userIds={coAuthorIds} /> users will be specified as
		co-authors and you will become the only manager
	{:else}
		After publishing <UsersCountWithExpandableHint userIds={coAuthorIds} /> users will be specified as
		co-authors and <UsersCountWithExpandableHint userIds={userIdsToBecomeManagers} /> will become managers
	{/if}
</div>

<style>
	.co-authors-label {
		width: 100%;
		margin: 1rem 0 0.5rem;
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 425;
		line-height: 1.25;
		text-align: center;
		text-wrap: balance;
	}
</style>
