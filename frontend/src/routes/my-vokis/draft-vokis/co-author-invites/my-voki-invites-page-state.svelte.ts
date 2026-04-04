import { ApiVokiCreationCore } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { VokiType } from "$lib/ts/voki-type";

type InvitesState =
    | { state: 'loading' }
    | { state: 'errs'; errs: Err[] }
    | { state: 'loaded'; invites: InviteForVokiCoAuthorData[] };

export type InviteForVokiCoAuthorData = {
    vokiId: string,
    vokiName: string,
    vokiCover: string,
    vokiType: VokiType,
    primaryAuthorId: string,
    coAuthorIds: string[],
    invitedForCoAuthorUserIds: string[],
    creationDate: Date
}

export class MyVokiInvitesPageState {
    loadingState: InvitesState = $state({ state: 'loading' });

    constructor() {
        this.loadInvites();
    }

    async forceRefetch() {
        await this.loadInvites();
    }

    async loadInvites() {
        this.loadingState = { state: 'loading' };

        const response = await ApiVokiCreationCore.fetchJsonResponse<{ invites: InviteForVokiCoAuthorData[] }>(
            '/list-invites',
            { method: 'GET' }
        );

        if (response.isSuccess) {
            this.loadingState = { state: 'loaded', invites: response.data.invites };
        } else {
            this.loadingState = { state: 'errs', errs: response.errs };
        }
    }
    deleteInvite(vokiId: string) {
        if (this.loadingState.state !== 'loaded') {
            this.loadInvites();
            return;
        }
        this.loadingState.invites = this.loadingState.invites.filter(inv => inv.vokiId !== vokiId);
    }


}