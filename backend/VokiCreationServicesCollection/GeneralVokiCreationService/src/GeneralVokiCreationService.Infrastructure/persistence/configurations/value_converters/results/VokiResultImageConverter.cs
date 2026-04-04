using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.results;

public class VokiResultImageConverter : ValueConverter<GeneralVokiResultImageKey?, string?>
{
    public VokiResultImageConverter() : base(
        img => ToString(img),
        str =>FromString(str)
    ) { }
    private static string? ToString(GeneralVokiResultImageKey? image) =>
        image?.ToString();
    private static GeneralVokiResultImageKey? FromString(string? str) => 
        str is null ? null : new GeneralVokiResultImageKey(str);
}