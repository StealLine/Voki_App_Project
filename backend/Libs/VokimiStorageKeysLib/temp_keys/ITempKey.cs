using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.temp_keys;

public interface ITempKey : IEquatable<ITempKey>
{
    public string Value { get; }
    public IFileExtension Extension { get; }
    protected const string TempFolder = "temp";

}