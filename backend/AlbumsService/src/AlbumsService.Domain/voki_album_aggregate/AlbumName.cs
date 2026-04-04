using SharedKernel.exceptions;

namespace AlbumsService.Domain.voki_album_aggregate;

public sealed class AlbumName : ValueObject
{
    private readonly string _value;

    public AlbumName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public const int MinLength = 4;
    public const int MaxLength = 300;

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static ErrOr<AlbumName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new AlbumName(value);

    public static ErrOrNothing CheckForErr(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return ErrFactory.NoValue.Common("Album name cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (name.Length < MinLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Album name must be at least {MinLength} characters long",
                $"Provided name is {name.Length} characters"
            ));
        }
        else if (name.Length > MaxLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Album name cannot exceed {MaxLength} characters",
                $"Provided name is {name.Length} characters"
            ));
        }

        if (char.IsWhiteSpace(name[0])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Album name cannot start with a space"));
        }

        if (char.IsWhiteSpace(name[^1])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Album name cannot end with a space"));
        }

        return errs;
    }
}

