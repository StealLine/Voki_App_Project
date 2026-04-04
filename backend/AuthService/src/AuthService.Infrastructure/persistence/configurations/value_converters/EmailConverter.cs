using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthService.Infrastructure.persistence.configurations.value_converters;

public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() : base(
        email => email.ToString(),
        value => Email.Create(value).AsSuccess()
    ) { }
}