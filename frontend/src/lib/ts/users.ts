export type UserProfilePreview = {
    id: string;
    uniqueName: string;
    displayName: string;
    profilePic: string;
}
export type UserProfilePicData = { key: string; shape: ProfilePicShape };
export type ProfilePicShape = 'Circle' | 'Squircle' | 'RoundedSquare20' | 'RoundedSquare30' | 'Seal';

export const AllAuthorProfileLinkTypes = [
    'Other',
    'Website',
    'X',
    'Instagram',
    'Youtube',
    'Tiktok',
    'Telegram',
    'Mangalib',
    'Reddit'
];
export type AuthorProfileLinkType = typeof AllAuthorProfileLinkTypes[number];

export type AllowCoAuthorInvitesSettingValue = 'Everyone' | 'NoOne';

export type LanguageFlagDisplay = 'Flag' | 'ThreeLetterCode'



export type AuthorBanner = BannerDefault | BannerFillColor;

export type BannerDefault = {
    $type: 'Default';
};

export type BannerFillColor = {
    $type: 'FillColor';
    color: string;
};