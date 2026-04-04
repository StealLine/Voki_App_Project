using SharedKernel;
using VokiCreationServicesLib.Application;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.publishing;

public sealed record PublishVokiWithWarningsIgnoredCommand(
    VokiId VokiId,
    ISet<AppUserId> ConfirmedCoAuthorIds,
    ISet<AppUserId> ConfirmedManagerIds
) :
    ICommand<VokiSuccessfullyPublishedResult>,
    IWithAuthCheckStep;

internal sealed class PublishVokiWithWarningsIgnoredCommandHandler :
    ICommandHandler<PublishVokiWithWarningsIgnoredCommand, VokiSuccessfullyPublishedResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserCtxProvider _userCtxProvider;

    public PublishVokiWithWarningsIgnoredCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IDateTimeProvider dateTimeProvider,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _dateTimeProvider = dateTimeProvider;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiSuccessfullyPublishedResult>> Handle(
        PublishVokiWithWarningsIgnoredCommand command,
        CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsAndResultsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki you are trying to publish doesn't exist. Maybe it has been already published");
        }

        ErrOrNothing publishingRes = voki.PublishWithWarningsIgnored(
            command.UserCtx(_userCtxProvider),
            _dateTimeProvider.UtcNow,
            confirmedCoAuthorIds: command.ConfirmedCoAuthorIds,
            confirmedManagerIds: command.ConfirmedManagerIds
        );

        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Delete(voki, ct);
        return new VokiSuccessfullyPublishedResult(voki.Id, voki.Cover, voki.Name);
    }
}