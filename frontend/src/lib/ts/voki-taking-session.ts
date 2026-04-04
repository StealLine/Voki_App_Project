export type ExistingUnfinishedSessionForVokiData = {
    vokiId: string;
    sessionId: string;
    startedAt: Date;
    questionsWithSavedAnswersCount: number;
    totalQuestionsCount: number;
};