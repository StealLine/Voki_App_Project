import type { VokiItemViewOkStateProps, VokiItemViewErrStateProps } from "$lib/components/voki_item/_c_voki_item/voki-item";
import { ApiVokiCreationCore } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { StringUtils } from "$lib/ts/utils/string-utils";
import { toast } from "svelte-sonner";
import { MyDraftVokisCacheStore } from "./my-draft-vokis-cache-store.svelte";

type DraftVokiIdsState =
    | { state: 'loading' }
    | { state: 'errs'; errs: Err[] }
    | { state: 'loaded'; vokiIds: string[] };

export class MyDraftVokisPageState {
    draftVokiIds: DraftVokiIdsState = $state({ state: 'loading' });

    constructor() {
        this.loadDraftVokis();
    }

    async forceRefetch() {
        MyDraftVokisCacheStore.Clear();
        await this.loadDraftVokis();
    }

    async loadDraftVokis() {
        this.draftVokiIds = { state: 'loading' };

        const response = await ApiVokiCreationCore.fetchJsonResponse<{ vokiIds: string[] }>(
            '/list-user-voki-ids',
            { method: 'GET' }
        );

        if (response.isSuccess) {
            this.draftVokiIds = { state: 'loaded', vokiIds: response.data.vokiIds };
        } else {
            this.draftVokiIds = { state: 'errs', errs: response.errs };
        }
    }

    getVokiViewItemState(
        vokiId: string
    ):
        | { name: 'ok'; data: VokiItemViewOkStateProps }
        | { name: 'loading' }
        | { name: 'errs'; data: VokiItemViewErrStateProps } {
        const voki = MyDraftVokisCacheStore.Get(vokiId);

        if (voki.state === 'loading') {
            return { name: 'loading' };
        }
        if (voki.state === 'errs') {
            return { name: 'errs', data: { vokiId, errs: voki.errs } };
        }

        return {
            name: 'ok',
            data: {
                vokiId,
                type: voki.data.type,
                voki: {
                    name: voki.data.name,
                    cover: voki.data.cover,
                    primaryAuthorId: voki.data.primaryAuthorId,
                    coAuthorIds: voki.data.coAuthorIds
                },
                onMoreBtnClick: () => toast.error("Voki more button isn't implemented yet"),
                link: `/voki-creation/${StringUtils.pascalToKebab(voki.data.type)}/${vokiId}`
            }
        };
    }
}