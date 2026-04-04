using AuthService.Application.background_services.commands;
using AuthService.Application.unconfirmed_users.commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.background_services;

public class UnconfirmedUsersCleanupBackgroundService : BackgroundService
{
    private readonly ICommandHandler<DeleteExpiredUnconfirmedUsersCommand, int> _deleteExpiredCommandHandler;
    private readonly ILogger<UnconfirmedUsersCleanupBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

    public UnconfirmedUsersCleanupBackgroundService(
        ICommandHandler<DeleteExpiredUnconfirmedUsersCommand, int> deleteExpiredCommandHandler,
        ILogger<UnconfirmedUsersCleanupBackgroundService> logger
    ) {
        _deleteExpiredCommandHandler = deleteExpiredCommandHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"Background service '{nameof(UnconfirmedUsersCleanupBackgroundService)}' started");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                var result = await _deleteExpiredCommandHandler.Handle(
                    new DeleteExpiredUnconfirmedUsersCommand(),
                    stoppingToken
                );

                if (result.IsErr(out var err)) {
                    _logger.LogWarning("Cleanup encountered errors: {Errors}", err.ToString());
                }
                else {
                    _logger.LogInformation(
                        "Deleted {Count} expired unconfirmed users at {Time}",
                        result.AsSuccess(), DateTime.UtcNow
                    );
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Exception was thrown while deleting expired unconfirmed users");
            }

            try {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        _logger.LogInformation($"Background service '{nameof(UnconfirmedUsersCleanupBackgroundService)}' stopped");

    }
}