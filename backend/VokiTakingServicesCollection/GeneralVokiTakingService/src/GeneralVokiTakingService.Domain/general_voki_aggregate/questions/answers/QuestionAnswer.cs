using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;

public abstract record BaseQuestionAnswer(
    GeneralVokiAnswerId Id,
    ushort Order,
    ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
)
{
    public sealed record TextOnly(
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record ImageOnly(
        GeneralVokiAnswerImageKey Image,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record ImageAndText(
        GeneralVokiAnswerImageKey Image,
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record ColorOnly(
        HexColor Color,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record ColorAndText(
        HexColor Color,
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record AudioOnly(
        GeneralVokiAnswerAudioKey Audio,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);

    public sealed record AudioAndText(
        GeneralVokiAnswerAudioKey Audio,
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerId Id,
        ushort Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Id, Order, RelatedResultIds);
}
