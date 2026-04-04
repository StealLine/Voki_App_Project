import type { Err } from "$lib/ts/err";
import type { AllowCoAuthorInvitesSettingValue } from "$lib/ts/users";

export type VokiCreationAuthorsInfo = {
    primaryAuthorId: string;
    coAuthorIds: string[];
    invitedForCoAuthorUserIds: string[];
    vokiCreationDate: Date;
    maxVokiCoAuthors: number;
    expectedManagers: VokiExpectedManagersSetting;
}
export type VokiExpectedManagersSetting = {
    makeAllCoAuthorsManagers: boolean;
    userIdsToBecomeManagers: string[];
};
export type UserPreviewWithInvitesSettings = {
    id: string;
    uniqueName: string;
    displayName: string;
    profilePic: string;
    allowCoAuthorInvites: AllowCoAuthorInvitesSettingValue;
}

export type UserInviteState =
    | { state: 'PrimaryAuthor' }
    | { state: 'CoAuthor' }
    | { state: 'AlreadyInvited' }
    | {
        state: 'CandidateToInvite';
        isUserInListToInvite: boolean;
        addToListToInvite: () => void;
        removeFromListToInvite: () => void;
    };

export type UsersRecommendedToInvite =
    | { state: 'ok'; users: UserPreviewWithInvitesSettings[] }
    | { state: 'loading' }
    | { state: 'errs'; errs: Err[] }