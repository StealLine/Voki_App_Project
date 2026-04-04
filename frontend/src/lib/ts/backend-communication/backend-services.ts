import { DateUtils } from "../utils/date-utils";
import { ErrUtils, type Err } from "../err";
import type { ResponseResult, ResponseVoidResult } from "./result-types";



export class BackendService {
    private _baseUrl: string;

    constructor(baseUrl: string) {
        this._baseUrl = baseUrl;
    }
    public async fetchJsonResponse<T>(url: string, options: RequestInit): Promise<ResponseResult<T>> {
        try {

            const response = await fetch(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });
            if (response.ok) {
                const text = await response.text();
                const data = BackendService.parseWithDates<T>(text);
                return { isSuccess: true, data };
            }
            const errs = await this.parseErrResponse(response);

            return { isSuccess: false, errs };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: this.createErrsFromException(e)
            };
        }
    }

    public async fetchVoidResponse(url: string, options: RequestInit): Promise<ResponseVoidResult> {
        try {
            const response = await fetch(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });
            if (response.ok) {
                return { isSuccess: true };
            }

            const errs = await this.parseErrResponse(response);
            return { isSuccess: false, errs };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: this.createErrsFromException(e)
            };
        }
    }

    public async serverFetchJsonResponse<T>(
        fetchFunc: typeof fetch,
        url: string,
        options: RequestInit
    ): Promise<ResponseResult<T>> {
        try {
            const response = await fetchFunc(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });
            if (response.ok) {
                const text = await response.text();
                const data = BackendService.parseWithDates<T>(text);
                return { isSuccess: true, data };
            }

            const errs = await this.parseErrResponse(response);
            return { isSuccess: false, errs };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: this.createErrsFromException(e)
            };
        }
    }

    static parseWithDates<T>(json: string): T {
        return JSON.parse(json, (key, value) => {
            if (typeof value === 'string' && DateUtils.isoDateRegex.test(value)) {
                return new Date(value);
            }
            return value;
        });
    }

    private async parseErrResponse(response: Response): Promise<Err[]> {
        const contentType = response.headers.get("content-type");

        if (contentType?.includes("application/json")) {

            try {
                const json = await response.json();
                if (!Array.isArray(json.errs)) {
                    return [ErrUtils.createUnknown("Expected 'errs' array in response")];
                }
                return json.errs.map(ErrUtils.fromPlain);
            } catch {
                return [ErrUtils.createUnknown("Failed to parse JSON error response")];
            }
        }
        // in case of server unexpected error
        const status = response.status;
        if (status === 404) {
            return [{ message: "Unexpected error: requested resource was not found (404)" }];
        }
        if (status === 403) {
            return [{ message: "Unexpected error: no permission to access this resource (403)" }];
        }
        if (status === 401) {
            return [{ message: "Unexpected error: not authorized (401)" }];
        }
        if (status >= 500) {
            return [{ message: `Unexpected error: Internal server error (${status})` }];
        }

        // fallback for everything else
        return [{ message: `Unexpected error: Unexpected non-JSON error response (status ${status})` }];
    }
    private createErrsFromException(e: unknown): Err[] {
        if (e instanceof TypeError && e.message === "Failed to fetch") {
            // Server doesn't respond or no connection
            return [{ message: "Could not connect to the server. Please check your connection or try again later" }];
        }

        if (e instanceof DOMException && e.name === "AbortError") {
            // Request was aborted
            return [{ message: "The request was aborted" }];
        }

        return [{ message: "Unexpected unhandled error " }];
    }

}
export const ApiAuth = new BackendService('/api/auth');
export const ApiVokiCreationCore = new BackendService('/api/voki-creation/core');
export const ApiTags = new BackendService('/api/tags');
export const ApiUserProfiles = new BackendService('/api/user-profiles');
export const ApiVokisCatalog = new BackendService('/api/vokis-catalog');
export const ApiVokiComments = new BackendService('/api/voki-comments');
export const ApiVokiRatings = new BackendService('/api/voki-ratings');
export const ApiAlbums = new BackendService('/api/albums');
export const ApiSubscriptions = new BackendService('/api/subscriptions');

export const ApiVokiTakingGeneral = new BackendService('/api/voki-taking/general');


export const RJO = {
    POST: (data: any): RequestInit => formRequestJsonOptions(data, "POST"),
    PUT: (data: any): RequestInit => formRequestJsonOptions(data, "PUT"),
    PATCH: (data: any): RequestInit => formRequestJsonOptions(data, "PATCH"),
    DELETE: (data: any): RequestInit => formRequestJsonOptions(data, "DELETE"),
};

function formRequestJsonOptions(data: any, method: string): RequestInit {
    return {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    };
}