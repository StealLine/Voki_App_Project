import type { GeneralVokiTakingProcessSettings } from "./types";
import type { IVokiCreationPageState } from "../../../voki-creation-page-context";

export class GeneralVokiCreationAllQuestionsPageState implements IVokiCreationPageState {
    readonly maxQuestionsCount: number;
    public savedSettings: GeneralVokiTakingProcessSettings = $state()!;
    public isEditingVokiTakingProcessSettings = $state(false);

    constructor(initialSettings: GeneralVokiTakingProcessSettings, maxQuestionsCount: number) {
        this.savedSettings = initialSettings;
        this.maxQuestionsCount = maxQuestionsCount;
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.isEditingVokiTakingProcessSettings;
    }
}
