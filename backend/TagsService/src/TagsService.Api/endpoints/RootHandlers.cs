using TagsService.Api.contracts;
using TagsService.Application.voki_tags.queries;

namespace TagsService.Api.endpoints;

internal  class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/search", SearchTags);
        group.MapGet("/tags-popular-for-languages", ListTagsPopularForLanguages);

        return group;
    }
    private static async Task<IResult> SearchTags(
        CancellationToken ct,
        IQueryHandler<SearchTagsQuery, ImmutableArray<VokiTagId>> handler,
        string searchValue,
        int count = 10
    ) {
        SearchTagsQuery query = new(searchValue, count);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (tags) => Results.Json(
            new { Tags = tags.Select(t => t.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> ListTagsPopularForLanguages(
        CancellationToken ct,
        IQueryHandler<ListTagsPopularForLanguagesQuery, ListTagsPopularForLanguagesQueryResult> handler
    ) {
        ListTagsPopularForLanguagesQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ListTagsPopularForLanguagesQueryResult, TagsPopularForLanguagesResponse>(result);
    }
}