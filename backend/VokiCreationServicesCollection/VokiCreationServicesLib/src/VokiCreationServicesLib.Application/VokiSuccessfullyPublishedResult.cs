using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using VokimiStorageKeysLib.concrete_keys;

namespace VokiCreationServicesLib.Application;

public sealed record VokiSuccessfullyPublishedResult(
    VokiId Id,
    VokiCoverKey Cover,
    VokiName Name
);