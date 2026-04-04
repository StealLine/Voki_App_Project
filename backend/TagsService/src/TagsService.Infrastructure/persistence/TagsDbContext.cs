using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;
using TagsService.Domain.voki_tag_aggregate;

namespace TagsService.Infrastructure.persistence;

public class TagsDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public TagsDbContext(
        DbContextOptions<TagsDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<VokiTag> VokiTags { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TagsDbContext).Assembly);
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