using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

internal class VokisHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/");

        group.MapPost("/brief-info", ListVokisBriefInfo)
            .WithRequestValidation<ListVokisBriefInfoRequest>();
        
        return group;

    }

    private static async Task<IResult> ListVokisBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListVokisQuery, DraftVoki[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<ListVokisBriefInfoRequest>();

        ListVokisQuery query = new(request.ParsedVokiIds);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<DraftVoki[], MultipleVokisBriefInfoResponse>(result);
    }
}