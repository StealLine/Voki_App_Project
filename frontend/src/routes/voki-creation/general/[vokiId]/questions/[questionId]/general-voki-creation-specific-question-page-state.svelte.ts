import type { GeneralVokiCreationQuestionContent, GeneralVokiCreationQuestionImageSet } from "./types";
import type { IVokiCreationPageState } from "../../../../voki-creation-page-context";
import type { QuestionAnswersSettings } from "./types";
import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { Err } from "$lib/ts/err";

export class GeneralVokiCreationSpecificQuestionPageState implements IVokiCreationPageState {
    public resultsIdToName: QuestionPageResultsState = $state()!;
    public readonly vokiId: string;


    public isEditingQuestionText = $state(false);
    public isEditingQuestionAnswerSettings = $state(false);
    public isEditingQuestionTypeSpecificContent = $state(false);

    public savedText: string = $state()!;
    public savedImageSet: GeneralVokiCreationQuestionImageSet = $state()!;
    public savedAnswerSettings: QuestionAnswersSettings = $state()!;
    public savedTypeSpecificContent: GeneralVokiCreationQuestionContent = $state()!;

    constructor(
        text: string,
        imageSet: GeneralVokiCreationQuestionImageSet,
        answerSettings: QuestionAnswersSettings,
        typeSpecificContent: GeneralVokiCreationQuestionContent,
        vokiId: string,
        resultsIdToName: Record<string, string>,
    ) {
        this.savedText = text;
        this.savedImageSet = imageSet;
        this.savedAnswerSettings = answerSettings;
        this.savedTypeSpecificContent = typeSpecificContent;
        this.vokiId = vokiId;
        this.resultsIdToName = { state: 'ok', resultsIdToName };
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.isEditingQuestionText
            || this.isEditingQuestionAnswerSettings
            || this.isEditingQuestionTypeSpecificContent;
    }
    async fetchResultNames() {
        this.resultsIdToName = { state: 'loading' };
        const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
            resultsIdsToName: Record<string, string>;
        }>(`/vokis/${this.vokiId}/results/ids-names`, { method: 'GET' });
        if (response.isSuccess) {
            this.resultsIdToName = { state: 'ok', resultsIdToName: response.data.resultsIdsToName };
        } else {
            this.resultsIdToName = { state: 'error', errs: response.errs };
        }
    }
}

export type QuestionPageResultsState =
    | { state: 'ok', resultsIdToName: Record<string, string> }
    | { state: 'loading' }
    | { state: 'error', errs: Err[] };