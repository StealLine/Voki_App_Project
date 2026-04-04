using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.base_keys;

public abstract class BaseStorageAudioKey: BaseStorageKey
{
    public abstract AudioFileExtension AudioExtension { get; }
    public sealed override IFileExtension Extension => AudioExtension;

}