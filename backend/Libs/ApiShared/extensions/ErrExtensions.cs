using Microsoft.AspNetCore.Http;

namespace ApiShared.extensions;

public static class ErrExtensions
{
    public static ErrDto[] ToDtoArray(this Err err) {
        List<ErrDto> list = [];

        var current = err;
        while (current != null) {
            list.Add(ErrDto.FromErr(current));
            current = current.Next;
        }

        return list.ToArray();
    }

    public static int ToHttpStatusCode(this Err e) => e.Code switch {
        ErrCodes.NotImplemented => StatusCodes.Status501NotImplemented,
        ErrCodes.NoAccess => StatusCodes.Status403Forbidden,
        ErrCodes.AuthRequired => StatusCodes.Status401Unauthorized,

        >= 11_000 and < 14_000 => StatusCodes.Status400BadRequest, // Validation
        >= 21_000 and < ErrCodes.NotFound.Common => StatusCodes.Status409Conflict, // Business Logic
        >= ErrCodes.NotFound.Common and < 30_000 => StatusCodes.Status404NotFound, // NotFound
        >= 31_000 and < 33_000 => StatusCodes.Status403Forbidden, // Access/Auth

        _ => StatusCodes.Status500InternalServerError
    };
}