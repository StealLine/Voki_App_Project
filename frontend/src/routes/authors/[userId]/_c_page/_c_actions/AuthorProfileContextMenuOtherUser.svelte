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
			text: 'Send message',
			iconHref: '#context-menu-message-icon',
			action: { type: 'link', href: `/messages/${profileId}` }
		},
		{
			text: 'Invite as Co-Author',
			iconHref: '#context-menu-invite-coauthor-icon',
			action: { type: 'btn', onclick: () => toast.info('Invite feature coming soon!') }
		},
		{
			text: 'Copy profile link',
			iconHref: '#context-menu-copy-link-icon',
			action: { type: 'btn', onclick: copyProfileLink }
		},
		{
			text: 'Report user',
			iconHref: '#context-menu-report-icon',
			action: { type: 'btn', onclick: () => toast.info('Report feature coming soon!') },
			variant: 'red'
		}
	];
</script>

<AuthorProfileDropdown bind:this={dropdown} items={menuItems} />
