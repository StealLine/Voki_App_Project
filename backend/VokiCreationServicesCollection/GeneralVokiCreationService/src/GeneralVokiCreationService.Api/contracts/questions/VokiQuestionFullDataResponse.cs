using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Application.draft_vokis.queries.questions;
using GeneralVokiCreationService.Application.dtos;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

internal record class VokiQuestionFullDataResponse(
    string Id,
    string Text,
    QuestionImageSetResponse ImageSet,
    IQuestionContentPrimitiveDto Content,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    int MaxAnswersForQuestionCount,
    Dictionary<string, string> ResultsIdToName,
    int MaxResultsForAnswerCount
) : ICreatableResponse<GetVokiQuestionWithAnswersAndResultsQueryResult>
{
    public static ICreatableResponse<GetVokiQuestionWithAnswersAndResultsQueryResult> Create(
        GetVokiQuestionWithAnswersAndResultsQueryResult queryRes
    ) => new VokiQuestionFullDataResponse(
        queryRes.Question.Id.ToString(),
        queryRes.Question.Text.ToString(),
        QuestionImageSetResponse.Create(queryRes.Question.ImageSet),
        IQuestionContentPrimitiveDto.FromQuestionContent(queryRes.Question.Content),
        queryRes.Question.ShuffleAnswers,
        queryRes.Question.AnswersCountLimit.MinAnswers,
        queryRes.Question.AnswersCountLimit.MaxAnswers,
        MaxAnswersForQuestionCount: VokiQuestion.MaxAnswersCount,
        queryRes.ResultsIdToName.ToDictionary(
            r => r.Key.ToString(),
            r => r.Value.ToString()
        ),
        AnswerRelatedResultIdsSet.MaxRelatedResultsForAnswerCount
    );
}

internal record QuestionImageSetResponse(double Width, double Height, string[] Keys)
{
    public static QuestionImageSetResponse Create(VokiQuestionImagesSet set) => new(
        set.AspectRatio.Width,
        set.AspectRatio.Height,
        set.Keys.Select(imageKey => imageKey.ToString()).ToArray()
    );
}