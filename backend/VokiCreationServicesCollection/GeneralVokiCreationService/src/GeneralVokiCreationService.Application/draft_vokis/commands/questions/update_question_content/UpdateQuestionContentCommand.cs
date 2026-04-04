using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;

public sealed record UpdateQuestionContentCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    BaseUnsavedQuestionContentDto NewContent
) :
    ICommand<BaseQuestionTypeSpecificContent>,
    IWithAuthCheckStep;

internal sealed class UpdateQuestionContentCommandHandler
    : ICommandHandler<UpdateQuestionContentCommand, BaseQuestionTypeSpecificContent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateQuestionContentCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<BaseQuestionTypeSpecificContent>> Handle(UpdateQuestionContentCommand command, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsAndResultsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOr<BaseQuestionTypeSpecificContent> savedContent = await SaveContent(
            command.VokiId, command.QuestionId, command.NewContent, ct
        );
        if (savedContent.IsErr(out var err)) {
            return err;
        }

        ErrOr<BaseQuestionTypeSpecificContent> res = voki.UpdateQuestionTypeSpecificContent(
            command.UserCtx(_userCtxProvider),
            command.QuestionId,
            savedContent.AsSuccess()
        );
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> SaveContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, BaseUnsavedQuestionContentDto unsavedContent, CancellationToken ct
    ) => await unsavedContent.Match<Task<ErrOr<BaseQuestionTypeSpecificContent>>>(
        textOnly: (dto) => Task.FromResult(dto.ToSavedContent().Bind<BaseQuestionTypeSpecificContent>(s => s)),
        imageOnly: (dto) => CreateImageOnlyContent(vokiId, questionId, dto, ct),
        imageAndText: (dto) => CreateImageAndTextContent(vokiId, questionId, dto, ct),
        colorOnly: (dto) => Task.FromResult(dto.ToSavedContent().Bind<BaseQuestionTypeSpecificContent>(s => s)),
        colorAndText: (dto) => Task.FromResult(dto.ToSavedContent().Bind<BaseQuestionTypeSpecificContent>(s => s)),
        audioOnly: (dto) => CreateAudioOnlyContent(vokiId, questionId, dto, ct),
        audioAndText: (dto) => CreateAudioAndTextContent(vokiId, questionId, dto, ct)
    );

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> CreateImageOnlyContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, ImageOnlyUnsavedQuestionContentDto content,
        CancellationToken ct
    ) {
        List<BaseQuestionAnswer.ImageOnly> savedAnswers = new(content.Answers.Length);
        List<(ImageOnlyUnsavedQuestionContentDto.Answer, TempImageKey)> unsavedAnswers = new();
        foreach (var answer in content.Answers) {
            if (GeneralVokiAnswerImageKey.FromString(answer.ImageKey).IsSuccess(out var savedKey)) {
                savedAnswers.Add(new BaseQuestionAnswer.ImageOnly(savedKey, answer.Order, answer.RelatedResultIds));
                continue;
            }

            ErrOr<TempImageKey> tempKeyCreationRes = TempImageKey.FromString(answer.ImageKey);
            if (tempKeyCreationRes.IsErr(out var keyErr)) {
                return ErrFactory.IncorrectFormat(
                    $"Some of the answers has incorrect image data. Answer order: {answer.Order.Value}",
                    $"Image key: {answer.ImageKey}. Key err: {keyErr}"
                );
            }

            unsavedAnswers.Add((answer, tempKeyCreationRes.AsSuccess()));
        }

        if (unsavedAnswers.Any()) {
            Dictionary<TempImageKey, GeneralVokiAnswerImageKey> tempToSavedImageKeys = unsavedAnswers.ToDictionary(
                tuple => tuple.Item2,
                tuple => GeneralVokiAnswerImageKey.CreateForAnswerFromTemp(vokiId, questionId, tuple.Item2)
            );

            ErrOrNothing copyRes = await _mainStorageBucket.CopyVokiAnswerImageKeysFromTempToStandard(tempToSavedImageKeys, ct);
            if (copyRes.IsErr(out var err)) {
                return err;
            }

            foreach (var unsaved in unsavedAnswers) {
                savedAnswers.Add(new BaseQuestionAnswer.ImageOnly(
                    tempToSavedImageKeys[unsaved.Item2],
                    unsaved.Item1.Order,
                    unsaved.Item1.RelatedResultIds
                ));
            }
        }

        return QuestionAnswersList<BaseQuestionAnswer.ImageOnly>
            .Create(savedAnswers)
            .Bind<BaseQuestionTypeSpecificContent>(
                answersList => new BaseQuestionTypeSpecificContent.ImageOnly(answersList)
            );
    }

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> CreateImageAndTextContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, ImageAndTextUnsavedQuestionContentDto content,
        CancellationToken ct
    ) {
        List<BaseQuestionAnswer.ImageAndText> savedAnswers = new(content.Answers.Length);
        List<(ImageAndTextUnsavedQuestionContentDto.Answer, TempImageKey)> unsavedAnswers = new();
        foreach (var answer in content.Answers) {
            if (GeneralVokiAnswerImageKey.FromString(answer.ImageKey).IsSuccess(out var savedKey)) {
                savedAnswers.Add(
                    new BaseQuestionAnswer.ImageAndText(answer.Text, savedKey, answer.Order, answer.RelatedResultIds));
                continue;
            }

            ErrOr<TempImageKey> tempKeyCreationRes = TempImageKey.FromString(answer.ImageKey);
            if (tempKeyCreationRes.IsErr(out var keyErr)) {
                return ErrFactory.IncorrectFormat(
                    $"Some of the answers has incorrect image data. Answer order: {answer.Order.Value}",
                    $"Image key: {answer.ImageKey}. Key err: {keyErr}"
                );
            }

            unsavedAnswers.Add((answer, tempKeyCreationRes.AsSuccess()));
        }

        if (unsavedAnswers.Any()) {
            Dictionary<TempImageKey, GeneralVokiAnswerImageKey> tempToSavedImageKeys = unsavedAnswers.ToDictionary(
                tuple => tuple.Item2,
                tuple => GeneralVokiAnswerImageKey.CreateForAnswerFromTemp(vokiId, questionId, tuple.Item2)
            );

            ErrOrNothing copyRes = await _mainStorageBucket.CopyVokiAnswerImageKeysFromTempToStandard(tempToSavedImageKeys, ct);
            if (copyRes.IsErr(out var err)) {
                return err;
            }

            foreach (var unsaved in unsavedAnswers) {
                savedAnswers.Add(new BaseQuestionAnswer.ImageAndText(
                    unsaved.Item1.Text,
                    tempToSavedImageKeys[unsaved.Item2],
                    unsaved.Item1.Order,
                    unsaved.Item1.RelatedResultIds
                ));
            }
        }

        return QuestionAnswersList<BaseQuestionAnswer.ImageAndText>
            .Create(savedAnswers)
            .Bind<BaseQuestionTypeSpecificContent>(
                answersList => new BaseQuestionTypeSpecificContent.ImageAndText(answersList)
            );
    }

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> CreateAudioOnlyContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, AudioOnlyUnsavedQuestionContentDto content,
        CancellationToken ct
    ) {
        List<BaseQuestionAnswer.AudioOnly> savedAnswers = new(content.Answers.Length);
        List<(AudioOnlyUnsavedQuestionContentDto.Answer, TempAudioKey)> unsavedAnswers = new();
        foreach (var answer in content.Answers) {
            if (GeneralVokiAnswerAudioKey.FromString(answer.AudioKey).IsSuccess(out var savedKey)) {
                savedAnswers.Add(new BaseQuestionAnswer.AudioOnly(savedKey, answer.Order, answer.RelatedResultIds));
                continue;
            }

            ErrOr<TempAudioKey> tempKeyCreationRes = TempAudioKey.FromString(answer.AudioKey);
            if (tempKeyCreationRes.IsErr(out var keyErr)) {
                return ErrFactory.IncorrectFormat(
                    $"Some of the answers has incorrect audio data. Answer order: {answer.Order.Value}",
                    $"Audio key: {answer.AudioKey}. Key err: {keyErr}"
                );
            }

            unsavedAnswers.Add((answer, tempKeyCreationRes.AsSuccess()));
        }

        if (unsavedAnswers.Any()) {
            Dictionary<TempAudioKey, GeneralVokiAnswerAudioKey> tempToSavedAudioKeys = unsavedAnswers.ToDictionary(
                tuple => tuple.Item2,
                tuple => GeneralVokiAnswerAudioKey.CreateForAnswerFromTemp(vokiId, questionId, tuple.Item2)
            );

            ErrOrNothing copyRes = await _mainStorageBucket.CopyVokiAnswerAudioKeysFromTempToStandard(tempToSavedAudioKeys, ct);
            if (copyRes.IsErr(out var err)) {
                return err;
            }

            foreach (var unsaved in unsavedAnswers) {
                savedAnswers.Add(new BaseQuestionAnswer.AudioOnly(
                    tempToSavedAudioKeys[unsaved.Item2],
                    unsaved.Item1.Order,
                    unsaved.Item1.RelatedResultIds
                ));
            }
        }

        return QuestionAnswersList<BaseQuestionAnswer.AudioOnly>
            .Create(savedAnswers)
            .Bind<BaseQuestionTypeSpecificContent>(
                answersList => new BaseQuestionTypeSpecificContent.AudioOnly(answersList)
            );
    }

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> CreateAudioAndTextContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, AudioAndTextUnsavedQuestionContentDto content,
        CancellationToken ct
    ) {
        List<BaseQuestionAnswer.AudioAndText> savedAnswers = new(content.Answers.Length);
        List<(AudioAndTextUnsavedQuestionContentDto.Answer, TempAudioKey)> unsavedAnswers = new();
        foreach (var answer in content.Answers) {
            if (GeneralVokiAnswerAudioKey.FromString(answer.AudioKey).IsSuccess(out var savedKey)) {
                savedAnswers.Add(
                    new BaseQuestionAnswer.AudioAndText(answer.Text, savedKey, answer.Order, answer.RelatedResultIds));
                continue;
            }

            ErrOr<TempAudioKey> tempKeyCreationRes = TempAudioKey.FromString(answer.AudioKey);
            if (tempKeyCreationRes.IsErr(out var keyErr)) {
                return ErrFactory.IncorrectFormat(
                    $"Some of the answers has incorrect audio data. Answer order: {answer.Order.Value}",
                    $"Audio key: {answer.AudioKey}. Key err: {keyErr}"
                );
            }

            unsavedAnswers.Add((answer, tempKeyCreationRes.AsSuccess()));
        }

        if (unsavedAnswers.Any()) {
            Dictionary<TempAudioKey, GeneralVokiAnswerAudioKey> tempToSavedAudioKeys = unsavedAnswers.ToDictionary(
                tuple => tuple.Item2,
                tuple => GeneralVokiAnswerAudioKey.CreateForAnswerFromTemp(vokiId, questionId, tuple.Item2)
            );

            ErrOrNothing copyRes = await _mainStorageBucket.CopyVokiAnswerAudioKeysFromTempToStandard(tempToSavedAudioKeys, ct);
            if (copyRes.IsErr(out var err)) {
                return err;
            }

            foreach (var unsaved in unsavedAnswers) {
                savedAnswers.Add(new BaseQuestionAnswer.AudioAndText(
                    unsaved.Item1.Text,
                    tempToSavedAudioKeys[unsaved.Item2],
                    unsaved.Item1.Order,
                    unsaved.Item1.RelatedResultIds
                ));
            }
        }

        return QuestionAnswersList<BaseQuestionAnswer.AudioAndText>
            .Create(savedAnswers)
            .Bind<BaseQuestionTypeSpecificContent>(
                answersList => new BaseQuestionTypeSpecificContent.AudioAndText(answersList)
            );
    }
}