using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public class VokiQuestionTextConverter : ValueConverter<VokiQuestionText, string>
{
    public VokiQuestionTextConverter() : base(
        text => text.ToString(),
        value => VokiQuestionText.Create(value).AsSuccess()
    ) { }
}