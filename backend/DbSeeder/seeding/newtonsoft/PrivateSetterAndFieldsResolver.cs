using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DbSeeder.seeding.newtonsoft;

public class PrivateSetterAndFieldsResolver : DefaultContractResolver
{
    protected override JsonObjectContract CreateObjectContract(Type objectType) {
        JsonObjectContract contract = base.CreateObjectContract(objectType);

        contract.DefaultCreatorNonPublic = false;
        contract.DefaultCreator = () => RuntimeHelpers.GetUninitializedObject(objectType);

        return contract;
    }

    protected override IList<JsonProperty> CreateProperties(Type type,
        MemberSerialization memberSerialization) {
        IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

        IEnumerable<JsonProperty> fields = type
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(f => {
                var jp = base.CreateProperty(f, memberSerialization);
                jp.Readable = true;
                jp.Writable = true;
                return jp;
            });

        props = props.Concat(fields).ToList();
        return props;
    }

    protected override JsonProperty CreateProperty(MemberInfo member,
        MemberSerialization memberSerialization) {
        JsonProperty prop = base.CreateProperty(member, memberSerialization);

        if (!prop.Writable) {
            PropertyInfo? pi = member as PropertyInfo;
            if (pi?.GetSetMethod(true) is not null) {
                prop.Writable = true;
            }
        }

        return prop;
    }
}