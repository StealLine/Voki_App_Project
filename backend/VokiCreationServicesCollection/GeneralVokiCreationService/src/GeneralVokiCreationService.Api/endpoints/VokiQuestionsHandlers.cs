using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.queries.questions;

namespace GeneralVokiCreationService.Api.endpoints;

internal class VokiQuestionsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/questions/");

        group.MapGet("/overview", GetVokiQuestionsOverview);
        group.MapPost("/add-new", AddNewQuestionToVoki)
            .WithRequestValidation<AddNewQuestionToVokiRequest>();

        return group;
    }

    private static async Task<IResult> GetVokiQuestionsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuestionsOverviewQuery, GetVokiQuestionsOverviewQueryResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuestionsOverviewQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<GetVokiQuestionsOverviewQueryResult, VokiQuestionsOverviewResponse>(result);
    }

    private static async Task<IResult> AddNewQuestionToVoki(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewQuestionToVokiRequest>();

        AddNewQuestionToVokiCommand command = new(id, request.QuestionContentType);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questionId) => Results.Json(
            new { Id = questionId.ToString() }
        ));
    }
}