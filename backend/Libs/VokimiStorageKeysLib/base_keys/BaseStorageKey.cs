using SharedKernel.domain;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.base_keys;

public abstract class BaseStorageKey : ValueObject
{
    protected abstract string Value { get; }
    public abstract IFileExtension Extension { get; }
    public sealed override IEnumerable<object> GetEqualityComponents() => [Value];
    public sealed override string ToString() => Value;
}