namespace SharedKernel.errs.utils;

public static class ErrFactory
{
    // Common 
    public static Err Unspecified(string msg = "Unspecified error", string? details = null) =>
        new(msg, ErrCodes.Unspecified, details);
    public static Err ProgramBug(string msg = "Program bug", string? details = null) =>
        new(msg, ErrCodes.ProgramBug, details);
    public static Err NotImplemented(string msg = "Not implemented", string? details = null) =>
        new(msg, ErrCodes.NotImplemented, details);

    #region Validation (1x_xxx)

    public static class NoValue
    {
        public static Err Common(string msg = "Missing required value", string? details = null) =>
            new(msg, ErrCodes.NoValue.Common, details);

        public static Err AppUserId(string msg = "Missing AppUser Id", string? details = null) =>
            new(msg, ErrCodes.NoValue.AppUserId, details);

        public static Err VokiId(string msg = "Missing Voki Id", string? details = null) =>
            new(msg, ErrCodes.NoValue.VokiId, details);

        public static Err GeneralVokiId(string msg = "Missing General Voki Id", string? details = null) =>
            new(msg, ErrCodes.NoValue.GeneralVokiId, details);
    }

    public static Err IncorrectFormat(string msg = "Incorrect format", string? details = null) =>
        new(msg, ErrCodes.IncorrectFormat, details);

    public static Err ValueOutOfRange(string msg = "Value is out of allowed range", string? details = null) =>
        new(msg, ErrCodes.ValueOutOfRange, details);

    #endregion

    #region Business Logic (2x_xxx)

    public static Err Conflict(string msg = "Conflict occurred", string? details = null) =>
        new(msg, ErrCodes.Conflict, details);

    public static Err LimitExceeded(string msg = "Limit exceeded", string? details = null) =>
        new(msg, ErrCodes.LimitExceeded, details);

    public static class NotFound
    {
        public static Err Common(string msg = "Not found", string? details = null) =>
            new(msg, ErrCodes.NotFound.Common, details);

        public static Err User(string msg = "User not found", string? details = null) =>
            new(msg, ErrCodes.NotFound.User, details);

        public static Err Voki(string msg = "Voki not found", string? details = null) =>
            new(msg, ErrCodes.NotFound.Voki, details);

        public static Err VokiContent(string msg = "Voki content not found", string? details = null) =>
            new(msg, ErrCodes.NotFound.VokiContent, details);

        public static Err GeneralVoki(string msg = "General Voki not found", string? details = null) =>
            new(msg, ErrCodes.NotFound.GeneralVoki, details);
    }

    #endregion

    #region Access (3x_xxx)

    public static Err NoAccess(string msg = "Access is denied", string? details = null) =>
        new(msg, ErrCodes.NoAccess, details);

    public static Err AuthRequired(string msg = "Authentication required", string? details = null) =>
        new(msg, ErrCodes.AuthRequired, details);

    #endregion
}