import { setContext, getContext } from "svelte";

const openFunctionKey = Symbol("open-create-new-album-dialog-function");
type openCreateNewAlbumDialogFunction = (onAfterNewAlbumCreated: (newAlbumId: string) => void) => void;

export function registerCreateNewAlbumOpenFunction(openDialog: openCreateNewAlbumDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getCreateNewAlbumOpenFunction() {
    return getContext<openCreateNewAlbumDialogFunction>(openFunctionKey);
}

