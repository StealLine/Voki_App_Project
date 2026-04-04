namespace SharedKernel.errs;

public class ErrOr<T>
{
    private readonly T? _success;
    private readonly Err? _err;
    private readonly bool _isSuccess;

    private ErrOr(T success) {
        _success = success;
        _err = null;
        _isSuccess = true;
    }

    private ErrOr(Err err) {
        _success = default;
        _err = err;
        _isSuccess = false;
    }

    public static ErrOr<T> Success(T value) => new(value);
    public static ErrOr<T> Err(Err err) => new(err);

    public bool IsSuccess() => _isSuccess;
    public bool IsErr() => !_isSuccess;

    public bool IsSuccess(out T value) {
        if (_isSuccess) {
            value = _success!;
            return true;
        }

        value = default!;
        return false;
    }

    public bool IsErr(out Err err) {
        if (!_isSuccess) {
            err = _err!;
            return true;
        }

        err = new Err("No error");
        return false;
    }

    public T AsSuccess() {
        if (_isSuccess) {
            return _success!;
        }

        throw new InvalidOperationException($"No success value available. Error value: {_err}");
    }

    public Err AsErr() {
        if (!_isSuccess) {
            return _err!;
        }

        return new Err("No error");
    }

    public TResult Match<TResult>(Func<T, TResult> successFunc, Func<Err, TResult> errorFunc) =>
        _isSuccess ? successFunc(_success!) : errorFunc(_err!);
    public ErrOr<TOut> Bind<TOut>(Func<T, ErrOr<TOut>> onSuccess) =>
        Match(
            onSuccess,
            err => err
        );

    public static implicit operator ErrOr<T>(T value) => new(value);
    public static implicit operator ErrOr<T>(Err err) => new(err);
}