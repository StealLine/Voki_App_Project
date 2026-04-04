<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';

	interface Props {
		aboutMe: Extract<PossiblyHidden<string>, { showOnProfile: true }>;
	}

	let { aboutMe }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="About me">
	<p class="value">{aboutMe.value}</p>
</ProfileSidebarSectionContainer>

<style>
	.value {
		word-break: break-word;
		font-size: 1rem;
		line-height: 1.25;
		color: var(--muted-foreground);
		font-weight: 450;
		letter-spacing: 0.125px;
	}
</style>
