import { setContext, getContext } from "svelte";

const openFunctionKey = Symbol("open-voki-flags-info-dialog-function");
type openVokiFlagsInfoDialogFunction = () => void;

export function registerVokiFlagsInfoDialogOpenFunction(openDialog: openVokiFlagsInfoDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getVokiFlagsInfoDialogOpenFunction() {
    return getContext<openVokiFlagsInfoDialogFunction>(openFunctionKey);
}

