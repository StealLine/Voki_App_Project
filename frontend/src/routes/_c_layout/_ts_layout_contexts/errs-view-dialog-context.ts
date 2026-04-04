import type { Err } from "$lib/ts/err";
import { setContext, getContext } from "svelte";

const openFunctionKey = Symbol("open-errs-view-dialog-function");
type openErrsViewDialogFunction = (errs: Err[]) => void;

export function registerErrsViewDialogOpenFunction(openDialog: openErrsViewDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getErrsViewDialogOpenFunction() {
    return getContext<openErrsViewDialogFunction>(openFunctionKey);
}

