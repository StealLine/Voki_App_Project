
export type QuestionFullInfo = {
    id: string;
    text: string;
    imageSet: GeneralVokiCreationQuestionImageSet;
    content: GeneralVokiCreationQuestionContent;
    shuffleAnswers: boolean;
    minAnswersCount: number;
    maxAnswersCount: number;
    maxAnswersForQuestionCount: number;
    maxResultsForAnswerCount: number;
}

export type QuestionAnswersSettings = {
    shuffleAnswers: boolean;
    minAnswersCount: number;
    maxAnswersCount: number;
}

export type GeneralVokiCreationQuestionImageSet = {
    width: number;
    height: number;
    keys: string[]
}

export type GeneralVokiCreationQuestionContent =
    | { '$type': 'TextOnly', answers: AnswerDataTextOnly[] }
    | { '$type': 'ImageOnly', answers: AnswerDataImageOnly[] }
    | { '$type': 'ImageAndText', answers: AnswerDataImageAndText[] }
    | { '$type': 'ColorOnly', answers: AnswerDataColorOnly[] }
    | { '$type': 'ColorAndText', answers: AnswerDataColorAndText[] }
    | { '$type': 'AudioOnly', answers: AnswerDataAudioOnly[] }
    | { '$type': 'AudioAndText', answers: AnswerDataAudioAndText[] };

export type BaseGeneralVokiAnswerData = {
    relatedResultIds: string[]
    order: number
}
export type AnswerDataTextOnly = { text: string; }
    & BaseGeneralVokiAnswerData
export type AnswerDataImageOnly = { image: string }
    & BaseGeneralVokiAnswerData
export type AnswerDataImageAndText = { image: string; text: string }
    & BaseGeneralVokiAnswerData
export type AnswerDataColorOnly = { color: string }
    & BaseGeneralVokiAnswerData
export type AnswerDataColorAndText = { color: string; text: string }
    & BaseGeneralVokiAnswerData
export type AnswerDataAudioOnly = { audio: string }
    & BaseGeneralVokiAnswerData
export type AnswerDataAudioAndText = { audio: string; text: string }
    & BaseGeneralVokiAnswerData;