using ApiShared.extensions;
using Microsoft.AspNetCore.Http;

namespace ApiShared;

public static class CustomResults
{
    public static IResult ErrorResponse(Err e) =>
        Results.Json(new ErrResponseObject(e.ToDtoArray()), statusCode: e.ToHttpStatusCode());


    public static IResult FromErrOrNothing(ErrOrNothing possibleErr, Func<IResult> successFunc) =>
        possibleErr.IsErr(out var err) ? ErrorResponse(err) : successFunc();

    public static IResult FromErrOr<T>(ErrOr<T> errOrValue, Func<T, IResult> successFunc) =>
        errOrValue.Match(successFunc, ErrorResponse);

    public static IResult FromErrOrToJson<TSuccess, TResponse>(
        ErrOr<TSuccess> errOrValue
    ) where TResponse : ICreatableResponse<TSuccess> =>
        FromErrOr(errOrValue, (success) => Results.Json(TResponse.Create(success)));


    public static IResult Created(object responseObject) =>
        Results.Json(responseObject, statusCode: StatusCodes.Status201Created);

    public static IResult Deleted() => Results.StatusCode(StatusCodes.Status202Accepted);
}

