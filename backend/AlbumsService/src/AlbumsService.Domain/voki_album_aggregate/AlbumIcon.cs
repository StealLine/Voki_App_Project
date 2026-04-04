using SharedKernel.exceptions;

namespace AlbumsService.Domain.voki_album_aggregate;

public sealed class AlbumIcon : ValueObject
{
    private readonly string _value;

    public AlbumIcon(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static ErrOr<AlbumIcon> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new AlbumIcon(value);

    public static ErrOrNothing CheckForErr(string icon) {
        if (string.IsNullOrWhiteSpace(icon)) {
            return ErrFactory.NoValue.Common("Album icon cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;

        foreach (char c in icon) {
            if (!IsValidCharacter(c)) {
                errs.AddNext(ErrFactory.IncorrectFormat(
                    $"Invalid character in album icon: '{c}'",
                    $"Album icon can only contain lowercase letters, digits and hyphens. Invalid character: {(int)c}"
                ));
                break;
            }
        }

        return errs;
    }

    private static bool IsValidCharacter(char c) =>
        (c >= 'a' && c <= 'z') ||
        (c >= '0' && c <= '9') ||
        (c == '-');
}