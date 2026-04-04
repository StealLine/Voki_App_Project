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
			text: 'Sign in to interact',
			iconHref: '#context-menu-signin-icon',
			action: { type: 'link', href: '/signin' }
		},
		{
			text: 'Copy profile link',
			iconHref: '#context-menu-copy-link-icon',
			action: { type: 'btn', onclick: copyProfileLink }
		}
	];
</script>

<AuthorProfileDropdown bind:this={dropdown} items={menuItems} />
