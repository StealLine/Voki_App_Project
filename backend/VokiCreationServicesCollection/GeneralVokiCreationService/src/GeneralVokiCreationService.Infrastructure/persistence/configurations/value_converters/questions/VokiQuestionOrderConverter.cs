using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public class VokiQuestionOrderConverter : ValueConverter<VokiQuestionOrder, int>
{
    public VokiQuestionOrderConverter() : base(
        order => order.Value,
        val => VokiQuestionOrder.Create((ushort)val).AsSuccess()
    )
    { }
}
