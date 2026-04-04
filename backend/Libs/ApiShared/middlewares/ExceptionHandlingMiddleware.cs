using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernel.exceptions;

namespace ApiShared.middlewares;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    ) {
        _next = next;
        _logger = logger;
    }

    private static readonly Err ServerError = new("Server error occurred. Please try again later");

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (InvalidConstructorArgumentException ex) {
            _logger.LogCritical(ex.Err.ToStringWithField("Caller", ex.Caller));
            await CustomResults.ErrorResponse(ServerError).ExecuteAsync(context);
            return;
        }
        catch (UnexpectedBehaviourException ex) {
            _logger.LogCritical(ex.Err.ToStringWithFields(
                ("Caller", ex.Caller),
                ("UserMessage", ex.UserMessage)
            ));

            Err errToReturn = string.IsNullOrEmpty(ex.UserMessage)
                ? ServerError
                : ErrFactory.Unspecified(ex.UserMessage);

            await CustomResults.ErrorResponse(errToReturn).ExecuteAsync(context);
            return;
        }
        catch (Exception ex) {
            _logger.LogCritical(ex.ToString());

            await CustomResults.ErrorResponse(ServerError).ExecuteAsync(context);
            return;
        }
    }
}