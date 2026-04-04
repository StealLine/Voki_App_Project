using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.app_users;

namespace InfrastructureShared.EfCore.value_converters;

public class UserUniqueNameConverter : ValueConverter<UserUniqueName, string>
{
    public UserUniqueNameConverter() : base(
        id => id.Value,
        value => UserUniqueName.Create(value).AsSuccess()
    ) { }
}

