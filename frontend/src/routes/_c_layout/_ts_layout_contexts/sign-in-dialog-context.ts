import { getContext, setContext } from 'svelte';

export type SignInDialogState = 'login' | 'signup' | 'confirmation-sent';


const openFunctionKey = Symbol("open-sign-in-dialog-function");
type openSignInDialogFunction = (val: SignInDialogState | null) => void;

export function registerSignInDialogOpenFunction(openDialog: openSignInDialogFunction) {
	setContext(openFunctionKey, openDialog);
}

export function getSignInDialogOpenFunction() {
	return getContext<openSignInDialogFunction>(openFunctionKey);
}


