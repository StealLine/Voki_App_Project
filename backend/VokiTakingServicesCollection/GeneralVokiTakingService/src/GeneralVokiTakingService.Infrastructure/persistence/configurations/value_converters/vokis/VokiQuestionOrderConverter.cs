using GeneralVokiTakingService.Domain.general_voki_aggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.vokis;

public class VokiQuestionOrderConverter : ValueConverter<VokiQuestionOrder, int>
{
    public VokiQuestionOrderConverter() : base(
        order => order.Value,
        val => VokiQuestionOrder.Create((ushort)val).AsSuccess()
    )
    { }
}
