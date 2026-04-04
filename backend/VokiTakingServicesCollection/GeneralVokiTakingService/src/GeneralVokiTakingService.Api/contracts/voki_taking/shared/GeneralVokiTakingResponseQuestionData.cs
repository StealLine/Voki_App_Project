using GeneralVokiTakingService.Application.dtos;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared;

public record GeneralVokiTakingResponseQuestionData(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    IVokiTakingQuestionContentPrimitiveDto Content
)
{
    public static GeneralVokiTakingResponseQuestionData FromQuestion(VokiTakingQuestionData question) => new(
        question.Id.ToString(),
        question.Text,
        question.ImagesSet.Keys.Select(k => k.ToString()).ToArray(),
        question.ImagesSet.AspectRatio,
        question.OrderInVokiTaking.Value,
        question.MinAnswersCount,
        question.MaxAnswersCount,
        question.Content
    );
}