using SharedKernel.common;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public record class VokiDetails(
    VokiDescription Description,
    bool HasMatureContent,
    Language Language
)
{
    public static VokiDetails Default => new(VokiDescription.Empty, false, Language.Other);
}