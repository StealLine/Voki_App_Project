using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.results;

public class VokiResultNameConverter : ValueConverter<VokiResultName, string>
{
    public VokiResultNameConverter() : base(
        name => name.ToString(),
        value => VokiResultName.Create(value).AsSuccess()
    ) { }
}