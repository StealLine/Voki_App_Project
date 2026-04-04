using AlbumsService.Infrastructure.persistence;
using AuthService.Infrastructure.persistence;
using CoreVokiCreationService.Infrastructure.persistence;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TagsService.Infrastructure.persistence;
using UserProfilesService.Infrastructure.persistence;
using VokiCommentsService.Infrastructure.persistence;
using VokiRatingsService.Infrastructure.persistence;
using VokisCatalogService.Infrastructure.persistence;

namespace DbSeeder;

public static class DbContextsCollection
{
    public static AuthDbContext Auth(IConfiguration config) => new(
        DbOptions<AuthDbContext>(config, "AuthServiceDb"), FakePublisher.Instance
    );

    public static TagsDbContext Tags(IConfiguration config) => new(
        DbOptions<TagsDbContext>(config, "TagsServiceDb"), FakePublisher.Instance
    );

    public static UserProfilesDbContext UserProfiles(IConfiguration config) => new(
        DbOptions<UserProfilesDbContext>(config, "UserProfilesServiceDb"), FakePublisher.Instance
    );

    public static CoreVokiCreationDbContext CoreVokiCreation(IConfiguration config) => new(
        DbOptions<CoreVokiCreationDbContext>(config, "CoreVokiCreationServiceDb"), FakePublisher.Instance
    );

    public static GeneralVokiCreationDbContext GeneralVokiCreation(IConfiguration config) => new(
        DbOptions<GeneralVokiCreationDbContext>(config, "GeneralVokiCreationServiceDb"), FakePublisher.Instance
    );

    public static VokisCatalogDbContext VokisCatalog(IConfiguration config) => new(
        DbOptions<VokisCatalogDbContext>(config, "VokisCatalogServiceDb"), FakePublisher.Instance
    );

    public static GeneralVokiTakingDbContext GeneralVokiTaking(IConfiguration config) => new(
        DbOptions<GeneralVokiTakingDbContext>(config, "GeneralVokiTakingServiceDb"), FakePublisher.Instance
    );

    public static VokiRatingsDbContext VokiRatings(IConfiguration config) => new(
        DbOptions<VokiRatingsDbContext>(config, "VokiRatingsServiceDb"), FakePublisher.Instance
    );

    public static VokiCommentsDbContext VokiComments(IConfiguration config) => new(
        DbOptions<VokiCommentsDbContext>(config, "VokiCommentsServiceDb"), FakePublisher.Instance
    );

    public static AlbumsDbContext Albums(IConfiguration config) => new(
        DbOptions<AlbumsDbContext>(config, "AlbumsServiceDb"), FakePublisher.Instance
    );

    static DbContextOptions<T> DbOptions<T>(IConfiguration config, string str) where T : DbContext {
        string? connectionStr = config.GetConnectionString(str);
        if (connectionStr is null) {
            throw new NullReferenceException($"Connection string '{str}' is not provided");
        }

        DbContextOptionsBuilder<T> optionsBuilder = new();
        return optionsBuilder.UseNpgsql(connectionStr).Options;
    }
}