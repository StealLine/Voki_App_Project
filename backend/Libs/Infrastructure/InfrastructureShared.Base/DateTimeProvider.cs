using SharedKernel;

namespace InfrastructureShared.Base;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
