namespace VokiRatingsService.Domain.common;

public class VokiRatingsDistribution : ValueObject
{
    public override IEnumerable<object> GetEqualityComponents() =>
        [Rating1Count, Rating2Count, Rating3Count, Rating4Count, Rating5Count];

    public VokiRatingsDistribution(
        uint rating1Count, uint rating2Count, uint rating3Count, uint rating4Count, uint rating5Count
    ) {
        Rating1Count = rating1Count;
        Rating2Count = rating2Count;
        Rating3Count = rating3Count;
        Rating4Count = rating4Count;
        Rating5Count = rating5Count;
        TotalCount = Rating1Count + Rating2Count + Rating3Count + Rating4Count + Rating5Count;
        TotalSum = rating1Count * 1 +
                   rating2Count * 2 +
                   rating3Count * 3 +
                   rating4Count * 4 +
                   rating5Count * 5;
    }

    public uint Rating1Count { get; }
    public uint Rating2Count { get; }
    public uint Rating3Count { get; }
    public uint Rating4Count { get; }
    public uint Rating5Count { get; }
    public uint TotalCount { get; }
    public uint TotalSum { get; }

    public static VokiRatingsDistribution Empty => new(
        rating1Count: 0,
        rating2Count: 0,
        rating3Count: 0,
        rating4Count: 0,
        rating5Count: 0
    );

    public static VokiRatingsDistribution FromRatingsArray(RatingValue[] ratings) {
        if (ratings.Length == 0) {
            return Empty;
        }

        uint r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0;

        foreach (var rating in ratings) {
            switch (rating.Value) {
                case 1:
                    r1++;
                    break;
                case 2:
                    r2++;
                    break;
                case 3:
                    r3++;
                    break;
                case 4:
                    r4++;
                    break;
                case 5:
                    r5++;
                    break;
            }
        }

        return new VokiRatingsDistribution(
            rating1Count: r1,
            rating2Count: r2,
            rating3Count: r3,
            rating4Count: r4,
            rating5Count: r5
        );
    }
}