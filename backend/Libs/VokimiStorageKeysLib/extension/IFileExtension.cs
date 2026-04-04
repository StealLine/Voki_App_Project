namespace VokimiStorageKeysLib.extension;

public interface IFileExtension: IEquatable<IFileExtension>
{
    public string Value {get; }

}