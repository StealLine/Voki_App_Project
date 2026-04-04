<script lang="ts">
	import PicsStepPresetsRow from './_c_profile_pic_step/PicsStepPresetsRow.svelte';
	import PicsStepPreviewWithInput from './_c_profile_pic_step/PicsStepPreviewWithInput.svelte';

	interface Props {
		profilePic: string;
	}
	let { profilePic = $bindable() }: Props = $props();
	function updateCurrentPic(newPic: string) {
		if (!profilePic.startsWith('default-profile-pics/')) {
			let picsHistory = picPresets['previous'];

			const newHistory = [
				profilePic,
				...picsHistory.filter((p) => p !== profilePic && p !== newPic),
			'preset-profile-pics/basic-black.webp'
			];

			picPresets['previous'] = newHistory.slice(0, 5);
		}

		profilePic = newPic;
	}

	let picPresets: Record<string, string[]> = $state({
		['previous']: [
			'preset-profile-pics/basic-black.webp',
			'preset-profile-pics/basic-black.webp',
			'preset-profile-pics/basic-black.webp',
			'preset-profile-pics/basic-black.webp'
		],
		['pets']: [
			'preset-profile-pics/pets-marsita.webp',
			'preset-profile-pics/pets-ya-cat.webp',
			'preset-profile-pics/pets-monika.webp',
			'preset-profile-pics/pets-zara.webp'
		],
		['boykisser']: [
			'preset-profile-pics/boykisser-1.webp',
			'preset-profile-pics/boykisser-2.webp',
			'preset-profile-pics/boykisser-3.webp',
			'preset-profile-pics/boykisser-4.webp'
		],
		['dasha']: [
			'preset-profile-pics/dasha-1.webp',
			'preset-profile-pics/dasha-2.webp',
			'preset-profile-pics/dasha-3.webp',
			'preset-profile-pics/dasha-4.webp'
		]
	});
</script>

<div class="profile-pic-step">
	<PicsStepPreviewWithInput setCurrent={updateCurrentPic} currentPic={profilePic} />
	<div class="rows">
		<PicsStepPresetsRow
			rowName="Previous"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['previous']}
		/>
		<PicsStepPresetsRow rowName="Pets" onPicClick={updateCurrentPic} picsArr={picPresets['pets']} />
		<PicsStepPresetsRow
			rowName="Boykisser"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['boykisser']}
		/>
		<PicsStepPresetsRow
			rowName="Dasha"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['dasha']}
		/>
	</div>
</div>

<style>
	.profile-pic-step {
		display: grid;
		gap: 1.25rem;
	}

	.rows {
		display: grid;
		grid-template-columns: 1fr 1fr;
		grid-template-rows: 1fr 1fr;
		gap: 1.5rem 3rem;
		padding: 0 2rem;
		justify-items: center;
	}
</style>
