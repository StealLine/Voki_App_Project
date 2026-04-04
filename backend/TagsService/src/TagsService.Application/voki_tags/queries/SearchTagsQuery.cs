using System.Collections.Immutable;

namespace TagsService.Application.voki_tags.queries;

public sealed record SearchTagsQuery(string SearchValue, int Count) :
    IQuery<ImmutableArray<VokiTagId>>;

internal sealed class SearchTagsQueryHandler : IQueryHandler<SearchTagsQuery, ImmutableArray<VokiTagId>>
{
    public Task<ErrOr<ImmutableArray<VokiTagId>>> Handle(SearchTagsQuery query, CancellationToken ct) {
        if (!VokiTagId.IsStringValidTag(query.SearchValue)) {
            return Task.FromResult<ErrOr<ImmutableArray<VokiTagId>>>(ErrFactory.IncorrectFormat($"'{query.SearchValue}' is not a valid tag"));
        }

        return Task.FromResult(ErrOr<ImmutableArray<VokiTagId>>.Success([new VokiTagId(query.SearchValue)]));
    }
}