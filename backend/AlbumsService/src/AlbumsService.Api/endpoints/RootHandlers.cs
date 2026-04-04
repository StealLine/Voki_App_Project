using AlbumsService.Api.contracts;
using AlbumsService.Application.app_users.commands;
using AlbumsService.Application.app_users.queries;
using AlbumsService.Application.voki_albums.commands;
using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;

namespace AlbumsService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/all-albums-preview", GetAllUserAlbumsPreview);

        group.MapPost("/albums/create-new", CreateNewAlbum)
            .WithRequestValidation<SaveVokiAlbumRequest>();

        group.MapPost("/update-auto-albums-appearance", UpdateAutoAlbumsAppearance)
            .WithRequestValidation<UpdateAutoAlbumsAppearanceRequest>();

        return group;
    }

    private static async Task<IResult> GetAllUserAlbumsPreview(
        CancellationToken ct, IQueryHandler<ListAllUserAlbumsPreviewQuery, ListAllUserAlbumsPreviewQueryResult> handler
    ) {
        ListAllUserAlbumsPreviewQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ListAllUserAlbumsPreviewQueryResult, AllAlbumsPreviewResponse>(result);
    }

    private static async Task<IResult> CreateNewAlbum(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<CreateNewAlbumCommand, VokiAlbum> handler
    ) {
        var request = httpContext.GetValidatedRequest<SaveVokiAlbumRequest>();

        CreateNewAlbumCommand command = new(
            request.ParsedName, request.ParsedIcon, request.ParsedMainColor, request.ParsedSecondaryColor
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiAlbum, AlbumCreatedResponse>(result);
    }

    private static async Task<IResult> UpdateAutoAlbumsAppearance(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateAutoAlbumsAppearanceCommand, UserAutoAlbumsAppearance> handler
    ) {
        var request = httpContext.GetValidatedRequest<UpdateAutoAlbumsAppearanceRequest>();

        UpdateAutoAlbumsAppearanceCommand command = new(request.ParsedAppearance);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<UserAutoAlbumsAppearance, AutoAlbumsAppearanceResponse>(result);
    }
}