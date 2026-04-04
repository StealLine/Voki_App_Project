using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Domain.common.dtos;

public record VokiResultWithDistributionPercent(VokiResult Result, double DistributionPercent);
