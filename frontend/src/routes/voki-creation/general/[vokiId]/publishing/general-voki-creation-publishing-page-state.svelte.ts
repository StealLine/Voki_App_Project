import type { IVokiCreationPageState } from "../../../voki-creation-page-context";

export class GeneralVokiCreationPublishingPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
}
