<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { ProfilePicShape } from '$lib/ts/users';
	import RoundedSquareProfilePicDisplay from './_c_profile_pic/RoundedSquareProfilePicDisplay.svelte';
	import SealProfilePicDisplay from './_c_profile_pic/SealProfilePicDisplay.svelte';
	import SquircleProfilePicDisplay from './_c_profile_pic/SquircleProfilePicDisplay.svelte';

	interface Props {
		key: string;
		shape: ProfilePicShape;
		class?: string;
		sizeInRem: number;
		borderWidthInRem: number;
		borderColor: string;
	}

	let { key, shape, class: className, sizeInRem, borderWidthInRem, borderColor }: Props = $props();
	const src = $derived(StorageBucketMain.fileSrc(key));
</script>

<div
	class="profile-picture {className}"
	style="
--profile-picture-size: {sizeInRem}rem;
--profile-pic-border-width: {borderWidthInRem}rem;
--profile-pic-border-color: {borderColor};
"
>
	{#if shape === 'Seal'}
		<SealProfilePicDisplay {src} />
	{:else if shape === 'Squircle'}
		<SquircleProfilePicDisplay {src} />
	{:else if shape === 'Circle' || shape === 'RoundedSquare20' || shape === 'RoundedSquare30'}
		<RoundedSquareProfilePicDisplay
			src={StorageBucketMain.fileSrc(key)}
			radiusPercent={shape === 'Circle'
				? 50
				: shape === 'RoundedSquare20'
					? 20
					: shape === 'RoundedSquare30'
						? 30
						: 0}
		/>
	{/if}
</div>

<style>
	.profile-picture {
		width: var(--profile-picture-size);
		height: var(--profile-picture-size);
	}
</style>
