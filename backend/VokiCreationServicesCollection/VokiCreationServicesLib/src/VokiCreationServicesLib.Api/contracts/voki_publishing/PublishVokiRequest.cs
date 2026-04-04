using SharedKernel.common.rules;

namespace VokiCreationServicesLib.Api.contracts.voki_publishing;

public class PublishVokiRequest : IRequestWithValidationNeeded
{
    public string[] CoAuthorIdsToPublishWith { get; init; }
    public string[] ManagerIdsToPublishWith { get; init; }

    public ErrOrNothing Validate() {
        if (CoAuthorIdsToPublishWith.Length > VokiRules.MaxCoAuthors) {
            return ErrFactory.LimitExceeded(
                $"Too many users to become co-authors are selected. Maximum number is: {VokiRules.MaxCoAuthors}",
                $"You have selected: {CoAuthorIdsToPublishWith.Length}"
            );
        }


        var coAuthorRes = ParseUserIds(CoAuthorIdsToPublishWith);
        if (coAuthorRes.IsErr(out var err)) {
            return err;
        }

        var managerIdRes = ParseUserIds(ManagerIdsToPublishWith);
        if (managerIdRes.IsErr(out err)) {
            return err;
        }

        ParsedCoAuthorIds = coAuthorRes.AsSuccess();
        ParsedUserIdsToBecomeManagers = managerIdRes.AsSuccess();


        return ErrOrNothing.Nothing;
    }

    public ISet<AppUserId> ParsedCoAuthorIds { get; private set; }
    public ISet<AppUserId> ParsedUserIdsToBecomeManagers { get; private set; }

    private ErrOr<ISet<AppUserId>> ParseUserIds(string[] idsToParse) {
        string[] incorrectVals = idsToParse
            .Where(u => !Guid.TryParse(u, out _))
            .ToArray();
        if (incorrectVals.Length > 0) {
            return ErrFactory.IncorrectFormat(
                $"Some of the selected users({incorrectVals.Length}) could not be identified",
                $"Incorrect user ids: {string.Join(", ", incorrectVals)}"
            );
        }

        return idsToParse
            .Select(v => new AppUserId(new(v)))
            .ToImmutableHashSet();
    }
}