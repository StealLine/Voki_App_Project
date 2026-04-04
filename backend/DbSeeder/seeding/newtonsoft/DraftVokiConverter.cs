using System.Collections.Immutable;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;

namespace DbSeeder.seeding.newtonsoft;

public class DraftVokiConverter : JsonConverter<DraftVoki>
{
    public override DraftVoki ReadJson(
        JsonReader reader,
        Type objectType,
        DraftVoki? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    ) {
        JObject jo = JObject.Load(reader);
        var obj = (DraftVoki)JsonUtil.CreateUninitialized(typeof(DraftVoki));

        if (jo.TryGetValue("name", out var jName)) {
            JsonUtil.SetProperty(obj, "Name", new VokiName(jName.Value<string>()!));
        }

        if (jo.TryGetValue("type", out var jType)) {
            JsonUtil.SetProperty(
                obj,
                "Type",
                Enum.Parse<VokiType>(jType.Value<string>()!, ignoreCase: true)
            );
        }

        if (jo.TryGetValue("coAuthorIds", out var jCoAuthors)) {
            var arr = jCoAuthors.ToObject<List<string>>()!;
            JsonUtil.SetProperty(
                obj,
                "CoAuthorIds",
                arr.Select(s => new AppUserId(Guid.Parse(s))).ToImmutableHashSet()
            );
        }

        if (jo.TryGetValue("invitedForCoAuthorUserIds", out var jInv)) {
            var arr = jInv.ToObject<List<string>>()!;
            JsonUtil.SetProperty(
                obj,
                "InvitedForCoAuthorUserIds",
                arr.Select(s => new AppUserId(Guid.Parse(s))).ToImmutableHashSet()
            );
        }

        return obj;
    }

    public override void WriteJson(JsonWriter writer, DraftVoki? value, JsonSerializer serializer) =>
        throw new NotImplementedException();
}
