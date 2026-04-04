export type Err = {
    message: string;
    code?: number;
    details?: string;
}
export type ErrType =
    | 'Unspecified'
    | 'NoAccess'
    | 'AuthRequired'
    | 'NotImplemented'
    | 'NotFound'
    | 'Other';

export namespace ErrUtils {
    export function hasNonEmptyDetails(err: Err): boolean {
        return !!err.details && err.details.trim().length > 0;
    }

    export function hasSpecifiedCode(err: Err): boolean {
        return err.code != null && err.code !== ErrCodes.Unspecified;
    }

    export function hasSomethingExceptMessage(err: Err): boolean {
        return (
            hasNonEmptyDetails(err) ||
            hasSpecifiedCode(err)
        );
    }

    export function createUnknown(details?: string): Err {
        return {
            message: "Unknown error",
            code: ErrCodes.Unspecified,
            details
        };
    }

    export function fromPlain(obj: any): Err {
        return {
            message: String(obj.message ?? "Unknown error"),
            code: typeof obj.code === 'number' ? obj.code : undefined,
            details: typeof obj.details === 'string' ? obj.details : undefined
        };
    }

    export function getErrTypeByCode(e: Err): ErrType {
        const code = e.code;

        if (code == null || code === ErrCodes.Unspecified) {
            return 'Unspecified';
        }

        if (code === ErrCodes.NotImplemented) {
            return 'NotImplemented';
        }

        // NotFound (23_xxx)
        if (code >= ErrCodes.NotFound.Common && code < ErrCodes.NotFound.Common + 1000) {
            return 'NotFound';
        }

        if (code === ErrCodes.NoAccess) {
            return 'NoAccess';
        }

        if (code === ErrCodes.AuthRequired) {
            return 'AuthRequired';
        }

        return 'Other';
    }

    export function isWithVokiNotFoundCode(err: Err): boolean {
        const code = err.code;

        if (typeof code !== "number" || code === 0) {
            return false;
        }

        return (
            code === ErrCodes.NotFound.Voki ||
            code === ErrCodes.NotFound.GeneralVoki ||
            code === ErrCodes.NotFound.VokiContent
        );
    }

}
export namespace ErrCodes {
    export const Unspecified = 0;
    export const ProgramBug = 1;
    export const NotImplemented = 2;


    export namespace NoValue {
        export const Common = 11_100;
        export const AppUserId = 11_101;
        export const VokiId = 11_110;
        export const GeneralVokiId = 11_111;
    }

    export const IncorrectFormat = 12_000;
    export const ValueOutOfRange = 13_000;

    export const Conflict = 21_000;
    export const LimitExceeded = 22_000;

    export namespace NotFound {
        export const Common = 23_000;
        export const User = 23_001;
        export const Voki = 23_010;
        export const GeneralVoki = 23_011;
        export const VokiContent = 23_020;
    }

    export const NoAccess = 31_000;
    export const AuthRequired = 32_000;
}
