using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public sealed class DraftGeneralVoki : BaseDraftVoki
{
    public const int
        MinQuestionsCount = 2,
        MaxQuestionsCount = 100;

    public const int
        MinResultsCount = 2,
        MaxResultsCount = 60;

    private DraftGeneralVoki() { }
    public VokiTakingProcessSettings TakingProcessSettings { get; private set; }
    public GeneralVokiInteractionSettings InteractionSettings { get; private set; }

    private readonly List<VokiQuestion> _questions;
    private readonly List<VokiResult> _results;

    private DraftGeneralVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, cover,
        creationDate
    ) {
        TakingProcessSettings = VokiTakingProcessSettings.Default;
        InteractionSettings = GeneralVokiInteractionSettings.Default;
        _questions = [];
        _results = [];
    }

    public static DraftGeneralVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) {
        DraftGeneralVoki newGeneralVoki = new(vokiId, primaryAuthorId, name, cover, creationDate);
        newGeneralVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(
            newGeneralVoki.Id,
            newGeneralVoki.PrimaryAuthorId
        ));
        return newGeneralVoki;
    }

    public ErrOrNothing UpdateTakingProcessSettings(AuthenticatedUserCtx aUserCtx, VokiTakingProcessSettings newSettings) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki taking process settings you need to be the Voki author");
        }

        TakingProcessSettings = newSettings;
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateInteractionSettings(AuthenticatedUserCtx aUserCtx, GeneralVokiInteractionSettings newSettings) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki interaction settings you need to be the Voki author");
        }

        InteractionSettings = newSettings;
        return ErrOrNothing.Nothing;
    }

    public ErrOr<ImmutableArray<VokiQuestion>> GetQuestions(AuthenticatedUserCtx aUserCtx) {
        if (HasUserAccess(aUserCtx)) {
            return _questions.ToImmutableArray();
        }

        return ErrFactory.NoAccess("To access Voki questions user must have access to Voki");
    }

    public ErrOr<ImmutableArray<VokiResult>> GetResults(AuthenticatedUserCtx aUserCtx) {
        if (HasUserAccess(aUserCtx)) {
            return _results.ToImmutableArray();
        }

        return ErrFactory.NoAccess("To access Voki results user must have access to Voki");
    }

    public ErrOr<GeneralVokiQuestionId> AddNewQuestion(
        AuthenticatedUserCtx aUserCtx,
        GeneralVokiQuestionContentType contentType
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To modify Voki you must be its author");
        }

        if (_questions.Count >= MaxQuestionsCount) {
            return ErrFactory.LimitExceeded(
                $"General voki cannot have more than {MaxQuestionsCount} questions. Current count: {_questions.Count}"
            );
        }

        ErrOr<VokiQuestionOrder> orderCreationRes = VokiQuestionOrder.Create(_questions.Count + 1);
        if (orderCreationRes.IsErr(out var err)) {
            return err;
        }

        VokiQuestion question = VokiQuestion.CreateNew(orderCreationRes.AsSuccess(), contentType);
        _questions.Add(question);
        return question.Id;
    }

    public ErrOr<VokiQuestion> QuestionWithId(AuthenticatedUserCtx aUserCtx, GeneralVokiQuestionId questionId) {
        if (!this.HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess();
        }

        VokiQuestion? requestedQuestion = _questions.FirstOrDefault(q => q.Id == questionId);
        if (requestedQuestion is null) {
            return ErrFactory.NotFound.Common(
                "This voki doesn't have requested question",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        return requestedQuestion;
    }

    public ErrOr<VokiQuestion> UpdateQuestionText(
        AuthenticatedUserCtx aUserCtx, GeneralVokiQuestionId questionId, VokiQuestionText newText
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To modify Voki question you must be the Voki author");
        }

        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.Common(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        questionToUpdate.UpdateText(newText);
        return questionToUpdate;
    }

    public ErrOr<VokiQuestionImagesSet> UpdateQuestionImages(
        AuthenticatedUserCtx aUserCtx,
        GeneralVokiQuestionId questionId,
        VokiQuestionImagesSet newImageSet
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To modify Voki question you must be the Voki author");
        }


        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        var incorrectKeys = newImageSet.Keys
            .Where(k => !k.IsWithIds(Id, questionId))
            .Select(k => k.ToString())
            .ToArray();
        if (incorrectKeys.Length != 0) {
            return ErrFactory.Conflict(
                "Some of provided images does not belong to specified question",
                $"Voki id: {Id}, question id: {questionId}, incorrect keys: {string.Join(", ", incorrectKeys)}"
            );
        }

        ErrOrNothing res = questionToUpdate.UpdateImages(newImageSet);
        if (res.IsErr(out var err)) {
            return err;
        }

        return questionToUpdate.ImageSet;
    }

    public ErrOr<VokiQuestion> UpdateQuestionAnswerSettings(
        AuthenticatedUserCtx aUserCtx,
        GeneralVokiQuestionId questionId,
        QuestionAnswersCountLimit newCountLimit,
        bool shuffleAnswers
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To modify Voki question you must be the Voki author");
        }

        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        var res = questionToUpdate.UpdateAnswerSettings(newCountLimit, shuffleAnswers);
        if (res.IsErr(out var err)) {
            return err;
        }

        return questionToUpdate;
    }

    public ErrOr<BaseQuestionTypeSpecificContent> UpdateQuestionTypeSpecificContent(
        AuthenticatedUserCtx aUserCtx, GeneralVokiQuestionId questionId, BaseQuestionTypeSpecificContent content
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To modify Voki question you must be the Voki author");
        }

        VokiQuestion? question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.VokiContent(
                "Cannot add update question type specific content because question doesn't exist"
            );
        }

        if (
            CheckIfAnswerDataBelongs(questionId, content).IsErr(out var err)
            || CheckIfAllRelatedResultsExist(content).IsErr(out err)
        ) {
            return err;
        }

        question.UpdateContent(content);
        return question.Content;
    }

    private ErrOrNothing CheckIfAnswerDataBelongs(GeneralVokiQuestionId questionId, BaseQuestionTypeSpecificContent content) {
        if (content is IContentWithStorageKeys key && !key.IsAllForCorrectVokiQuestion(Id, questionId)) {
            return ErrFactory.Conflict("Certain answer data does not belong to this question");
        }

        return ErrOrNothing.Nothing;
    }

    private ErrOrNothing CheckIfAllRelatedResultsExist(BaseQuestionTypeSpecificContent content) {
        ImmutableHashSet<GeneralVokiResultId> mentionedResultIds = content.BaseAnswers
            .SelectMany(a => a.RelatedResultIds.ToArray())
            .ToImmutableHashSet();
        var existingResults = _results.Select(r => r.Id).ToHashSet();
        var incorrectRelatedResultIds = mentionedResultIds
            .Where(r => !existingResults.Contains(r))
            .Select(id => id.ToString())
            .ToArray();
        if (incorrectRelatedResultIds.Length > 0) {
            return ErrFactory.Conflict(
                "Some of the provided results specified as related does not exist in this Voki",
                $"Voki id: {Id}, incorrect result ids: {string.Join(", ", incorrectRelatedResultIds)}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOr<ImmutableArray<VokiQuestion>> MoveQuestionUpInOrder(
        AuthenticatedUserCtx aUserCtx,
        GeneralVokiQuestionId questionId
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To move question up you must have access to the Voki");
        }

        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find question to move up",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        if (question.OrderInVoki.IsFirst()) {
            return _questions.ToImmutableArray();
        }

        VokiQuestion? neighbor = _questions.FirstOrDefault(q => q.OrderInVoki.Value == question.OrderInVoki.Value - 1);
        if (neighbor is null) {
            return ErrFactory.Conflict(
                "Cannot move question up",
                $"No neighbor with order {question.OrderInVoki.Value - 1} found. Voki id: {Id}, question id: {questionId}"
            );
        }

        ErrOrNothing moveUpRes = question.MoveOrderUp();
        if (moveUpRes.IsErr(out var err)) {
            return err;
        }

        ErrOrNothing moveDownRes = neighbor.MoveOrderDown();
        if (moveDownRes.IsErr(out err)) {
            return err;
        }

        return _questions.ToImmutableArray();
    }

    public ErrOr<ImmutableArray<VokiQuestion>> MoveQuestionDownInOrder(AuthenticatedUserCtx aUserCtx,
        GeneralVokiQuestionId questionId) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To move question down you must have access to the Voki");
        }

        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find question to move down",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        if (question.OrderInVoki.Value == _questions.Count) {
            return _questions.ToImmutableArray();
        }

        VokiQuestion? neighbor = _questions.FirstOrDefault(q => q.OrderInVoki.Value == question.OrderInVoki.Value + 1);
        if (neighbor is null) {
            return ErrFactory.Conflict(
                "Cannot move question down",
                $"No neighbor with order {question.OrderInVoki.Value + 1} found. Voki id: {Id}, question id: {questionId}"
            );
        }

        ErrOrNothing moveDownRes = question.MoveOrderDown();
        if (moveDownRes.IsErr(out var err)) {
            return err;
        }

        ErrOrNothing moveUpRes = neighbor.MoveOrderUp();
        if (moveUpRes.IsErr(out err)) {
            return err;
        }

        return _questions.ToImmutableArray();
    }

    public ErrOr<ImmutableArray<VokiQuestion>> DeleteQuestion(AuthenticatedUserCtx aUserCtx, GeneralVokiQuestionId questionId) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To delete question you must have access to the Voki");
        }

        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find question to delete",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        VokiQuestionOrder order = question.OrderInVoki;
        _questions.Remove(question);

        foreach (var q in _questions.Where(q => q.OrderInVoki.Value > order.Value)) {
            ErrOrNothing moveUpRes = q.MoveOrderUp();
            if (moveUpRes.IsErr(out var err)) {
                return err.WithMessagePrefix("Cannot delete question due to moving other question order error: ");
            }
        }

        return _questions.ToImmutableArray();
    }


    public ErrOr<ImmutableArray<VokiResult>> AddNewResult(
        AuthenticatedUserCtx aUserCtx, VokiResultName resultName, DateTime now
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To add result you must have access to the Voki");
        }

        if (_results.Count >= MaxResultsCount) {
            return ErrFactory.LimitExceeded(
                $"General voki cannot have more than {MaxResultsCount} results. Current count: {_results.Count}"
            );
        }

        HashSet<VokiResultName> existingNames = _results.Select(r => r.Name).ToHashSet();
        if (existingNames.Contains(resultName)) {
            return ErrFactory.Conflict("Result with this name already exists in this Voki. Result name must be unique");
        }

        VokiResult result = VokiResult.CreateNew(resultName, now);
        _results.Add(result);
        return _results.ToImmutableArray();
    }

    public ErrOr<VokiResult> UpdateResult(
        AuthenticatedUserCtx aUserCtx,
        GeneralVokiResultId resultId,
        VokiResultName newName,
        VokiResultText newText,
        GeneralVokiResultImageKey? newImage
    ) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki result you need to be the author of this Voki");
        }

        VokiResult? resultToUpdate = _results.FirstOrDefault(q => q.Id == resultId);
        if (resultToUpdate is null) {
            return ErrFactory.NotFound.VokiContent(
                "Could not find result to update",
                $"Voki with id {Id} doesn't have a result with id {resultId}"
            );
        }

        if (newImage is not null && !newImage.IsWithIds(Id, resultId)) {
            return ErrFactory.Conflict(
                "Provided image does not belong to the specified result",
                $"Voki id: {Id}, result id: {resultId}, key: {newImage}"
            );
        }

        var updatingRes = resultToUpdate.Update(newName, newText, newImage);
        if (updatingRes.IsErr(out var err)) {
            return err;
        }

        return resultToUpdate;
    }


    public ErrOrNothing DeleteResult(AuthenticatedUserCtx aUserCtx, GeneralVokiResultId resultId) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To delete Voki result you need to be the author of this Voki");
        }

        VokiResult? result = _results.FirstOrDefault(q => q.Id == resultId);
        if (result is null) {
            return ErrOrNothing.Nothing;
        }

        foreach (var q in _questions) {
            q.RemoveRelatedResultInAnswers(resultId);
        }

        _results.Remove(result);
        return ErrOrNothing.Nothing;
    }

    private List<VokiPublishingIssue> CheckQuestionsForPublishingIssues() {
        if (_questions.Count < MinQuestionsCount) {
            return [
                VokiPublishingIssue.Problem(
                    message: $"Too few questions ({_questions.Count}). Minimum required is {MinQuestionsCount}",
                    source: "Questions",
                    fixRecommendation: $"Add at least {MinQuestionsCount - _questions.Count} more question(s)"
                )
            ];
        }

        if (_questions.Count > MaxQuestionsCount) {
            return [
                VokiPublishingIssue.Problem(
                    message: $"Too many questions ({_questions.Count}). Maximum allowed is {MaxQuestionsCount}",
                    source: "Questions",
                    fixRecommendation: $"Remove {_questions.Count - MaxQuestionsCount} question(s) to meet the limit"
                )
            ];
        }

        int questionsWithNoResultCount = _questions
            .Count(q =>
                q.Content.BaseAnswers.Count(a => a.RelatedResultIds.Count == 0) >= q.AnswersCountLimit.MinAnswers
            );
        if (questionsWithNoResultCount >= _questions.Count) {
            return [
                VokiPublishingIssue.Problem(
                    message: "Voki can be taken in the way that no answers leading to any result was chosen",
                    source: "Questions",
                    fixRecommendation:
                    "Add related results to more answers, increase questions' minimal answers count limit or remove some answers with no related results"
                )
            ];
        }

        return _questions.SelectMany(q => q.CheckForPublishingIssues()).ToList();
    }

    private List<VokiPublishingIssue> CheckResultsForPublishingIssues() {
        List<VokiPublishingIssue> issues = [];

        if (_results.Count < MinResultsCount) {
            issues.Add(
                VokiPublishingIssue.Problem(
                    message: $"Too few results ({_results.Count}). Minimum required is {MinResultsCount}",
                    source: "Results",
                    fixRecommendation: $"Add at least {MinResultsCount - _results.Count} more result(s)"
                )
            );
        }

        if (_results.Count > MaxResultsCount) {
            issues.Add(
                VokiPublishingIssue.Problem(
                    message: $"Too many results ({_results.Count}). Maximum allowed is {MaxResultsCount}",
                    source: "Results",
                    fixRecommendation: $"Remove {_results.Count - MaxResultsCount} result(s) to meet the limit"
                )
            );
        }

        int resultImages = _results.Count(r => r.Image is not null);
        if (resultImages > 0 && resultImages < _results.Count) {
            issues.Add(
                VokiPublishingIssue.Warning(
                    message: "Some results have images while others do not",
                    source: "Results",
                    fixRecommendation:
                    "Consider adding images to all results or removing them from all to keep presentation consistent"
                )
            );
        }

        HashSet<GeneralVokiResultId> resultsAnswersLeadTo = _questions
            .SelectMany(q => q.Content.BaseAnswers.SelectMany(a => a.RelatedResultIds.ToArray()))
            .ToHashSet();

        foreach (var result in _results) {
            if (!resultsAnswersLeadTo.Contains(result.Id)) {
                issues.Add(
                    VokiPublishingIssue.Problem(
                        message: $"Result \"{result.Name}\" is not linked to any answer",
                        source: "Results",
                        fixRecommendation: "Make this result related to at least one answer or remove it"
                    )
                );
            }
        }

        return issues;
    }


    public ErrOr<ImmutableArray<VokiPublishingIssue>> GatherAllPublishingIssues(AuthenticatedUserCtx aUserCtx) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("Too see Voki publishing issues you need to have access to this Voki");
        }

        return ErrOr<ImmutableArray<VokiPublishingIssue>>.Success([
            ..base.CheckCoverForPublishingIssues(),
            ..base.CheckTagsForPublishingIssues(),
            ..base.CheckDetailsForPublishingIssues(),
            ..CheckQuestionsForPublishingIssues(),
            ..CheckResultsForPublishingIssues()
        ]);
    }

    public ErrOrNothing PublishWithWarningsIgnored(
        AuthenticatedUserCtx aUserCtx, DateTime now,
        ISet<AppUserId> confirmedCoAuthorIds, ISet<AppUserId> confirmedManagerIds
    ) {
        if (aUserCtx.UserId != this.PrimaryAuthorId) {
            return ErrFactory.NoAccess("Only primary author can publish Voki");
        }

        if (CheckConfirmedCoAuthorsAndManagers(confirmedCoAuthorIds, confirmedManagerIds).IsErr(out var err)) {
            return err;
        }

        ErrOr<ImmutableArray<VokiPublishingIssue>> issuesRes = GatherAllPublishingIssues(aUserCtx);
        if (issuesRes.IsErr(out err)) {
            return err;
        }

        var issues = issuesRes.AsSuccess();
        bool anyProblems = issues.Any(i => i.Type == PublishingIssueType.Problem);
        if (anyProblems) {
            return ErrFactory.Conflict("Cannot publish Voki because of an unresolved problem");
        }

        AddVokiPublishedDomainEvent(now);
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing PublishWithNoIssues(
        AuthenticatedUserCtx aUserCtx, DateTime now,
        ISet<AppUserId> confirmedCoAuthorIds, ISet<AppUserId> confirmedManagerIds
    ) {
        if (aUserCtx.UserId != this.PrimaryAuthorId) {
            return ErrFactory.NoAccess("Only primary author can publish Voki");
        }


        if (CheckConfirmedCoAuthorsAndManagers(confirmedCoAuthorIds, confirmedManagerIds).IsErr(out var err)) {
            return err;
        }

        ErrOr<ImmutableArray<VokiPublishingIssue>> issuesRes = GatherAllPublishingIssues(aUserCtx);
        if (issuesRes.IsErr(out err)) {
            return err;
        }

        var issues = issuesRes.AsSuccess();
        if (issues.Length > 0) {
            return ErrFactory.Conflict(
                "Could not publish voki because new publishing issues were found. Please check them"
            );
        }

        AddVokiPublishedDomainEvent(now);
        return ErrOrNothing.Nothing;
    }

    private void AddVokiPublishedDomainEvent(DateTime utcNow) {
        QuestionDomainEventDto ParseQuestionToDto(VokiQuestion q) {
            return new QuestionDomainEventDto(
                q.Id, q.Text, q.ImageSet, q.OrderInVoki,
                q.ShuffleAnswers, q.AnswersCountLimit, q.Content.ToIntegrationEventDto(), HasAnyAudio: q.HasAudio()
            );
        }

        AddDomainEvent(new GeneralVokiPublishedEvent(
            Id, PrimaryAuthorId, CoAuthors, UserIdsToBecomeManagers,
            Name, Cover, Details, Tags,
            InitializingDate: CreationDate,
            PublicationDate: utcNow,
            TakingProcessSettings, InteractionSettings,
            _questions.Select(ParseQuestionToDto).ToArray(),
            _results.Select(r => new ResultDomainEventDto(r.Id, r.Name, r.Text, r.Image)).ToArray()
        ));
    }
}