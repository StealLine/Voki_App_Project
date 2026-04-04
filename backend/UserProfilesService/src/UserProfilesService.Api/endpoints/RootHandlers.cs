using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.commands;
using UserProfilesService.Application.app_users.queries;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/basic-setup-info", GetUserBasicSetupInfo);

        group.MapPost("/save-basic-setup", SaveBasicProfileSetup)
            .WithRequestValidation<SaveBasicProfileSetupRequest>();

        group.MapGet("/settings", GetUserSettings);


        return group;
    }

    private static async Task<IResult> GetUserBasicSetupInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetCurrentUserBasicSetupInfoQuery, GetCurrentUserBasicSetupInfoQueryResult> handler
    ) {
        GetCurrentUserBasicSetupInfoQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<
            GetCurrentUserBasicSetupInfoQueryResult, UserBasicProfileSetupInfoResponse
        >(result);
    }

    private static async Task<IResult> SaveBasicProfileSetup(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<SaveBasicProfileSetupCommand> handler
    ) {
        var request = httpContext.GetValidatedRequest<SaveBasicProfileSetupRequest>();

        SaveBasicProfileSetupCommand command = new(
            request.ProfilePic,
            request.ParsedDisplayName,
            request.PreferredLanguages.ToHashSet(),
            request.ParsedTags
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> GetUserSettings(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetCurrentUserQuery, AppUser> handler
    ) {
        GetCurrentUserQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<AppUser, AllUserSettingsResponse>(result);
    }
}