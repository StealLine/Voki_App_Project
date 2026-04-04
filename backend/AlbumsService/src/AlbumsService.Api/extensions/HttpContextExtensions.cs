using SharedKernel.domain.ids;
using SharedKernel.errs.utils;
using SharedKernel.exceptions;

namespace AlbumsService.Api.extensions;

public static class HttpContextExtensions
{
    public static VokiAlbumId GetAlbumIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["albumId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid voki album id",
                    $"'{idString}' is not a valid ${nameof(VokiAlbumId)}"
                ),
                userMessage: "Invalid album id. Couldn't parse album id from route"
            );
        }

        return new VokiAlbumId(guid);
    }
}