<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { watch } from 'runed';
	import { ApiAlbums, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { VokiAlbumPreviewData } from '../../types';
	import AlbumColorInput from '$lib/components/inputs/albums/AlbumColorInput.svelte';
	import AlbumIconPicker from '$lib/components/inputs/albums/AlbumIconPicker.svelte';
	import { IconUtils } from '$lib/ts/utils/icons-utils';

	interface Props {
		updateParent: (newData: VokiAlbumPreviewData) => void;
	}
	let { updateParent }: Props = $props();

	let album: VokiAlbumPreviewData | undefined = $state(undefined)!;
	let dialog = $state<DialogWithCloseButton>()!;

	let name = $state('');
	let icon = $state('albums-bookmark-1-icon');

	let mainColorInput = $state('#5B57E2');
	let secondaryColorInput = $state('#bfbfbf');
	let useTwoColors = $state(false);

	let savingErrs: Err[] = $state([]);

	const secondaryColorWithIfTwoCheck = $derived(
		useTwoColors ? secondaryColorInput : mainColorInput
	);
	const mainColorInputLabel = $derived(useTwoColors ? 'Main color' : 'Album color');

	watch(
		[() => name, () => mainColorInput, () => secondaryColorInput, () => icon, () => useTwoColors],
		() => {
			savingErrs = [];
		}
	);

	function initFromAlbum(a: VokiAlbumPreviewData) {
		const aData = a as any;

		name = aData.name ?? '';
		icon = aData.icon ?? aData.iconId ?? 'albums-bookmark-1-icon';

		const main = aData.mainColor ?? aData.color ?? aData.colors?.mainColor;
		const secondary = aData.secondaryColor ?? aData.colors?.secondaryColor ?? main ?? '#5B57E2';

		mainColorInput = main ?? '#5B57E2';
		secondaryColorInput = secondary ?? mainColorInput;
		useTwoColors = secondaryColorInput !== mainColorInput;

		savingErrs = [];
	}

	function handleBeforeClose() {
		album = undefined;
		savingErrs = [];
	}

	export function open(a: VokiAlbumPreviewData) {
		album = a;
		initFromAlbum(a);
		dialog.open();
	}

	async function save() {
		savingErrs = [];

		if (!album) {
			savingErrs = [{ message: 'No album selected' } as Err];
			return;
		}

		const mainColorVal = ColorUtils.normalizeHex6(mainColorInput);
		const secondaryColorVal = useTwoColors
			? ColorUtils.normalizeHex6(secondaryColorInput)
			: mainColorVal;

		if (StringUtils.isNullOrWhiteSpace(name)) {
			savingErrs.push({ message: 'Name is required' });
		}
		if (StringUtils.isNullOrWhiteSpace(mainColorVal)) {
			savingErrs.push({
				message: 'Main color is invalid. Choose it using the color picker'
			});
		}
		if (StringUtils.isNullOrWhiteSpace(secondaryColorVal)) {
			savingErrs.push({
				message: 'Secondary color is invalid. Choose it using the color picker'
			});
		}

		if (savingErrs.length > 0) {
			return;
		}

		const response = await ApiAlbums.fetchJsonResponse<VokiAlbumPreviewData>(
			`/albums/${album.id}/update`,
			RJO.PATCH({
				name,
				icon,
				mainColor: mainColorVal,
				secondaryColor: secondaryColorVal
			})
		);

		if (response.isSuccess) {
			updateParent(response.data);
			dialog.close();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="edit-album-dialog"
	onBeforeClose={handleBeforeClose}
>
	{#if album}
		<div
			class="edit-album-container"
			onkeydown={(e) => {
				if (e.key === 'Enter') {
					e.preventDefault();
					save();
				}
			}}
		>
			<div class="header">
				<h2>Edit album</h2>
				<p class="subtitle">Change album name, icon or colors</p>
			</div>

			<label class="field">
				<span class="label">Name</span>
				<input
					class="name-input"
					type="text"
					placeholder="My great album"
					bind:value={name}
					maxlength="1000"
					autocomplete="off"
				/>
			</label>

			<div class="field">
				<span class="label">Icon</span>
				<AlbumIconPicker
					icons={IconUtils.AllAlbumIcons}
					bind:value={icon}
					mainColor={mainColorInput}
					secondaryColor={secondaryColorWithIfTwoCheck}
				/>
			</div>

			<div class="colors-row">
				<AlbumColorInput label={mainColorInputLabel} bind:value={mainColorInput} />
				{#if useTwoColors}
					<AlbumColorInput label="Secondary color" bind:value={secondaryColorInput} />
				{/if}
			</div>
			<label class="use-two">
				Use two color
				<DefaultCheckBox bind:checked={useTwoColors} />
			</label>

			<section class="preview">
				<span class="label">Preview</span>
				<div
					class="preview-card"
					style={`--icon-color-1:${mainColorInput}; --icon-color-2:${secondaryColorWithIfTwoCheck};`}
					aria-label="Album preview"
				>
					<svg class="icon-badge" viewBox="0 0 24 24" aria-hidden="true">
						<use href={`#${icon}`} />
					</svg>
					<label class="name">{name || 'Untitled album'}</label>
				</div>
			</section>

			<DefaultErrBlock errList={savingErrs} class="saving-err-block" />
			<PrimaryButton onclick={save} type="button">Save changes</PrimaryButton>
		</div>
	{:else}
		<p class="no-album-selected">No album selected</p>
	{/if}
</DialogWithCloseButton>

<style>
	.edit-album-container {
		display: flex;
		flex-direction: column;
	}

	.header {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		margin-bottom: 1rem;
	}

	.header h2 {
		margin-bottom: 0;
		color: var(--text);
		font-size: 1.75rem;
	}

	.subtitle {
		margin-top: 0.25rem;
		color: var(--muted-foreground);
		font-size: 1rem;
	}

	.field {
		display: grid;
		gap: 0.125rem;
	}

	.label {
		margin-left: 1rem;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
	}

	.name-input {
		padding: 0.5rem 0.75rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.5rem;
		background-color: var(--back);
		font-size: 1.25rem;
		font-weight: 450;
		outline: none;
	}

	.name-input:hover:not(:focus) {
		border-color: var(--secondary-foreground);
	}

	.name-input:focus {
		border-color: var(--primary);
	}

	.name-input::placeholder {
		color: var(--secondary-foreground);
		font-weight: 320;
	}

	.colors-row {
		display: flex;
		justify-content: center;
		align-items: flex-start;
		gap: 2rem;
		margin-top: 1rem;
	}

	.use-two {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		margin: 0.5rem 0 1rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		cursor: pointer;
		align-self: center;
		user-select: none;
	}

	.preview {
		display: grid;
		gap: 0.25rem;
		width: fit-content;
		justify-self: center;
	}

	.preview-card {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		width: 32rem;
		max-width: 32rem;
		padding: 0.5rem 0.75rem;
		border-radius: 0.875rem;
		color: var(--preview-contrast);
		box-shadow: var(--shadow-xs);
		grid-template-columns: auto 1fr;
		overflow: hidden;
	}

	.icon-badge {
		width: 2rem;
		aspect-ratio: 1/1;
		fill: currentcolor;
		stroke-width: 1.8;
	}

	.name {
		font-size: 1.125rem;
		font-weight: 500;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	.edit-album-container > :global(.saving-err-block) {
		min-height: 3rem;
		margin: 0.5rem 0;
	}

	.edit-album-container > :global(.primary-btn) {
		align-self: center;
		width: 20rem;
		letter-spacing: 0.25px;
	}

	.no-album-selected {
		padding: 1.5rem;
		color: var(--muted-foreground);
		font-size: 1rem;
		text-align: center;
	}
</style>
