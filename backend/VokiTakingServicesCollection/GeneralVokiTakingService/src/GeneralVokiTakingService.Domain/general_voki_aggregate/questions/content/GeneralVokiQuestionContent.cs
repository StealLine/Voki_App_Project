using System.Runtime.CompilerServices;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using VokimiStorageKeysLib.base_keys;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;

public abstract partial record GeneralVokiQuestionContent
{
    public abstract IEnumerable<GeneralVokiAnswerId> AnswerIds { get; }
    public abstract ImmutableDictionary<GeneralVokiAnswerId, BaseQuestionAnswer> AnswerByIds { get; }
    public abstract IEnumerable<BaseStorageKey> GatherContentKeys();

    public TResult Match<TResult>(
        Func<TextOnly, TResult> textOnly,
        Func<ImageOnly, TResult> imageOnly,
        Func<ImageAndText, TResult> imageAndText,
        Func<ColorOnly, TResult> colorOnly,
        Func<ColorAndText, TResult> colorAndText,
        Func<AudioOnly, TResult> audioOnly,
        Func<AudioAndText, TResult> audioAndText
    ) => this switch {
        TextOnly typed => textOnly(typed),
        ImageOnly typed => imageOnly(typed),
        ImageAndText typed => imageAndText(typed),
        ColorOnly typed => colorOnly(typed),
        ColorAndText typed => colorAndText(typed),
        AudioOnly typed => audioOnly(typed),
        AudioAndText typed => audioAndText(typed),
        _ => throw new SwitchExpressionException(this)
    };
}