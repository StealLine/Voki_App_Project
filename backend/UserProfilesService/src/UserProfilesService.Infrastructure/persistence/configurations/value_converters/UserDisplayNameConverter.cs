using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

public class UserDisplayNameConverter : ValueConverter<UserDisplayName, string>
{
    public UserDisplayNameConverter() : base(
        id => id.ToString(),
        value => UserDisplayName.Create(value).AsSuccess()
    ) { }
}

