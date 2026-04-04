<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';
	import BannerSection from './_c_page/BannerSection.svelte';
	import ProfilePicSection from './_c_page/ProfilePicSection.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView errs={data.response.errs} defaultMessage="Could not load settings" />
{:else}
	<div class="page-container">
		<!-- <BannerSection banner={data.response.data.banner} /> -->
		<div class="main-part">
			<ProfilePicSection profilePicData={data.response.data.profilePic} />
			<div class="main-part-right-side"></div>
		</div>
	</div>
{/if}

<style>
	.page-container {
		width: 100%;
		display: flex;
		flex-direction: column;
	}
	.main-part {
		display: grid;
		grid-template-columns: auto 1fr;
	}
	.main-part-right-side {
		background-color: darkgoldenrod;
		width: 100%;
		height: 12rem;
	}
</style>
