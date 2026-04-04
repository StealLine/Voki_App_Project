using SharedKernel.domain.ids;

namespace VokiTakingServicesLib.Domain.common;

public class VokiTakenRecordId(Guid value) : GuidBasedId(value)
{
    public static VokiTakenRecordId CreateNew() => new(Guid.CreateVersion7());
}