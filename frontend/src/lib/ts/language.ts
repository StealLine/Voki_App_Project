import { StringUtils } from "./utils/string-utils";

const allLanguages = ['Eng', 'Rus', 'Spa', 'Ger', 'Fra', 'Ukr', 'Por', 'Other'] as const;
export type Language = typeof allLanguages[number];

export namespace LanguageUtils {
    export function name(language: Language): string {
        switch (language) {
            case 'Eng': return 'English';
            case 'Rus': return 'Russian';
            case 'Spa': return 'Spanish';
            case 'Ger': return 'German';
            case 'Fra': return 'French';
            case 'Ukr': return 'Ukrainian';
            case 'Por': return 'Portuguese';
            case 'Other': return 'Other';
        }
    }

export function icon(lang: Language): string {
    return `#languages-icons-${StringUtils.pascalToKebab(lang)}`;
}
    export function values(): Language[] {
        return [...allLanguages];
    }
}
