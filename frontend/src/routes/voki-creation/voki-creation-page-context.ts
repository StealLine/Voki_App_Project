import { type IVokiCreationBackendService } from '$lib/ts/backend-communication/voki-creation-backend-service';
import { getContext, setContext } from 'svelte';

export interface IVokiCreationPageState {
	get hasAnyUnsavedChanges(): boolean;
}

const key = Symbol("voki-creation-page-api");

type ContextType = {
	vokiCreationApi: IVokiCreationBackendService,
	headerVokiName: {
		value: string | undefined,
		invalidate: () => void
	},
	currentPageState: IVokiCreationPageState | undefined
}
export function initVokiCreationPageContext(
	apiService: IVokiCreationBackendService,
	headerVokiName: {
		value: string | undefined,
		invalidate: () => void
	}
) {

	setContext<ContextType>(key, {
		vokiCreationApi: apiService,
		headerVokiName: headerVokiName,
		currentPageState: undefined
	});
}

export function getVokiCreationPageContext() {
	return getContext<ContextType>(key);
}

export function setVokiCreationCurrentPageState(pageState: IVokiCreationPageState) {
	const context = getContext<ContextType>(key);
	context.currentPageState = pageState;
}
export function setVokiCreationCurrentPageStateAsUnableToLoad() {
	const context = getContext<ContextType>(key);
	context.currentPageState = {
		get hasAnyUnsavedChanges() {
			return false;
		}
	}
}