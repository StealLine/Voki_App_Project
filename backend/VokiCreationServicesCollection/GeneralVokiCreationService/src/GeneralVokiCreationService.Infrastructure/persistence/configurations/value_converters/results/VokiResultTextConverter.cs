using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.results;

public class VokiResultTextConverter : ValueConverter<VokiResultText, string>
{
    public VokiResultTextConverter() : base(
        text => text.ToString(),
        value => VokiResultText.Create(value).AsSuccess()
    ) { }
}