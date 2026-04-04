namespace SharedKernel.common.vokis.general_vokis;

public enum GeneralVokiQuestionContentType
{
    TextOnly,
    ImageOnly,
    ImageAndText,
    ColorOnly,
    ColorAndText,
    AudioOnly,
    AudioAndText
}

public static class GeneralVokiAnswerTypeExtensions
{
    public static T Match<T>(
        this GeneralVokiQuestionContentType type,
        Func<T> textOnly,
        Func<T> imageOnly, Func<T> imageAndText,
        Func<T> colorOnly, Func<T> colorAndText,
        Func<T> audioOnly, Func<T> audioAndText
    ) => type switch {
        GeneralVokiQuestionContentType.TextOnly => textOnly(),
        GeneralVokiQuestionContentType.ImageOnly => imageOnly(),
        GeneralVokiQuestionContentType.ImageAndText => imageAndText(),
        GeneralVokiQuestionContentType.ColorOnly => colorOnly(),
        GeneralVokiQuestionContentType.ColorAndText => colorAndText(),
        GeneralVokiQuestionContentType.AudioOnly => audioOnly(),
        GeneralVokiQuestionContentType.AudioAndText => audioAndText(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}