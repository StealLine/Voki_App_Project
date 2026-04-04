using System.Text;

namespace SharedKernel.errs;

public class Err
{
    public string Message { get; init; }
    public ushort Code { get; init; }
    public string? Details { get; init; }
    public Err? Next { get; private set; }

    public Err(
        string message,
        ushort code = ErrCodes.Unspecified,
        string? details = null
    ) {
        Message = message;
        Code = code;
        Details = details;
    }

    public Err WithMessagePrefix(string prefix) => new Err(
        message: $"{prefix} {Message}",
        code: Code,
        details: Details
    );

    public void AddNext(Err next) {
        var current = this;
        while (current.Next != null) {
            current = current.Next;
        }

        current.Next = next;
    }

    public Err WithNext(Err next) {
        AddNext(next);
        return this;
    }

    public void AddNextIfErr<T>(ErrOr<T> errOr) {
        if (errOr.IsErr(out var err)) {
            AddNext(err);
        }
    }

    public Err WithNextIfErr<T>(ErrOr<T> errOr) {
        if (errOr.IsErr(out var err)) {
            AddNext(err);
        }

        return this;
    }

    public override string ToString() {
        var sb = new StringBuilder();
        var current = this;
        int index = 1;

        while (current is not null) {
            sb.AppendLine($"[Error {index}]");
            sb.AppendLine($"Code: {current.Code}");
            sb.AppendLine($"Message: {current.Message}");

            if (!string.IsNullOrEmpty(current.Details)) {
                sb.AppendLine($"Details: {current.Details}");
            }

            sb.AppendLine();

            current = current.Next;
            index++;
        }

        return sb.ToString();
    }

    public string ToStringWithField(string fieldName, object? fieldValue) {
        var sb = new StringBuilder();
        var current = this;
        int index = 1;

        while (current is not null) {
            sb.AppendLine($"[Error {index}]");
            sb.AppendLine($"Code: {current.Code}");
            sb.AppendLine($"Message: {current.Message}");

            if (!string.IsNullOrEmpty(current.Details)) {
                sb.AppendLine($"Details: {current.Details}");
            }

            sb.AppendLine($"{fieldName}: {fieldValue?.ToString() ?? "null"}");
            sb.AppendLine();

            current = current.Next;
            index++;
        }

        return sb.ToString();
    }

    public string ToStringWithFields(params (string Name, object? Value)[] fields) {
        var sb = new StringBuilder();
        var current = this;
        int index = 1;

        while (current is not null) {
            sb.AppendLine($"[Error {index}]");
            sb.AppendLine($"Code: {current.Code}");
            sb.AppendLine($"Message: {current.Message}");

            if (!string.IsNullOrEmpty(current.Details)) {
                sb.AppendLine($"Details: {current.Details}");
            }

            foreach (var (name, value) in fields) {
                sb.AppendLine($"{name}: {value?.ToString() ?? "null"}");
            }

            sb.AppendLine();

            current = current.Next;
            index++;
        }

        return sb.ToString();
    }
}