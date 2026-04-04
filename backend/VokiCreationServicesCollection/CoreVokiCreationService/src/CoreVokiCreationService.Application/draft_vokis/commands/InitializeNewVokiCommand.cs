using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record InitializeNewVokiCommand(
    VokiType VokiType,
    VokiName VokiName
) : ICommand<DraftVoki>,
    IWithAuthCheckStep;

internal sealed class InitializeNewVokiCommandHandler : ICommandHandler<InitializeNewVokiCommand, DraftVoki>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMainStorageBucket _mainStorageBucket;

    public InitializeNewVokiCommandHandler(
        IUserCtxProvider userCtxProvider,
        IDraftVokiRepository draftVokiRepository,
        IDateTimeProvider dateTimeProvider,
        IMainStorageBucket mainStorageBucket
    ) {
        _userCtxProvider = userCtxProvider;
        _draftVokiRepository = draftVokiRepository;
        _dateTimeProvider = dateTimeProvider;
        _mainStorageBucket = mainStorageBucket;
    }

    private static readonly VokiType[] ImplementedTypes = [VokiType.General];

    public async Task<ErrOr<DraftVoki>> Handle(InitializeNewVokiCommand command, CancellationToken ct) {
        if (!ImplementedTypes.Contains(command.VokiType)) {
            return ErrFactory.NotImplemented("Specified voki type is not implemented");
        }

        DraftVoki voki = DraftVoki.Create(
            command.VokiName, command.VokiType,
            aUserCtx: command.UserCtx(_userCtxProvider),
            creationDate: _dateTimeProvider.UtcNow
        );

        var copyRes = await _mainStorageBucket.CopyDefaultVokiCoverForNewVoki(voki.Cover, ct);
        if (copyRes.IsErr()) {
            return ErrFactory.Unspecified(
                "Cannot initialize new voki due to image copying error. Please try again later"
            );
        }

        await _draftVokiRepository.Add(voki, ct);
        return voki;
    }
}