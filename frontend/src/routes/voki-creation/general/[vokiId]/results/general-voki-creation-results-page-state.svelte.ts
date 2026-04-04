import type { IVokiCreationPageState } from "../../../voki-creation-page-context";
import type { ResultOverViewData } from "./_c_results_page/types";

export class ResultItemState {
    public isEditing = $state(false);
    public data: ResultOverViewData = $state()!;
    constructor(data: ResultOverViewData) {
        this.data = data;
    }
}

export class GeneralVokiCreationResultsPageState implements IVokiCreationPageState {
    public results: ResultItemState[] = $state([]);
    public readonly maxResultsCount: number;
    constructor(initialResults: ResultOverViewData[], maxResultsCount: number) {
        this.results = initialResults.map(r => new ResultItemState(r));
        this.maxResultsCount = maxResultsCount;
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.results.some(r => r.isEditing);
    }

    addResult(result: ResultOverViewData) {
        this.results.push(new ResultItemState(result));
    }

    updateResult(result: ResultOverViewData) {
        const item = this.results.find(r => r.data.id === result.id);
        if (item) {
            item.data = result;
        }
    }

    deleteResult(resultId: string) {
        this.results = this.results.filter(r => r.data.id !== resultId);
    }
}
