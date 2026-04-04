using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateExpectedManagersCommand(
    VokiId VokiId,
    VokiExpectedManagersSetting NewSettingValue
) :
    ICommand<VokiExpectedManagersSetting>,
    IWithAuthCheckStep;

internal sealed class UpdateExpectedManagersCommandHandler
    : ICommandHandler<UpdateExpectedManagersCommand, VokiExpectedManagersSetting>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public UpdateExpectedManagersCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<VokiExpectedManagersSetting>> Handle(UpdateExpectedManagersCommand command, CancellationToken ct) {
        DraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("There is no such draft Voki");
        }

        var res = voki.UpdateExpectedManagers(command.UserCtx(_userCtxProvider), command.NewSettingValue);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.ExpectedManagers;
    }
}