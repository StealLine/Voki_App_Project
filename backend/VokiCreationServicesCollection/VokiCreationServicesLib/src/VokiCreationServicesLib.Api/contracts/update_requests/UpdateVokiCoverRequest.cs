using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Api.contracts.update_requests;

public class UpdateVokiCoverRequest : IRequestWithValidationNeeded
{
    public string NewCover { get; init; }

    public ErrOrNothing Validate() {
        ErrOr<TempImageKey> creationRes = TempImageKey.FromString(NewCover);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        ParsedCover = creationRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }


    public TempImageKey ParsedCover { get; private set; }
}