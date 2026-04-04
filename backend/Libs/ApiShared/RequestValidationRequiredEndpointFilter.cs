using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ApiShared;

internal class RequestValidationRequiredEndpointFilter<T> : IEndpointFilter where T : IRequestWithValidationNeeded
{
    private readonly ILogger<RequestValidationRequiredEndpointFilter<T>> _logger;

    public RequestValidationRequiredEndpointFilter(ILogger<RequestValidationRequiredEndpointFilter<T>> logger) {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var httpContext = context.HttpContext;
        var logPrefix = $"[{typeof(T).Name}] request validation";

        // not application/json
        bool isContentJson =
            httpContext.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? false;
        if (!isContentJson) {
            _logger.LogWarning("{Log} invalid Content-Type: {ContentType}", logPrefix, httpContext.Request.ContentType);
            return CustomResults.ErrorResponse(ErrFactory.IncorrectFormat(
                "Invalid Content-Type. Expected application/json"
            ));
        }

        // empty body
        if (httpContext.Request.ContentLength == 0) {
            _logger.LogWarning("{Log} empty request body", logPrefix);
            return CustomResults.ErrorResponse(ErrFactory.NoValue.Common(
                "Request body cannot be empty when Content-Type is application/json"
            ));
        }

        T? request;

        try {
            request = await httpContext.Request.ReadFromJsonAsync<T>();
        }
        catch (JsonException ex) {
            _logger.LogError(ex,
                "{Log} JSON deserialization failed: message={Message} path={Path} line={Line} bytePos={BytePos}",
                logPrefix, ex.Message, ex.Path, ex.LineNumber, ex.BytePositionInLine
            );

            return CustomResults.ErrorResponse(new Err("Server error: unable to read client request from json"));
        }
        catch (Exception ex) {
            _logger.LogError(ex, "{Log} unexpected error during deserialization: message={Message}", logPrefix, ex.Message);
            return CustomResults.ErrorResponse(new Err("Server error: unable to read client request from json"));
        }

        if (request is null || request is not T validatableRequest) {
            _logger.LogWarning("{Log} deserialized request is null or wrong type", logPrefix);
            return CustomResults.ErrorResponse(ErrFactory.IncorrectFormat("Invalid request body format"));
        }

        ErrOrNothing validationResult = validatableRequest.Validate();
        if (validationResult.IsErr(out var err)) {
            _logger.LogWarning("{Log} validation failed: errCode={ErrCode} message={Message}", logPrefix, err.Code, err.Message);
            return CustomResults.ErrorResponse(err);
        }

        _logger.LogInformation("{Log} validation successful", logPrefix);

        httpContext.Items["validatedRequest"] = validatableRequest;
        return await next(context);
    }
}