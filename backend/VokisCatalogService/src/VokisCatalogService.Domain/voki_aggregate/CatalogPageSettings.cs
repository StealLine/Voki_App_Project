using System.Collections.Immutable;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using SharedKernel.exceptions;

namespace VokisCatalogService.Domain.voki_aggregate;

public class CatalogPageSettings : ValueObject
{
    //list selected by primary author
    public bool ShowVokisRecommendedByPrimaryAuthor { get; }
    public ImmutableHashSet<VokiId> VokiIdsRecommendedByPrimaryAuthor { get; }

    //selected automatically by system
    public bool ShowSimilarVokis { get; }

    //selected automatically by system
    public bool ShowVokisByTheSameAuthors { get; }

    public override IEnumerable<object> GetEqualityComponents() => [
        ShowVokisRecommendedByPrimaryAuthor,
        VokiIdsRecommendedByPrimaryAuthor,
        ShowSimilarVokis,
        ShowVokisByTheSameAuthors
    ];

    private CatalogPageSettings(
        bool showVokisRecommendedByPrimaryAuthor,
        ImmutableHashSet<VokiId> vokiIdsRecommendedByPrimaryAuthor,
        bool showSimilarVokis,
        bool showVokisByTheSameAuthors
    )
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(
            showVokisRecommendedByPrimaryAuthor,
            vokiIdsRecommendedByPrimaryAuthor,
            showSimilarVokis,
            showVokisByTheSameAuthors
        ));
        ShowVokisRecommendedByPrimaryAuthor = showVokisRecommendedByPrimaryAuthor;
        VokiIdsRecommendedByPrimaryAuthor = vokiIdsRecommendedByPrimaryAuthor;
        ShowSimilarVokis = showSimilarVokis;
        ShowVokisByTheSameAuthors = showVokisByTheSameAuthors;
    }

    public static ErrOrNothing CheckForErr(
        bool showVokisRecommendedByPrimaryAuthor,
        ImmutableHashSet<VokiId> vokiIdsRecommendedByPrimaryAuthor,
        bool showSimilarVokis,
        bool showVokisByTheSameAuthors
    )
    {
        if (showVokisRecommendedByPrimaryAuthor && vokiIdsRecommendedByPrimaryAuthor.IsEmpty)
        {
            return ErrFactory.IncorrectFormat("Recommended Vokis list cannot be empty if ShowVokisRecommendedByPrimaryAuthor is true");
        }

        return ErrOrNothing.Nothing;
    }

    public static CatalogPageSettings Default => new(
        showVokisRecommendedByPrimaryAuthor: false,
        vokiIdsRecommendedByPrimaryAuthor: [],
        showSimilarVokis: true,
        showVokisByTheSameAuthors: true
    );
}