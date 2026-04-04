namespace SharedKernel.exceptions;

using System.Runtime.CompilerServices;

public class UnexpectedBehaviourException : Exception
{
    public Err Err { get; }
    public string Caller { get; }
    public string? UserMessage { get; }

    private UnexpectedBehaviourException(Err err, string caller, string? userMessage)
        : base(err.Message) {
        Err = err;
        Caller = caller;
        UserMessage = userMessage;
    }

    public static void ThrowIfErr(
        ErrOrNothing possibleErr,
        string? userMessage = null,
        [CallerMemberName] string caller = ""
    ) {
        if (possibleErr.IsErr(out var err)) {
            throw new UnexpectedBehaviourException(err, caller, userMessage);
        }
    }
    public static void ThrowIfErr<T>(
        ErrOr<T> possibleErr,
        string? userMessage = null,
        [CallerMemberName] string caller = ""
    ) {
        if (possibleErr.IsErr(out var err)) {
            throw new UnexpectedBehaviourException(err, caller, userMessage);
        }
    }
    public static void ThrowErr(
        Err err,
        string? userMessage = null,
        [CallerMemberName] string caller = ""
    ) {
        throw new UnexpectedBehaviourException(err, caller, userMessage);
    }
}