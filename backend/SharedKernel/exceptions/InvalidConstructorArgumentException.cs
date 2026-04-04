namespace SharedKernel.exceptions;

public class InvalidConstructorArgumentException : Exception
{
    public Err Err { get; }
    public string Caller { get; }

    private InvalidConstructorArgumentException(Err err, string caller) : base(err.Message) {
        Err = err;
        Caller = caller;
    }

    public static void ThrowIfErr(object caller, ErrOrNothing possibleErr) {
        if (possibleErr.IsErr(out var err)) {
            throw new InvalidConstructorArgumentException(err, caller.GetType().Name);
        }
    }

    public static void ThrowIfErr<T>(object caller, ErrOr<T> possibleErr) {
        if (possibleErr.IsErr(out var err)) {
            throw new InvalidConstructorArgumentException(err, caller.GetType().Name);
        }
    }

    public static void ThrowErr(
        object caller, Err err
    ) => throw new InvalidConstructorArgumentException(err, caller.GetType().Name);
}