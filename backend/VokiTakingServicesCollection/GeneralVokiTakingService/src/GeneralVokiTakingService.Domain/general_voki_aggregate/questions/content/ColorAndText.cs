using System.Collections.Immutable;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using VokimiStorageKeysLib.base_keys;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;

public abstract partial record GeneralVokiQuestionContent
{
    public sealed record ColorAndText(
        ImmutableArray<BaseQuestionAnswer.ColorAndText> Answers
    ) : GeneralVokiQuestionContent
    {
        public override IEnumerable<GeneralVokiAnswerId> AnswerIds => Answers.Select(a => a.Id);

        public override ImmutableDictionary<GeneralVokiAnswerId, BaseQuestionAnswer> AnswerByIds => Answers.ToImmutableDictionary(
            a => a.Id,
            a => a as BaseQuestionAnswer
        );

        public override IEnumerable<BaseStorageKey> GatherContentKeys() => [];
    }
}
