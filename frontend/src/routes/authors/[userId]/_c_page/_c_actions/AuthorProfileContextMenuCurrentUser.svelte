<script lang="ts">
	import { toast } from 'svelte-sonner';
	import AuthorProfileDropdown, { type DropdownItem } from './AuthorProfileDropdown.svelte';

	interface Props {
		profileId: string;
	}

	let { profileId }: Props = $props();
	let dropdown = $state<AuthorProfileDropdown>();

	export function open() {
		dropdown?.open();
	}

	function copyProfileLink() {
		const link = `${location.origin}/authors/${profileId}`;
		navigator.clipboard.writeText(link).then(
			() => toast.success('Profile link copied to clipboard'),
			() => toast.error('Could not copy profile link')
		);
	}

	let menuItems: DropdownItem[] = [
		{
			text: 'Edit profile',
			iconHref: '#context-menu-edit-icon',
			action: { type: 'link', href: '/settings' }
		},
		{
			text: 'Share profile',
			iconHref: '#context-menu-share-icon',
			action: { type: 'btn', onclick: copyProfileLink }
		}
	];
</script>

<AuthorProfileDropdown bind:this={dropdown} items={menuItems} />
