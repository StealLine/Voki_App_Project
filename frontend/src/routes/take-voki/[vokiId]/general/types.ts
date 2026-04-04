export type BaseVokiTakingSessionData = {
    vokiId: string;
    sessionId: string;
    startedAt: Date;
    totalQuestionsCount: number;
}
export type GeneralVokiTakingData = BaseVokiTakingSessionData & {
    vokiName: string;
    isWithForceSequentialAnswering: boolean;
    questions: GeneralVokiTakingQuestionData[];
}
export type PosssibleGeneralVokiTakingDataSaveData =
    | {
        anySave: true,
        savedChosenAnswers: Record<string, string[]>;
        currentQuestionId: string;
        continuedAt: Date;
    }
    | { anySave: false }
export type GeneralVokiTakingQuestionData = {
    id: string;
    text: string;
    imageKeys: string[];
    imagesAspectRatio: number;
    orderInVokiTaking: number;
    minAnswersCount: number;
    maxAnswersCount: number;
    content: GeneralVokiTakingQuestionContent;
}
export type GeneralVokiTakingQuestionContent =
    | { '$type': 'TextOnly', answers: GeneralVokiTakingAnswerTextOnly[] }
    | { '$type': 'ImageOnly', answers: GeneralVokiTakingAnswerImageOnly[] }
    | { '$type': 'ImageAndText', answers: GeneralVokiTakingAnswerImageAndText[] }
    | { '$type': 'ColorOnly', answers: GeneralVokiTakingAnswerColorOnly[] }
    | { '$type': 'ColorAndText', answers: GeneralVokiTakingAnswerColorAndText[] }
    | { '$type': 'AudioOnly', answers: GeneralVokiTakingAnswerAudioOnly[] }
    | { '$type': 'AudioAndText', answers: GeneralVokiTakingAnswerAudioAndText[] };

export type BaseGeneralVokiTakingAnswerData = {
    id: string;
    orderInQuestionInSession: number;
}
export type GeneralVokiTakingAnswerTextOnly = { text: string; }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerImageOnly = { image: string }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerImageAndText = { image: string; text: string }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerColorOnly = { color: string }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerColorAndText = { color: string; text: string }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerAudioOnly = { audio: string }
    & BaseGeneralVokiTakingAnswerData
export type GeneralVokiTakingAnswerAudioAndText = { audio: string; text: string }
    & BaseGeneralVokiTakingAnswerData;
