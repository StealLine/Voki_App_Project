using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.queries;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate.dtos;

namespace UserProfilesService.Api.endpoints;

internal class UsersHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/users/");

        group.MapPost("/preview", GetUserPreviewData)
            .WithRequestValidation<UsersPreviewRequest>();

        group.MapGet("/{userId}/profile-view", GetUserProfileView);

        group.MapGet("/search-to-invite", SearchUsersToInviteByName);
        group.MapGet("/recommended-for-co-author", ListUsersRecommendedForCoAuthor);

        return group;
    }

    private static async Task<IResult> GetUserPreviewData(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListUsersNamesWithProfilePicsQuery, UserPreviewDto[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<UsersPreviewRequest>();

        ListUsersNamesWithProfilePicsQuery query = new(request.ParsedUserIds);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserPreviewDto[], MultipleUsersPreviewResponse>(result);
    }

    private static async Task<IResult> GetUserProfileView(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetUserProfileViewQuery, UserProfileViewDto> handler
    ) {
        AppUserId userId = httpContext.GetAppUserIdFromRoute();

        GetUserProfileViewQuery query = new(userId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserProfileViewDto, UserProfileViewResponse>(result);
    }

    private static async Task<IResult> SearchUsersToInviteByName(
        string searchValue, int limit,
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<SearchUsersToInviteForCoAuthorQuery, UserPreviewWithAllowInvitesSettingDto[]> handler
    ) {
        SearchUsersToInviteForCoAuthorQuery query = new(searchValue, limit);
        var result = await handler.Handle(query, ct);

        return CustomResults
            .FromErrOrToJson<UserPreviewWithAllowInvitesSettingDto[], ListUsersToInviteResponse>(result);
    }

    private static async Task<IResult> ListUsersRecommendedForCoAuthor(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListUsersRecommendedForCoAuthorQuery, UserPreviewWithAllowInvitesSettingDto[]> handler
    ) {
        ListUsersRecommendedForCoAuthorQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults
            .FromErrOrToJson<UserPreviewWithAllowInvitesSettingDto[], ListUsersToInviteResponse>(result);
    }
}