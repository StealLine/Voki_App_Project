import { tick } from 'svelte';

export type AnswerRef = { id: string };

type Params = {
    answers: AnswerRef[];
    chooseAnswer: (id: string) => void;
    selector?: string;
    focusOnMount?: boolean;
    useSpacebarToChoose?: boolean;
};

export function answersKeyboardNav(node: HTMLElement, init: Params) {
    let params = {
        selector: '.answer',
        focusOnMount: true,
        useSpacebarToChoose: true,
        ...init
    };

    let items: HTMLElement[] = [];
    const refresh = () => {
        items = Array.from(node.querySelectorAll<HTMLElement>(params.selector!));
    };

    function focusCard(i: number) {
        if (!items.length) { return };
        if (i < 0) { i = items.length - 1; }
        if (i >= items.length) { i = 0; }
        items[i]?.focus();
    }


    const mo = new MutationObserver(async () => {
        refresh();
        await tick();
        focusCard(0);
    });
    mo.observe(node, { childList: true, subtree: true });
    refresh();

    function onKeyDown(e: KeyboardEvent) {
        const active = document.activeElement as HTMLElement | null;
        const idx = active ? items.indexOf(active) : -1;

        if (e.shiftKey || e.altKey || e.ctrlKey || e.metaKey) {
            return;
        }
        switch (e.key) {
            case 'Enter': {
                if (idx >= 0) {
                    e.preventDefault();
                    params.chooseAnswer(params.answers[idx].id);
                }
                break;
            }
            case ' ': {
                if (params.useSpacebarToChoose && idx >= 0) {
                    e.preventDefault();
                    params.chooseAnswer(params.answers[idx].id);
                }
                break;
            }
            case 'ArrowRight':
            case 'ArrowDown':
                e.preventDefault();
                focusCard((idx >= 0 ? idx : -1) + 1);
                break;
            case 'ArrowLeft':
            case 'ArrowUp':
                e.preventDefault();
                focusCard((idx >= 0 ? idx : items.length) - 1);
                break;

        }
    }

    node.addEventListener('keydown', onKeyDown);

    if (params.focusOnMount) {
        tick().then(() => focusCard(0));
    }
    return {
        update(next: Params) {
            params = {
                selector: '.answer',
                focusOnMount: true,
                useSpacebarToChoose: true,
                ...next
            };
            refresh();
        },
        destroy() {
            mo.disconnect();
            node.removeEventListener('keydown', onKeyDown);
        },
        focusFirstAnswer() {
            focusCard(0);
        }
    };
}
