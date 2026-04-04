namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

internal class ListVokisBriefInfoRequest : IRequestWithValidationNeeded
{
    public string[] Ids { get; init; } = [];
    private const int MaxAllowedIds = 200;

    public ErrOrNothing Validate() {
        if (Ids.Length == 0) {
            return ErrFactory.NoValue.Common("No voki ids were provided");
        }

        if (Ids.Length > MaxAllowedIds) {
            return ErrFactory.ValueOutOfRange(
                $"Too many voki ids specified. Cannot handle more than {MaxAllowedIds} ids at a time",
                $"Provided ids count: {Ids.Length}"
            );
        }

        bool anyCorrect = Ids.Any(id => Guid.TryParse(id, out _));
        return anyCorrect
            ? ErrOrNothing.Nothing
            : ErrFactory.IncorrectFormat($"All ({Ids.Length}) of provided ids were incorrect");
    }

    public VokiId[] ParsedVokiIds => Ids
        .Where(i => Guid.TryParse(i, out var _))
        .Select(i => new VokiId(new(i)))
        .ToArray();
}