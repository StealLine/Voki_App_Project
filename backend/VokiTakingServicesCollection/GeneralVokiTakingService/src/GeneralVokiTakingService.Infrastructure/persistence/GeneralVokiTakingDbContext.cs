using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiTakingService.Infrastructure.persistence;

public class GeneralVokiTakingDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public GeneralVokiTakingDbContext(
        DbContextOptions<GeneralVokiTakingDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<GeneralVoki> Vokis { get; init; } = null!;
    public DbSet<GeneralVokiTakenRecord> VokiTakenRecords { get; init; } = null!;
    public DbSet<BaseVokiTakingSession> BaseVokiTakingSessions { get; init; } = null!;
    public DbSet<SessionWithFreeAnswering> VokiTakingSessionsWithFreeAnswering { get; init; } = null!;
    public DbSet<SessionWithSequentialAnswering> VokiTakingSessionsWithSequentialAnswering { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeneralVokiTakingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var domainEvents = ChangeTracker.Entries()
            .Where(e => e.Entity is IAggregateRoot)
            .SelectMany(e => ((IAggregateRoot)e.Entity).PopAndClearDomainEvents())
            .ToList();

        await PublishDomainEvents(domainEvents, cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents, CancellationToken ct) {
        foreach (var domainEvent in domainEvents) {
            await _publisher.Publish(domainEvent, ct);
        }
    }
}