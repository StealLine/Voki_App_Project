using System.Reflection;
using System.Runtime.CompilerServices;

namespace DbSeeder.seeding.newtonsoft;

public static class JsonUtil
{
    public static object CreateUninitialized(Type t) =>
        RuntimeHelpers.GetUninitializedObject(t);

    public static void SetProperty(object obj, string propertyName, object? value) {
        Type? current = obj.GetType();

        while (current is not null) {
            var prop = current.GetProperty(
                propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
            );

            if (prop is not null) {
                if (prop.CanWrite) {
                    prop.SetValue(obj, value);
                    return;
                }
            }

            var field = current.GetField(
                $"<{propertyName}>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
            );

            if (field is not null) {
                field.SetValue(obj, value);
                return;
            }

            current = current.BaseType;
        }

        throw new InvalidOperationException(
            $"Could not find property or backing field '{propertyName}' on type '{obj.GetType()}' or its base types.");
    }
}