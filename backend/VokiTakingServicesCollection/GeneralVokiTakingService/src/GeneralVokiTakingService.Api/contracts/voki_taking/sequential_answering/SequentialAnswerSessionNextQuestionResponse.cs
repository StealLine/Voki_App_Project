using GeneralVokiTakingService.Api.contracts.voki_taking.shared;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;

public record SequentialAnswerSessionNextQuestionResponse(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    IVokiTakingQuestionContentPrimitiveDto Content,
    DateTime ServerShownAt
) : GeneralVokiTakingResponseQuestionData(
        Id, Text, ImageKeys, ImagesAspectRatio, OrderInVokiTaking,
        MinAnswersCount: MinAnswersCount,
        MaxAnswersCount: MaxAnswersCount,
        Content
    ),
    ICreatableResponse<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult>
{
    public static ICreatableResponse<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult> Create(
        AnswerQuestionInSequentialAnsweringVokiTakingCommandResult commandRes
    ) => new SequentialAnswerSessionNextQuestionResponse(
        commandRes.Question.Id.ToString(),
        commandRes.Question.Text,
        commandRes.Question.ImagesSet.Keys.Select(k => k.ToString()).ToArray(),
        commandRes.Question.ImagesSet.AspectRatio,
        commandRes.Question.OrderInVokiTaking.Value,
        commandRes.Question.MinAnswersCount,
        commandRes.Question.MaxAnswersCount,
        commandRes.Question.Content,
        ServerShownAt: commandRes.CurrentTime
    );
}