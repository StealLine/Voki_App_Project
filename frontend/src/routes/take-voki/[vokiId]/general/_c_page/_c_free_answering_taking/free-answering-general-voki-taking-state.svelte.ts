import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import type { GeneralVokiTakingQuestionData, GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from "../../types";

export class FreeAnsweringGeneralVokiTakingState {
    readonly vokiId: string;
    readonly #sessionId: string;
    readonly #serverSessionStartTime: Date;
    readonly #clientSessionStartTime: Date;
    readonly #questions: GeneralVokiTakingQuestionData[] = $state()!;
    readonly totalQuestionsCount: number;
    readonly #clearVokiSeenUpdateTimer: () => void;

    readonly chosenAnswers = $state<Record<string, Record<string, boolean>>>({});

    currentQuestionOrder = $state<number>()!;
    currentQuestion: GeneralVokiTakingQuestionData | undefined = $derived<GeneralVokiTakingQuestionData | undefined>(
        this.#questions.find(
            q => q.orderInVokiTaking === this.currentQuestionOrder
        )
    );

    constructor(data: GeneralVokiTakingData, saveData: PosssibleGeneralVokiTakingDataSaveData, clearVokiSeenUpdateTimer: () => void) {
        if (data.isWithForceSequentialAnswering) {
            throw new Error("Cannot create voki taking state, because voki is with sequential answering");

        }

        this.vokiId = data.vokiId;
        this.#sessionId = data.sessionId;
        this.#serverSessionStartTime = data.startedAt;
        this.#clientSessionStartTime = new Date();
        this.#questions = data.questions;
        this.totalQuestionsCount = data.totalQuestionsCount;
        if (this.#questions.length !== this.totalQuestionsCount) {
            throw new Error(
                `GeneralVokiTakingState inconsistency: questions length (${this.#questions.length}) != totalQuestionsCount (${this.totalQuestionsCount})`
            );
        }

        // Initialize answers for all questions
        this.#questions.forEach(q => {
            this.chosenAnswers[q.id] = Object.fromEntries(
                q.content.answers.map(a => [a.id, false])
            ) as Record<string, boolean>;
        });

        if (saveData.anySave) {
            // Restore chosen answers from save
            for (const [questionId, answerIds] of Object.entries(saveData.savedChosenAnswers)) {
                if (this.chosenAnswers[questionId]) {
                    for (const answerId in this.chosenAnswers[questionId]) {
                        this.chosenAnswers[questionId][answerId] = false;
                    }
                    for (const answerId of answerIds) {
                        if (this.chosenAnswers[questionId][answerId] !== undefined) {
                            this.chosenAnswers[questionId][answerId] = true;
                        }
                    }
                }
            }

            const questionToSetAsCurrent = this.#questions.find(q => q.id === saveData.currentQuestionId);
            if (questionToSetAsCurrent) {
                this.currentQuestionOrder = questionToSetAsCurrent.orderInVokiTaking;
            }
            else {
                this.currentQuestionOrder = Math.min(...this.#questions.map((q) => q.orderInVokiTaking));
            }


        }
        else {
            this.currentQuestionOrder = Math.min(...this.#questions.map((q) => q.orderInVokiTaking));
        }

        this.#lastSavedState = Object.fromEntries(saveData.anySave
            ? Object.entries(this.chosenAnswers).map(([qId, answers]) => [qId, { ...answers }])
            : this.#questions.map(q => [q.id, Object.fromEntries(q.content.answers.map(a =>
                [a.id, false]
            ))])
        );

        this.#clearVokiSeenUpdateTimer = clearVokiSeenUpdateTimer;
    }

    goToPreviousQuestion(): boolean {
        if (this.currentQuestionOrder > 1) {
            this.currentQuestionOrder -= 1;
            return true;
        }
        return false;
    }

    goToNextQuestion(): boolean {
        if (this.currentQuestionOrder < this.totalQuestionsCount) {
            this.saveCurrentStateInTheBackground();
            this.currentQuestionOrder += 1;
            return true;
        }
        return false;
    }

    jumpToSpecificQuestion(orderInVokiTaking: number): Err[] {
        if (orderInVokiTaking === this.currentQuestion?.orderInVokiTaking) {
            return [];
        }
        const q = this.#questions.find(q => q.orderInVokiTaking === orderInVokiTaking);
        if (!q) {
            return [{
                message: `Cannot jump to specific question, because question with order ${orderInVokiTaking} does not exist`
            }];
        }
        this.currentQuestionOrder = q.orderInVokiTaking;
        this.saveCurrentStateInTheBackground();
        return [];
    }


    get isCurrentQuestionLast() {
        return this.currentQuestionOrder === this.totalQuestionsCount;
    }

    get isCurrentQuestionFirst() {
        return this.currentQuestionOrder === 1;
    }

    async finishTakingAndReceiveResult(): Promise<
        | { isSuccess: false, errs: ErrMessageWithQuestionOrder[] }
        | ResponseResult<{ receivedResultId: string }>
    > {
        const errs = this.checkErrsBeforeFinish();
        if (errs.length > 0) {
            return { isSuccess: false, errs };
        }
        const response = await ApiVokiTakingGeneral.fetchJsonResponse<{ receivedResultId: string }>(
            `/vokis/${this.vokiId}/free-answering/finish`,
            RJO.POST(this.createVokiTakenData())
        );

        if (response.isSuccess) {
            this.#clearVokiSeenUpdateTimer();
            return response;
        }
        return response;
    }
    createVokiTakenData() {
        return {
            chosenAnswers: this.#questionIdToChosenAnswerIds(),
            serverSessionStartTime: this.#serverSessionStartTime,
            clientSessionStartTime: this.#clientSessionStartTime,
            clientSessionFinishTime: new Date(),
            sessionId: this.#sessionId
        };
    }
    checkErrsBeforeFinish(): ErrMessageWithQuestionOrder[] {
        const errs: ErrMessageWithQuestionOrder[] = [];

        for (const q of this.#questions) {
            const chosenMap = this.chosenAnswers[q.id] ?? {};
            const chosenCount = Object.values(chosenMap).filter(x => x).length;

            const min = q.minAnswersCount ?? 0;
            const max = q.maxAnswersCount ?? Number.POSITIVE_INFINITY;

            const preview =
                !q.text || q.text.length < 40
                    ? (q.text ?? '')
                    : `${q.text.slice(0, 30)}…`;

            const title = `#${q.orderInVokiTaking} '${preview}'`;

            if (chosenCount === 0 && min > 0) {
                errs.push({
                    message: `Question ${title} is not answered. Please choose at least ${min} ${min === 1 ? 'answer' : 'answers'}`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }

            if (chosenCount < min) {
                errs.push({
                    message: `Question ${title}: you chose ${chosenCount}, but at least ${min} ${min === 1 ? 'answer is' : 'answers are'} required`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }

            if (chosenCount > max) {
                errs.push({
                    message: `Question ${title}: you chose ${chosenCount}, but at most ${max} ${max === 1 ? 'answer is' : 'answers are'} allowed`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }
        }

        return errs;
    }
    #lastSavedState: Record<string, Record<string, boolean>>; //questionId:(answerId:isChosen)
    async saveCurrentStateInTheBackground() {
        let anySavedNeeded = false;
        for (const [qId, answers] of Object.entries(this.chosenAnswers)) {
            const savedAnswers = this.#lastSavedState[qId];
            if (!savedAnswers) {
                anySavedNeeded = true;
                break;
            }
            for (const [answerId, isChosen] of Object.entries(answers)) {
                if (savedAnswers[answerId] !== isChosen) {
                    anySavedNeeded = true;
                    break;
                }
            }
        }
        if (!anySavedNeeded) {
            return;
        }

        const response = await ApiVokiTakingGeneral.fetchJsonResponse<
            { savedChosenAnswers: Record<string, string[]> }
        >(
            `/vokis/${this.vokiId}/free-answering/save-current-state`, RJO.POST({
                sessionId: this.#sessionId,
                questionIdToChosenAnswers: this.#questionIdToChosenAnswerIds()
            })
        );

        if (response.isSuccess) {
            this.#lastSavedState = Object.fromEntries(
                Object.entries(response.data.savedChosenAnswers)
                    .map(([qId, answerIds]) => [
                        qId,
                        Object.fromEntries(answerIds.map(answerId => [answerId, true]))
                    ])
            );
        }
    }

    #questionIdToChosenAnswerIds() {
        return Object.fromEntries(
            Object.entries(this.chosenAnswers).map(([qId, answers]) => [
                qId,
                Object.entries(answers)
                    .filter(([, isSelected]) => isSelected)
                    .map(([answerId]) => answerId)
            ])
        );
    }
}
type ErrMessageWithQuestionOrder = { message: string, questionOrder: number };