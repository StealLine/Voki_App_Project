using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Api.contracts;

public class RateVokiRequest : IRequestWithValidationNeeded
{
    public ushort RatingValue { get; init; }

    public ErrOrNothing Validate() {
        ErrOr<RatingValue> creationRes = VokiRatingsService.Domain.common.RatingValue.Create(RatingValue);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        ParsedRating = creationRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public RatingValue ParsedRating { get; private set; }
}