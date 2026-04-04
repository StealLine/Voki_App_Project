import { StringUtils } from "./utils/string-utils";

export type VokiType = 'General' | 'Scoring' | 'TierList';

export namespace VokiTypeUtils {
    export function name(type: VokiType): string {
        switch (type) {
            case 'General':
                return 'General';
            case 'Scoring':
                return 'Scoring';
            case 'TierList':
                return 'Tier list';
        }
    }

    export function icon(type: VokiType): string {
        return `#${StringUtils.pascalToKebab(type)}-voki-type-icon`;
    }
    export function all(): VokiType[] {
        return ['General', 'Scoring', 'TierList'];
    }
}
