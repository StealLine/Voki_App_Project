using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;
using VokiCommentsService.Domain.app_user_aggregate;

namespace VokiCommentsService.Infrastructure.persistence;

public class VokiCommentsDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public VokiCommentsDbContext(
        DbContextOptions<VokiCommentsDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;

    // public DbSet<Voki> Vokis { get; init; } = null!;
    // public DbSet<VokiComment> Comments { get; init; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VokiCommentsDbContext).Assembly);
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
            await _publisher.Publish(domainEvent,ct);
        }
    }
}