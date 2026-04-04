type Params = {
    goToNextQuestion: () => boolean;
    goToPreviousQuestion: () => boolean;
    focusFirstAnswerCardInContainer: () => void;
    enabled?: boolean;
};

export function createQuestionsKeyHandler(params: Params) {
    let options = {
        enabled: true,
        ...params
    };

    return function onKeyDown(e: KeyboardEvent) {
        if (!options.enabled) return;
        if (!e.shiftKey) return;

        switch (e.key) {
            case "ArrowRight": {
                e.preventDefault();
                const changed = options.goToNextQuestion();
                if (changed) {
                    options.focusFirstAnswerCardInContainer();
                }
                break;
            }
            case "ArrowLeft": {
                e.preventDefault();
                const changed = options.goToPreviousQuestion();
                if (changed) {
                    options.focusFirstAnswerCardInContainer();
                }
                break;
            }
        }
    };
}
