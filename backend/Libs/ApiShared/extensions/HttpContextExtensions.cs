using Microsoft.AspNetCore.Http;
using SharedKernel.exceptions;

namespace ApiShared.extensions;

public static class HttpContextExtensions
{
    public static T GetValidatedRequest<T>(this HttpContext context) where T : class, IRequestWithValidationNeeded {
        if (!context.Items.TryGetValue("validatedRequest", out var validatedRequest)) {
            throw new InvalidDataException(
                "Trying to access validated request on the request that has not passed the validation"
            );
        }

        if (validatedRequest is T request) {
            return request;
        }

        throw new InvalidCastException("Request type mismatch");
    }

    public static VokiId GetVokiIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["vokiId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid voki id",
                    $"'{idString}' is not a valid ${nameof(VokiId)}"
                ),
                userMessage: "Invalid voki id. Couldn't parse voki id from route"
            );
        }

        return new VokiId(guid);
    }

    public static AppUserId GetAppUserIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["userId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid user id",
                    $"'{idString}' is not a valid ${nameof(AppUserId)}"
                ),
                userMessage: "Invalid user id. Couldn't parse user id from route"
            );
        }

        return new AppUserId(guid);
    }
}