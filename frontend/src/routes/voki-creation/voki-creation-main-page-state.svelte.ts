import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { IVokiCreationPageState } from "./voki-creation-page-context";

export abstract class VokiCreationMainPageState implements IVokiCreationPageState {
    public savedName: string = $state()!;
    public savedCover: string = $state()!;
    public savedTags: Set<string> = $state()!;
    public savedDetails: VokiDetails = $state()!;
    constructor(
        name: string,
        cover: string,
        tags: string[],
        details: VokiDetails,
    ) {
        this.savedName = name;
        this.savedCover = cover;
        this.savedTags = new Set(tags);
        this.savedDetails = details;
    }
    public isNameEditing = $state<boolean>(false);
    public isDetailsEditing = $state<boolean>(false);

    abstract get hasAnyUnsavedChanges(): boolean;
}
