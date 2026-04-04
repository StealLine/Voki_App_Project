using System.Collections.Immutable;

namespace TagsService.Application.voki_tags.queries;

public sealed record ListTagsPopularForLanguagesQuery() : IQuery<ListTagsPopularForLanguagesQueryResult>;

internal sealed class ListTagsPopularForLanguagesQueryHandler
    : IQueryHandler<ListTagsPopularForLanguagesQuery, ListTagsPopularForLanguagesQueryResult>
{
    public Task<ErrOr<ListTagsPopularForLanguagesQueryResult>> Handle(ListTagsPopularForLanguagesQuery query,
        CancellationToken ct) {
        Dictionary<Language, ImmutableArray<VokiTagId>> tagsByLanguage = new() {
            [Language.Rus] = [
                VokiTagId.Create("мор_утопия").AsSuccess(),
                VokiTagId.Create("валорант").AsSuccess(),
                VokiTagId.Create("Жан_Поль_Сартр").AsSuccess()
            ],
            [Language.Eng] = [
                VokiTagId.Create("booktok").AsSuccess(),
                VokiTagId.Create("dune").AsSuccess(),
                VokiTagId.Create("elden_ring").AsSuccess()
            ],
            [Language.Spa] = [
                VokiTagId.Create("don_quijote").AsSuccess(),
                VokiTagId.Create("la_casa_de_papel").AsSuccess(),
                VokiTagId.Create("real_madrid").AsSuccess()
            ],
            [Language.Ger] = [
                VokiTagId.Create("die_zauberflote").AsSuccess(),
                VokiTagId.Create("schachnovelle").AsSuccess(),
                VokiTagId.Create("heidegger").AsSuccess()
            ],
            [Language.Fra] = [
                VokiTagId.Create("le_petit_prince").AsSuccess(),
                VokiTagId.Create("les_miserables").AsSuccess(),
                VokiTagId.Create("asterix").AsSuccess()
            ],
            [Language.Ukr] = [
                VokiTagId.Create("гоголь").AsSuccess(),
                VokiTagId.Create("тарас_шевченко").AsSuccess()
            ],
            [Language.Por] = [
                VokiTagId.Create("cidade_de_deus").AsSuccess(),
                VokiTagId.Create("os_lusiadas").AsSuccess(),
                VokiTagId.Create("capoeira").AsSuccess()
            ],
            [Language.Other] = [
                VokiTagId.Create("anime").AsSuccess(),
                VokiTagId.Create("kpop").AsSuccess(),
                VokiTagId.Create("manga").AsSuccess()
            ]
        };
        ListTagsPopularForLanguagesQueryResult result = new(
            [
                VokiTagId.Create("music").AsSuccess(),
                VokiTagId.Create("literature").AsSuccess(),
                VokiTagId.Create("valorant").AsSuccess(),
                VokiTagId.Create("youtube").AsSuccess(),
                VokiTagId.Create("programming").AsSuccess()
            ],
            tagsByLanguage
        );

        return Task.FromResult(ErrOr<ListTagsPopularForLanguagesQueryResult>.Success(result));
    }
}

public record ListTagsPopularForLanguagesQueryResult(
    ImmutableArray<VokiTagId> DefaultSuggestions,
    Dictionary<Language, ImmutableArray<VokiTagId>> TagsByLanguage
);