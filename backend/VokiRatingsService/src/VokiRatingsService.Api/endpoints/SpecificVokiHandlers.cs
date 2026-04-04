using ApiShared.extensions;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.voki_ratings.commands;
using VokiRatingsService.Application.voki_ratings.queries;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/ratings", GetVokiRatingsData);
        group.MapGet("/all-with-average", GetVokiOtherUsersRatingsWithAverage);

        group.MapPatch("/rate", RateVoki)
            .WithRequestValidation<RateVokiRequest>();
        
        return group;
    }

    private static async Task<IResult> GetVokiRatingsData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        UserRatingsDataForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserRatingsDataForVokiQueryResult, UserRatingsDataForVokiResponse>(result);
    }

    private static async Task<IResult> GetVokiOtherUsersRatingsWithAverage(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListRatingsForVokiQuery, VokiRating[]> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ListRatingsForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiRating[], RatingsWithAverageResponse>(result);
    }

    private static async Task<IResult> RateVoki(
        HttpContext httpContext, CancellationToken ct, ICommandHandler<RateVokiCommand, VokiRating> handler
    ) {
        var request = httpContext.GetValidatedRequest<RateVokiRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        RateVokiCommand command = new(vokiId, request.ParsedRating);
        ErrOr<VokiRating> result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiRating, VokiRatingDataResponse>(result);
    }
}