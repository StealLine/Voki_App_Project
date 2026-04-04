using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionImagesRequest : IRequestWithValidationNeeded
{
    public string[] NewImages { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }

    public ErrOrNothing Validate() {
        if (VokiQuestionImagesSet.CheckForErr(NewImages.Length).IsErr(out var err)) {
            return err;
        }

        var aspectRatioCreationRes = VokiQuestionImagesAspectRatio.Create(Width, Height);
        if (aspectRatioCreationRes.IsErr(out err)) {
            return err;
        }

        ParsedAspectRatio = aspectRatioCreationRes.AsSuccess();

        ErrOrNothing parsingErrs = ErrOrNothing.Nothing;

        ErrOr<TempImageKey>[] possibleTemp = NewImages
            .Where(TempImageKey.IsPossiblySuitable)
            .Select(TempImageKey.FromString)
            .ToArray();
        foreach (var possibleErr in possibleTemp) {
            parsingErrs.AddNextIfErr(possibleErr);
        }

        var possibleSaved = NewImages
            .Where(k => !TempImageKey.IsPossiblySuitable(k))
            .Select(GeneralVokiQuestionImageKey.FromString)
            .ToArray();
        foreach (var possibleErr in possibleSaved) {
            parsingErrs.AddNextIfErr(possibleErr);
        }

        if (parsingErrs.IsErr(out err)) {
            return err;
        }

        ParsedTempKeys = possibleTemp.Select(t => t.AsSuccess()).ToHashSet();
        ParsedSavedKeys = possibleSaved.Select(t => t.AsSuccess()).ToHashSet();
        return parsingErrs;
    }

    public HashSet<TempImageKey> ParsedTempKeys { get; private set; }
    public HashSet<GeneralVokiQuestionImageKey> ParsedSavedKeys { get; private set; }
    public VokiQuestionImagesAspectRatio ParsedAspectRatio { get; private set; }
}