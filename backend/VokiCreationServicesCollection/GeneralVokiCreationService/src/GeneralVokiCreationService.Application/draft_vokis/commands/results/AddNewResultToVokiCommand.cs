using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record AddNewResultToVokiCommand(VokiId VokiId, VokiResultName ResultName) :
    ICommand<ImmutableArray<VokiResult>>,
    IWithAuthCheckStep;

internal sealed class AddNewResultToVokiCommandHandler :
    ICommandHandler<AddNewResultToVokiCommand, ImmutableArray<VokiResult>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserCtxProvider _userCtxProvider;

    public AddNewResultToVokiCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IDateTimeProvider dateTimeProvider,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _dateTimeProvider = dateTimeProvider;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<ImmutableArray<VokiResult>>> Handle(
        AddNewResultToVokiCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithResultsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = command.UserCtx(_userCtxProvider);
        ErrOr<ImmutableArray<VokiResult>> res = voki.AddNewResult(aUserCtx, command.ResultName, _dateTimeProvider.UtcNow);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }
}