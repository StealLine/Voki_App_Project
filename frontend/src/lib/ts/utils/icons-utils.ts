import type { AuthorProfileLinkType } from "../users";

export namespace IconUtils {
    export const AllAlbumIcons = [
        'albums-bookmark-1-icon',
        'albums-bookmark-2-icon',
        'albums-clock-1-icon',
        'albums-clock-2-icon',
        'albums-star-1-icon',
        'albums-star-2-icon'
    ];

    const profileLinkIconIdByTypeMap: Record<AuthorProfileLinkType, string> = {
        'Other': 'link-other-icon',
        'Website': 'link-website-icon',
        'X': 'link-x-icon',
        'Instagram': 'link-instagram-icon',
        'Youtube': 'link-youtube-icon',
        'Tiktok': 'link-tiktok-icon',
        'Telegram': 'link-telegram-icon',
        'Mangalib': 'link-mangalib-icon',
        'Reddit': 'link-reddit-icon'
    };
    export function getProfileLinkIconIdByType(type: AuthorProfileLinkType): string {
        const iconId = profileLinkIconIdByTypeMap[type];
        if (!iconId) {
            throw new Error(`Unknown author profile link type: ${type}`);
        }
        return `#${iconId}`;
    }
}