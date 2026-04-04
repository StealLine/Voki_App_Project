using CoreVokiCreationService.Domain.draft_voki_aggregate;
using CoreVokiCreationService.Infrastructure.persistence;
using DbSeeder.seeding.newtonsoft;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedKernel.domain.ids;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.extension;

namespace DbSeeder;

internal abstract class Program
{
	public static async Task<int> Main(string[] args)
	{
		CancellationToken ct = new CancellationToken();

		IConfiguration appSettingsConfig = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false)
			.AddEnvironmentVariables()
			.Build();

		Dictionary<string, Func<IConfiguration, CancellationToken, Task>> actions = new()
		{
			["clear"] = ClearAllDbs,
			["add_draft_voki"] = AddDraftVokiFromJson,
			["exit"] = (_, _) => {
				Console.WriteLine("Program exit...");
				return Task.CompletedTask;
			}
		};

		// Якщо передано аргумент - використати його, інакше питати інтерактивно
		string action;
		if (args.Length > 0)
		{
			action = args[0];
			Console.WriteLine($"Running action: {action}");
		}
		else
		{
			string actionKeys = string.Join(", ", actions.Keys);
			Console.WriteLine($"Select action: ({actionKeys})");
			action = Console.ReadLine()!;
		}

		await actions[action].Invoke(appSettingsConfig, ct);
		return 0;
	}


	static async Task AddDraftVokiFromJson(IConfiguration config, CancellationToken ct) {
        Console.WriteLine("Input path to file: ");
        string path = Console.ReadLine()!;
        string jsonString = await File.ReadAllTextAsync(path, ct);
        Console.WriteLine("Input voki author id:");
        string idStr = Console.ReadLine()!;
        AppUserId authorId = new AppUserId(new(idStr));
        var (vokiCore, vokiGen) = CreateVokiFromJson(jsonString, authorId);

        GeneralVokiCreationDbContext generalVokiCreationDb = DbContextsCollection.GeneralVokiCreation(config);
        CoreVokiCreationDbContext coreVokiCreationDb = DbContextsCollection.CoreVokiCreation(config);

        await generalVokiCreationDb.Database.EnsureCreatedAsync(ct);
        await generalVokiCreationDb.Database.BeginTransactionAsync(ct);
        await coreVokiCreationDb.Database.EnsureCreatedAsync(ct);
        await coreVokiCreationDb.Database.BeginTransactionAsync(ct);
        try {
            CoreVokiCreationService.Domain.app_user_aggregate.AppUser? author =
                await coreVokiCreationDb.AppUsers.FirstOrDefaultAsync(u => u.Id == authorId, ct);
            if (author is null) {
                throw new Exception($"User with id {authorId} not found");
            }

            author.AddInitializedVoki(vokiGen.Id);
            coreVokiCreationDb.AppUsers.Update(author);
            await coreVokiCreationDb.Vokis.AddAsync(vokiCore, ct);
            await coreVokiCreationDb.SaveChangesAsync(ct);


            await generalVokiCreationDb.Vokis.AddAsync(vokiGen, ct);
            await generalVokiCreationDb.SaveChangesAsync(ct);

            await generalVokiCreationDb.Database.CommitTransactionAsync(ct);
            await coreVokiCreationDb.Database.CommitTransactionAsync(ct);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            await generalVokiCreationDb.Database.RollbackTransactionAsync(ct);
            await coreVokiCreationDb.Database.RollbackTransactionAsync(ct);
        }
    }

    private static (DraftVoki vokiCore, DraftGeneralVoki vokiGen) CreateVokiFromJson(
        string jsonString,
        AppUserId authorId
    ) {
        string filledJson = VokiJsonPlaceholderFiller.FillPlaceholders(jsonString);

        var settings = new JsonSerializerSettings {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include
        };

        settings.Converters.Add(new DraftVokiConverter());
        // settings.Converters.Add(new DraftGeneralVokiConverter());

        DraftGeneralVoki vokiGen = JsonConvert.DeserializeObject<DraftGeneralVoki>(filledJson, settings)!;
        DraftVoki vokiCore = JsonConvert.DeserializeObject<DraftVoki>(filledJson, settings)!;

        VokiId id = new VokiId(Guid.CreateVersion7());
        DateTime nowUtc = DateTime.UtcNow;
        var cover = VokiCoverKey.CreateWithId(id, ImageFileExtension.Jpg);


        JsonUtil.SetProperty(vokiGen, "Id", id);
        JsonUtil.SetProperty(vokiCore, "Id", id);

        JsonUtil.SetProperty(vokiGen, "Cover", cover);
        JsonUtil.SetProperty(vokiCore, "Cover", cover);

        JsonUtil.SetProperty(vokiGen, "PrimaryAuthorId", authorId);
        JsonUtil.SetProperty(vokiCore, "PrimaryAuthorId", authorId);

        JsonUtil.SetProperty(vokiGen, "CreationDate", nowUtc);
        JsonUtil.SetProperty(vokiCore, "CreationDate", nowUtc);


        // foreach (var q in vokiGen.Questions) {
        //     JsonUtil.SetProperty(q, "Id", GeneralVokiQuestionId.CreateNew());
        //
        //     foreach (var a in q.Answers) {
        //         JsonUtil.SetProperty(a, "Id", GeneralVokiAnswerId.CreateNew());
        //     }
        // }
        //
        // foreach (var r in vokiGen.Results) {
        //     JsonUtil.SetProperty(r, "CreationDate", DateTime.UtcNow);
        // }

        return (vokiCore, vokiGen);
    }


    static async Task ClearAllDbs(IConfiguration config, CancellationToken ct) {
        DbContext[] dbs = [
            DbContextsCollection.Auth(config),
            DbContextsCollection.Tags(config),
            DbContextsCollection.UserProfiles(config),
            DbContextsCollection.CoreVokiCreation(config),
            DbContextsCollection.GeneralVokiCreation(config),
            DbContextsCollection.VokisCatalog(config),
            DbContextsCollection.GeneralVokiTaking(config),
            DbContextsCollection.VokiRatings(config),
            DbContextsCollection.VokiComments(config),
            DbContextsCollection.Albums(config),
        ];
        foreach (var db in dbs) {
            Console.Write($"Clearing {db.GetType().Name}: ");
            await db.Database.EnsureDeletedAsync(ct);
            await db.Database.EnsureCreatedAsync(ct);
            Console.WriteLine("Success");
        }
    }
}