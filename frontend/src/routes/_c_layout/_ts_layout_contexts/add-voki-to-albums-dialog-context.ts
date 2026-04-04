import { getContext, setContext } from "svelte";

const openFunctionKey = Symbol("open-add-voki-to-album-dialog-function");
type openAddVokiToAlbumDialogFunction = (vokiId: string) => void;

export function registerAddVokiToAlbumsOpenFunction(openDialog: openAddVokiToAlbumDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getAddVokiToAlbumsOpenFunction() {
    return getContext<openAddVokiToAlbumDialogFunction>(openFunctionKey);
}

