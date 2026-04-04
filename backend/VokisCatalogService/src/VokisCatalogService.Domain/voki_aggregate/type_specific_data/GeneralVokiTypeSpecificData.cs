namespace VokisCatalogService.Domain.voki_aggregate.type_specific_data;

public class GeneralVokiTypeSpecificData : BaseVokiTypeSpecificData
{
    private GeneralVokiTypeSpecificData() { }

    public ushort QuestionsCount { get; }
    public ushort ResultsCount { get; }
    public bool AnyAudios { get; }
    public bool ForceSequentialAnswering { get; }
    public bool ShuffleQuestions { get; }

    public GeneralVokiTypeSpecificData(
        ushort questionsCount,
        ushort resultsCount,
        bool anyAudios,
        bool forceSequentialAnswering,
        bool shuffleQuestions
    )
    {
        QuestionsCount = questionsCount;
        ResultsCount = resultsCount;
        AnyAudios = anyAudios;
        ForceSequentialAnswering = forceSequentialAnswering;
        ShuffleQuestions = shuffleQuestions;
    }


    public override IEnumerable<object> GetEqualityComponents() => [
        QuestionsCount,
        ResultsCount,
        AnyAudios,
        ForceSequentialAnswering,
        ShuffleQuestions
    ];
}