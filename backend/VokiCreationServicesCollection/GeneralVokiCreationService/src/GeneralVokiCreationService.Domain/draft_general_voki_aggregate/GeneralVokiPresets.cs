using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

internal static class GeneralVokiPresets
{
    private static readonly Random Random = new();

    private static readonly ImmutableArray<string> QuestionTexts = [
        "super mega cool question text >~<",
        "yo drop ur thoughts here fr",
        "default question but make it ✨quirky✨",
        "lowkey not even a real question",
        "type something smart here idk",
        "this one’s gonna be deep trust",
        "placeholder but it kinda slaps",
        "brain.exe not found, insert question",
        "vibe check question goes here",
        "this question is s built different",
        "💀💀💀 actual question coming soon"
    ];

    private static readonly ImmutableArray<string> ResultTexts = [
        "someone who'd bring a spoon to a knife fight",
        "secret ending, congrats",
        "you're like a walking plot twist, no one saw it coming"
    ];

    public static VokiQuestionText GetRandomQuestionText() {
        var randomText = QuestionTexts[Random.Next(QuestionTexts.Length)];
        return VokiQuestionText.Create(randomText).AsSuccess();
    }
    public static VokiResultText GetRandomResultText() {
        var randomText = ResultTexts[Random.Next(ResultTexts.Length)];
        return VokiResultText.Create(randomText).AsSuccess();
    }
}