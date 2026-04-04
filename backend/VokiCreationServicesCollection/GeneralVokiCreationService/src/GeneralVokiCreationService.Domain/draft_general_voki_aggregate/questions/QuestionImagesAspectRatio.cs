using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public abstract class VokiQuestionImagesAspectRatio
{
    public abstract double Width { get; }
    public abstract double Height { get; }

    public static ErrOr<VokiQuestionImagesAspectRatio> Create(double width, double height) => (width, height) switch {
        (16, 9) => VokiQuestionImagesAspectRatioPresets.Create16To9,
        (4, 3) => VokiQuestionImagesAspectRatioPresets.Create4To3,
        (3, 4) => VokiQuestionImagesAspectRatioPresets.Create3To4,
        (9, 16) => VokiQuestionImagesAspectRatioPresets.Create9To16,
        _ => VokiQuestionImagesAspectRatioCustom.CreateCustom(width, height)
            .Bind<VokiQuestionImagesAspectRatio>(custom => custom)
    };

    public double GetRatio() => Math.Round(Width / Height, 4, MidpointRounding.AwayFromZero);

    public static VokiQuestionImagesAspectRatio Default =>
        VokiQuestionImagesAspectRatioCustom.CreateCustom(1, 1).AsSuccess();

    private sealed class VokiQuestionImagesAspectRatioPresets : VokiQuestionImagesAspectRatio
    {
        private VokiQuestionImagesAspectRatioPresets(double width, double height) {
            Width = width;
            Height = height;
        }

        public override double Width { get; }
        public override double Height { get; }

        internal static VokiQuestionImagesAspectRatioPresets Create16To9 => new(16, 9);
        internal static VokiQuestionImagesAspectRatioPresets Create4To3 => new(4, 3);
        internal static VokiQuestionImagesAspectRatioPresets Create3To4 => new(3, 4);
        internal static VokiQuestionImagesAspectRatioPresets Create9To16 => new(9, 16);
    }

    private sealed class VokiQuestionImagesAspectRatioCustom : VokiQuestionImagesAspectRatio
    {
        private const double MinCustomImageAspectRatio = 1, MaxCustomImageAspectRatio = 3;

        private VokiQuestionImagesAspectRatioCustom(double width, double height) {
            InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(width, height));
            Width = Math.Round(width, 3, MidpointRounding.AwayFromZero);
            Height = Math.Round(height, 3, MidpointRounding.AwayFromZero);
        }

        public override double Width { get; }
        public override double Height { get; }

        private static ErrOrNothing CheckForErr(double width, double height) => (width, height) switch {
            var (w, _) when w < MinCustomImageAspectRatio => ErrFactory.ValueOutOfRange(
                $"Width must be >= {MinCustomImageAspectRatio}"),
            var (w, _) when w > MaxCustomImageAspectRatio => ErrFactory.ValueOutOfRange(
                $"Width must be <= {MaxCustomImageAspectRatio}"),
            var (_, h) when h < MinCustomImageAspectRatio => ErrFactory.ValueOutOfRange(
                $"Height must be >= {MinCustomImageAspectRatio}"),
            var (_, h) when h > MaxCustomImageAspectRatio => ErrFactory.ValueOutOfRange(
                $"Height must be <= {MaxCustomImageAspectRatio}"),
            _ => ErrOrNothing.Nothing
        };


        internal static ErrOr<VokiQuestionImagesAspectRatioCustom> CreateCustom(double width, double height) =>
            CheckForErr(width, height).IsErr(out var err)
                ? err
                : new VokiQuestionImagesAspectRatioCustom(width, height);
    }
}