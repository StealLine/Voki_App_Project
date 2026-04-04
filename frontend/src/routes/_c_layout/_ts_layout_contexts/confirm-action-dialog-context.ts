import type { Err } from "$lib/ts/err";
import { getContext, setContext, type Snippet } from "svelte";

export type ConfirmActionDialogButtons = {
	confirmBtnText: string;
	confirmBtnOnclick: () => Promise<Err[]> | void;
	cancelBtnText: string;
	cancelBtnOnclick: () => void;
};
export type ConfirmActionDialogMainContent = {
	subheading: string | undefined;
	mainText: string;
};
export type ConfirmActionDialogContent = {
	mainContent: ConfirmActionDialogMainContent | Snippet,
	dialogButtons: ConfirmActionDialogButtons | Snippet
};

const openFunctionKey = Symbol("open-confirm-action-dialog-function");
type openConfirmActionDialogFunction = { open: (val: ConfirmActionDialogContent) => void, close: () => void };

export function registerConfirmActionDialogOpenFunction(openDialog: openConfirmActionDialogFunction) {
	setContext(openFunctionKey, openDialog);
}

export function getConfirmActionDialogOpenFunction() {
	return getContext<openConfirmActionDialogFunction>(openFunctionKey);
}