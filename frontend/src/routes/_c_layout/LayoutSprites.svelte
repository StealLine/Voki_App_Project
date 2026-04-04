<script lang="ts">
	import vokiTypesIconsSprite from '$lib/icons/voki-type-icons.svg?raw';
	import commonIconsSprite from '$lib/icons/common-icons.svg?raw';
	import caretIconsSprite from '$lib/icons/caret-icons.svg?raw';
	import languagesIconsSprite from '$lib/icons/languages-icons.svg?raw';
	import albumIconsSprite from '$lib/icons/album-icons.svg?raw';
	import imageIconsSprite from '$lib/icons/image-icons.svg?raw';
	import contextMenuIconsSprite from '$lib/icons/context-menu-icons.svg?raw';
	import userProfileLinksIconsSprite from '$lib/icons/user-profile-links-icons.svg?raw';
</script>

<div class="sprites">
	{@html vokiTypesIconsSprite}
	{@html commonIconsSprite}
	{@html languagesIconsSprite}
	{@html caretIconsSprite}
	{@html albumIconsSprite}
	{@html imageIconsSprite}
	{@html contextMenuIconsSprite}
	{@html userProfileLinksIconsSprite}
</div>

<style>
	.sprites {
		position: absolute;
		width: 0;
		height: 0;
		overflow: hidden;
		pointer-events: none;
	}
</style>
