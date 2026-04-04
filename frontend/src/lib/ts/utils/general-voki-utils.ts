import type { GeneralVokiQuestionContentType } from "../voki";

export namespace GeneralVokiUtils {
    export function questionContentTypeDisplayName(type: GeneralVokiQuestionContentType): string {
        switch (type) {
            case 'TextOnly':
                return 'Text only';
            case 'ImageOnly':
                return 'Image only';
            case 'ImageAndText':
                return 'Image and text';
            case 'ColorOnly':
                return 'Color only';
            case 'ColorAndText':
                return 'Color and text';
            case 'AudioOnly':
                return 'Audio only';
            case 'AudioAndText':
                return 'Audio and text';
        }
    }
}
