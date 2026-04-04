using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Infrastructure.persistence.value_converters;

public class VokiDescriptionConverter : ValueConverter<VokiDescription, string>
{
    public VokiDescriptionConverter() : base(
        id => id.ToString(),
        value => VokiDescription.Create(value).AsSuccess()
    ) { }
}