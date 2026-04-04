using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.errs.utils;
using SharedKernel.exceptions;
using VokimiStorageKeysLib.base_keys;

namespace VokiCreationServicesLib.Infrastructure.persistence.value_converters;

public class StorageKeyConverter<T> : ValueConverter<T, string> where T : BaseStorageKey
{
    public StorageKeyConverter() : base(
        id => id.ToString(),
        value => StringToKey(value)
    ) { }

    private static T StringToKey(string value) {
        T? key = (T?)Activator.CreateInstance(typeof(T), value);
        if (key is null) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not parse {nameof(T)} from {value} in the {nameof(Guid)}"
            ));
        }

        return key!;
    }
}