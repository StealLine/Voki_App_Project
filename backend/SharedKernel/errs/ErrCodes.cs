namespace SharedKernel.errs;

public static class ErrCodes
{
    public const ushort Unspecified = 00_000;
    public const ushort ProgramBug = 00_001;
    public const ushort NotImplemented = 00_002;

    #region Validation (1x_xxx)

    public static class NoValue
    {
        public const ushort Common = 11_100;
        public const ushort AppUserId = 11_101;
        public const ushort VokiId = 11_110;
        public const ushort GeneralVokiId = 11_111;
    };

    public const ushort IncorrectFormat = 12_000;
    public const ushort ValueOutOfRange = 13_000;

    #endregion

    #region Business Logic (2x_xxx)

    public const ushort Conflict = 21_000;
    public const ushort LimitExceeded = 22_000;

    public static class NotFound
    {
        public const ushort Common = 23_000;
        public const ushort User = 23_001;
        public const ushort Voki = 23_010;
        public const ushort GeneralVoki = 23_011;
        public const ushort VokiContent = 23_020;
    }

    #endregion

    #region Access (3x_xxx)

    public const ushort NoAccess = 31_000;
    public const ushort AuthRequired = 32_000;

    #endregion
}