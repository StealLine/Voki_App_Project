using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Api.extensions;

public static class HttpContextExtensions
{
    public static GeneralVokiQuestionId GetQuestionIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["questionId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid question id",
                    $"'{idString}' is not a valid ${nameof(GeneralVokiQuestionId)}"
                ),
                userMessage: "Invalid question id. Couldn't parse question id from route"
            );
        }

        return new GeneralVokiQuestionId(guid);
    }

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

    public static GeneralVokiAnswerId GetAnswerIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["answerId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid result",
                    $"'{idString}' is not a valid ${nameof(GeneralVokiAnswerId)}"
                ),
                userMessage: "Invalid answer id. Couldn't parse answer id from route"
            );
        }

        return new GeneralVokiAnswerId(guid);
    }
}