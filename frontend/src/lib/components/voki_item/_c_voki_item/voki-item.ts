import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import type { PublishedVokiBriefInfo } from "$lib/ts/voki";
import type { VokiType } from "$lib/ts/voki-type";

export type VokiItemViewState =
    | { name: 'ok'; data: VokiItemViewOkStateProps, hide?: VokiItemHidableElements[] }
    | { name: 'loading', hide?: VokiItemHidableElements[] }
    | { name: 'errs'; data: VokiItemViewErrStateProps, hide?: VokiItemHidableElements[] };

export interface VokiItemViewOkStateProps {
    vokiId: string;
    type: VokiType;
    voki: {
        name: string;
        cover: string;
        primaryAuthorId: string;
        coAuthorIds: string[];
    };
    onMoreBtnClick: (mEvent: MouseEvent) => void;
    link: string;
    flags?: {
        language: Language;
        hasMatureContent: boolean;
        authenticatedOnlyTaking: boolean;
    };
}
export interface VokiItemViewErrStateProps {
    vokiId: string;
    errs: Err[];
}
export type VokiItemHidableElements = 'Name' | 'Authors' | 'MoreBtn';
export namespace VokiItemViewUtils {
    export function briefInfoToVokiItemOkStateProps(
        voki: PublishedVokiBriefInfo,
        link: string,
        onMoreBtnClick: (mEvent: MouseEvent) => void
    ): VokiItemViewOkStateProps {
        return {
            vokiId: voki.id,
            type: voki.type,
            voki: {
                name: voki.name,
                cover: voki.cover,
                primaryAuthorId: voki.primaryAuthorId,
                coAuthorIds: voki.coAuthorIds
            },
            onMoreBtnClick: onMoreBtnClick,
            link,
            flags: {
                language: voki.language,
                hasMatureContent: voki.hasMatureContent,
                authenticatedOnlyTaking: voki.signedInOnlyTaking
            }
        }
    }
}