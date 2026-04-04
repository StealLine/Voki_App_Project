using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserLinkArrayConverter : ValueConverter<ImmutableArray<UserLink>, string[]>
{
    private const char Divider = '^';

    private static string[] Serialize(ImmutableArray<UserLink> links) =>
        links.Select(l => $"{l.Type}{Divider}{l.Value}").ToArray();

    private static ImmutableArray<UserLink> Deserialize(string[] array) =>
        array.Select(str => {
            var parts = str.Split(Divider, 2);
            return UserLink.Create(parts[1], Enum.Parse<UserLinkType>(parts[0], true)).AsSuccess();
        }).ToImmutableArray();

    public UserLinkArrayConverter() : base(
        links => Serialize(links),
        array => Deserialize(array)
    ) { }
}

internal sealed class UserLinkArrayComparer : ValueComparer<ImmutableArray<UserLink>>
{
    private static bool _seqEqual(ImmutableArray<UserLink> a, ImmutableArray<UserLink> b) => a.SequenceEqual(b);

    public UserLinkArrayComparer() : base(
        (a, b) => _seqEqual(a, b),
        v => v.Aggregate(0, (hash, x) => HashCode.Combine(hash, x.Value, x.Type)),
        v => v.ToImmutableArray()
    ) { }
}