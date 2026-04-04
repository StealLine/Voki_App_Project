import type { GeneralVokiQuestionContentType } from "$lib/ts/voki";

export type QuestionBriefInfo = {
    id: string;
    text: string;
    images: string[];
    contentType: GeneralVokiQuestionContentType;
    orderInVoki: number;
    shuffleAnswers: boolean;
    isMultipleChoice: boolean;
}
export type GeneralVokiTakingProcessSettings = {
    forceSequentialAnswering: boolean;
    shuffleQuestions: boolean;
}