using AuthService.Domain.app_user_aggregate;
using AuthService.Domain.unconfirmed_user_aggregate;
using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence;

public class AuthDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public AuthDbContext(DbContextOptions<AuthDbContext> options, IDomainEventsPublisher publisher)
        : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<UnconfirmedUser> UnconfirmedUsers { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
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