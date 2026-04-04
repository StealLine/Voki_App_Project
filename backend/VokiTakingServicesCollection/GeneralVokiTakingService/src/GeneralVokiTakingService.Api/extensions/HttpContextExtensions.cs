using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Api.extensions;

public static class HttpContextExtensions
{
    public static GeneralVokiResultId GetResultIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["resultId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid result",
                    $"'{idString}' is not a valid ${nameof(GeneralVokiResultId)}"
                ),
                userMessage: "Invalid result id. Couldn't parse result id from route"
            );
        }

        return new GeneralVokiResultId(guid);
    }
}