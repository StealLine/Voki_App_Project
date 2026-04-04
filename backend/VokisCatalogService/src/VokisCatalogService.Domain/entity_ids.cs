namespace VokisCatalogService.Domain;

public class UserTakenVokisListId(Guid value) : GuidBasedId(value)
{
    public static UserTakenVokisListId CreateNew() => new(Guid.CreateVersion7());
}
