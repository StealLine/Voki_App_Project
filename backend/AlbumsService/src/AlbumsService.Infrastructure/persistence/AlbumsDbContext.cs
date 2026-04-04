using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;

namespace AlbumsService.Infrastructure.persistence;

public class AlbumsDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public AlbumsDbContext(
        DbContextOptions<AlbumsDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<VokiAlbum> VokiAlbums { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlbumsDbContext).Assembly);
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