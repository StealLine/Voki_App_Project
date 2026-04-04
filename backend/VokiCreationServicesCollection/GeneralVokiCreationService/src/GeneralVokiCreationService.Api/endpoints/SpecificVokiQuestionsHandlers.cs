using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.contracts.questions.update_question;
using GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Application.draft_vokis.queries.questions;
using GeneralVokiCreationService.Application.dtos;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

namespace GeneralVokiCreationService.Api.endpoints;

internal class SpecificVokiQuestionsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/questions/{questionId}/");

        group.MapGet("/", GetVokiQuestionFullData);
        group.MapPatch("/update-text", UpdateQuestionText)
            .WithRequestValidation<UpdateQuestionTextRequest>();
        group.MapPatch("/update-images", UpdateQuestionImages)
            .WithRequestValidation<UpdateQuestionImagesRequest>();
        group.MapPatch("/update-answer-settings", UpdateQuestionAnswerSettings)
            .WithRequestValidation<UpdateQuestionAnswerSettingsRequest>();
        group.MapPatch("/update-content", UpdateQuestionContent)
            .WithRequestValidation<IUpdateQuestionContentRequest>();
        group.MapPatch("/move-up-in-order", MoveQuestionUpInOrder);
        group.MapPatch("/move-down-in-order", MoveQuestionDownInOrder);
        group.MapDelete("/delete", DeleteVokiQuestion);

        return group;
    }

    private static async Task<IResult> GetVokiQuestionFullData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuestionWithResultsDataQuery, GetVokiQuestionWithAnswersAndResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        GetVokiQuestionWithResultsDataQuery dataQuery = new(vokiId, questionId);
        var result = await handler.Handle(dataQuery, ct);

        return CustomResults.FromErrOrToJson<
            GetVokiQuestionWithAnswersAndResultsQueryResult,
            VokiQuestionFullDataResponse
        >(result);
    }

    private static async Task<IResult> UpdateQuestionText(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionTextCommand, VokiQuestionText> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionTextRequest>();

        UpdateQuestionTextCommand command = new(id, questionId, request.ParsedText);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (text) => Results.Json(
            new { NewText = text.ToString() }
        ));
    }

    private static async Task<IResult> UpdateQuestionImages(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionImageSetCommand, VokiQuestionImagesSet> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionImagesRequest>();

        UpdateQuestionImageSetCommand command = new(
            id, questionId, request.ParsedTempKeys, request.ParsedSavedKeys, request.ParsedAspectRatio
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (imgsSet) => Results.Json(new {
            NewKeys = ImmutableArrayExtensions.Select(imgsSet.Keys, s => s.ToString()).ToArray(),
            NewWidth = imgsSet.AspectRatio.Width,
            NewHeight = imgsSet.AspectRatio.Height
        }));
    }

    private static async Task<IResult> UpdateQuestionAnswerSettings(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionAnswerSettingsCommand, VokiQuestion> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionAnswerSettingsRequest>();

        UpdateQuestionAnswerSettingsCommand command = new(
            id, questionId, request.ParsedAnswersCountLimit, request.ShuffleAnswers
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (question) => Results.Json(new {
            MinAnswers = question.AnswersCountLimit.MinAnswers,
            MaxAnswers = question.AnswersCountLimit.MaxAnswers,
            ShuffleAnswers = question.ShuffleAnswers
        }));
    }

    private static async Task<IResult> UpdateQuestionContent(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionContentCommand, BaseQuestionTypeSpecificContent> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<IUpdateQuestionContentRequest>();

        UpdateQuestionContentCommand command = new(id, questionId, request.ValidatedContent);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result,
            content => Results.Json(IQuestionContentPrimitiveDto.FromQuestionContent(content))
        );
    }

    private static async Task<IResult> MoveQuestionUpInOrder(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<MoveQuestionUpInOrderCommand, ImmutableArray<VokiQuestion>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        MoveQuestionUpInOrderCommand command = new(id, questionId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questions) => Results.Json(
            ListAllQuestionsBriefDataResponse.Create(questions)
        ));
    }

    private static async Task<IResult> MoveQuestionDownInOrder(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<MoveQuestionDownInOrderCommand, ImmutableArray<VokiQuestion>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        MoveQuestionDownInOrderCommand command = new(id, questionId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questions) => Results.Json(
            ListAllQuestionsBriefDataResponse.Create(questions)
        ));
    }

    private static async Task<IResult> DeleteVokiQuestion(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<DeleteVokiQuestionCommand, ImmutableArray<VokiQuestion>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        DeleteVokiQuestionCommand command = new(id, questionId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questions) => Results.Json(
            ListAllQuestionsBriefDataResponse.Create(questions)
        ));
    }
}