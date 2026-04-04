using TagsService.Application.voki_tags.queries;

namespace TagsService.Api.contracts;

public record TagsPopularForLanguagesResponse(
    string[] DefaultSuggestions,
    Dictionary<Language, string[]> SuggestionsByLangs
) : ICreatableResponse<ListTagsPopularForLanguagesQueryResult>
{
    public static ICreatableResponse<ListTagsPopularForLanguagesQueryResult> Create(
        ListTagsPopularForLanguagesQueryResult queryRes
    ) => new TagsPopularForLanguagesResponse(
        queryRes.DefaultSuggestions.Select(i => i.ToString()).ToArray(),
        queryRes.TagsByLanguage.ToDictionary(
            i => i.Key,
            i => i.Value.Select(t => t.ToString()).ToArray()
        )
    );
}