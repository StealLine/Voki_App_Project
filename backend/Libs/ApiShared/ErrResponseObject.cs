namespace ApiShared;

public record ErrResponseObject(ErrDto[] Errs);

public record ErrDto(
    string Message,
    ushort Code,
    string? Details
)
{
    public static ErrDto FromErr(Err err) => new(
        err.Message,
        err.Code,
        err.Details
    );
}