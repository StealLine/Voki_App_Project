<script lang="ts">
	import UserProfileNotLoaded from './_c_page/AuthorProfileNotLoaded.svelte';
	import type { PageProps } from './$types';
	import AuthorBannerDisplay from './_c_page/AuthorBannerDisplay.svelte';
	import AuthorIdentityBlock from './_c_page/AuthorIdentityBlock.svelte';
	import AuthorProfileSidebar from './_c_page/AuthorProfileSidebar.svelte';
	import AuthorProfileActions from './_c_page/AuthorProfileActions.svelte';
	import AuthorProfilePicDisplay from './_c_page/AuthorProfilePicDisplay.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<UserProfileNotLoaded errs={data.response.errs} userId={data.userId!} />
{:else}
	<div class="author-page">
		<AuthorBannerDisplay banner={data.response.data.banner} />
		<div class="author-page-body">
			<div class="author-page-main">
				<div class="top-part">
					<AuthorProfilePicDisplay profilePicData={data.response.data.profilePic} />
					<AuthorIdentityBlock
						displayName={data.response.data.displayName}
						uniqueName={data.response.data.uniqueName}
						pronouns={data.response.data.pronouns}
						status={data.response.data.status}
					/>
					<AuthorProfileActions profileId={data.userId} />
				</div>
			</div>

			<aside class="author-page-sidebar">
				<AuthorProfileSidebar
					aboutMe={data.response.data.aboutMe}
					knownLanguages={data.response.data.knownLanguages}
					links={data.response.data.links}
					favouriteTags={data.response.data.favouriteTags}
					favouriteAuthorIds={data.response.data.favouriteAuthorIds}
				/>
			</aside>
		</div>
	</div>
{/if}

<style>
	.author-page {
		width: 100%;
		margin-top: 1rem;
	}

	.author-page-body {
		display: grid;
		grid-template-columns: 1fr 20rem;
		align-items: start;
	}

	.author-page-main {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
		min-width: 0;
	}
	.top-part {
		display: grid;
		grid-template-columns: auto 1fr 11rem;
		gap: 1.25rem;
		min-width: 0;
		padding: 0 3rem;
	}
	.author-page-sidebar {
		width: 100%;
		height: 20rem;
		margin-top: 0.75rem;
	}
</style>
