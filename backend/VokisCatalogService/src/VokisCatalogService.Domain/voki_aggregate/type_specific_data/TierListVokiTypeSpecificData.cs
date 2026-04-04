namespace VokisCatalogService.Domain.voki_aggregate.type_specific_data;

public class TierListVokiTypeSpecificData : BaseVokiTypeSpecificData
{
    private TierListVokiTypeSpecificData() { }
    public override IEnumerable<object> GetEqualityComponents() => [];
}
