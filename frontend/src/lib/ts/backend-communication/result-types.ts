import type { Err } from "../err";

export type ResponseResult<T> =
    | { isSuccess: true; data: T }
    | { isSuccess: false; errs: Err[] };
export type ResponseVoidResult =
    | { isSuccess: true }
    | { isSuccess: false; errs: Err[] };