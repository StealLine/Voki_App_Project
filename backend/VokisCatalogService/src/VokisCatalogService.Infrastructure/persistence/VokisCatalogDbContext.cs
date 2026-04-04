using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence;

public class VokisCatalogDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public VokisCatalogDbContext(
        DbContextOptions<VokisCatalogDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<Voki> Vokis { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VokisCatalogDbContext).Assembly);
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