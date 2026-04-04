using System.Text.RegularExpressions;

namespace SharedKernel.common;

public class HexColor : ValueObject
{
    private static readonly Regex HexColorRegex = new("^#[a-fA-F0-9]{6}$");
    private string Value { get; }

    public HexColor(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckHexColorForErr(value));
        Value = value;
    }

    public static ErrOr<HexColor> Create(string color) {
        color = color.Trim();

        if (CheckHexColorForErr(color).IsErr(out var err)) {
            return err;
        }

        return new HexColor(color);
    }

    public static ErrOrNothing CheckHexColorForErr(string color) {
        if (string.IsNullOrWhiteSpace(color)) {
            return ErrFactory.NoValue.Common("No value provided for color");
        }

        if (!HexColorRegex.IsMatch(color)) {
            return ErrFactory.IncorrectFormat($"Invalid hex color format: '{color}'");
        }

        return ErrOrNothing.Nothing;
    }

    public static HexColor Default => new HexColor("#b0b0b0");
    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value;
}