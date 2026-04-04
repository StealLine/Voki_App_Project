using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VokiRatingsService.Application.background_services.commands;

namespace VokiRatingsService.Application.background_services;

public class RatingsSnapshotUpdaterBackgroundService : BackgroundService
{
    private readonly ICommandHandler<UpdateRatingsSnapshotsFromMarkersCommand, UpdateRatingsSnapshotsFromMarkersCommandResult>
        _updateRatingsSnapshotsCommandHandler;

    private readonly ILogger<RatingsSnapshotUpdaterBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(2);

    public RatingsSnapshotUpdaterBackgroundService(
        ICommandHandler<UpdateRatingsSnapshotsFromMarkersCommand, UpdateRatingsSnapshotsFromMarkersCommandResult>
            updateRatingsSnapshotsCommandHandler,
        ILogger<RatingsSnapshotUpdaterBackgroundService> logger
    ) {
        _updateRatingsSnapshotsCommandHandler = updateRatingsSnapshotsCommandHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"Background service '{nameof(RatingsSnapshotUpdaterBackgroundService)}' started");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                ErrOr<UpdateRatingsSnapshotsFromMarkersCommandResult> result = await _updateRatingsSnapshotsCommandHandler.Handle(
                    new UpdateRatingsSnapshotsFromMarkersCommand(),
                    stoppingToken
                );

                if (result.IsErr(out var err)) {
                    _logger.LogWarning("Snapshots updating encountered errors: {Errors}", err.ToString());
                }
                else {
                    UpdateRatingsSnapshotsFromMarkersCommandResult success = result.AsSuccess();
                    _logger.LogInformation(
                        "{ServiceName} cycle finished at {Time}. {TotalMarkersCount} markers found, {NewCreated} new snapshots created, {Updated} snapshots updated, delta: {Delta}",
                        nameof(UpdateRatingsSnapshotsFromMarkersCommand), success.Time,
                        success.MarkersCount, success.NewSnapshotsCount, success.SnapshotsUpdatedCount,
                        success.MarkersCount - success.NewSnapshotsCount - success.SnapshotsUpdatedCount
                    );
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Exception was thrown while updating Voki ratings snapshots");
            }

            try {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        _logger.LogInformation($"Background service '{nameof(RatingsSnapshotUpdaterBackgroundService)}' stopped");
    }
}