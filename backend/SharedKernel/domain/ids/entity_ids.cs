namespace SharedKernel.domain.ids;

public class AppUserId(Guid value) : GuidBasedId(value);

public class VokiId(Guid value) : GuidBasedId(value)
{
    public static VokiId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralVokiQuestionId(Guid value) : GuidBasedId(value)
{
    public static GeneralVokiQuestionId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralVokiAnswerId(Guid value) : GuidBasedId(value)
{
    public static GeneralVokiAnswerId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralVokiResultId(Guid value) : GuidBasedId(value)
{
    public static GeneralVokiResultId CreateNew() => new(Guid.CreateVersion7());
}

public class VokiAlbumId(Guid value) : GuidBasedId(value)
{
    public static VokiAlbumId CreateNew() => new(Guid.CreateVersion7());
}

public class VokiCommentId(Guid value) : GuidBasedId(value)
{
    public static VokiCommentId CreateNew() => new(Guid.CreateVersion7());
}

public class VokiRatingId(Guid value) : GuidBasedId(value)
{
    public static VokiRatingId CreateNew() => new(Guid.CreateVersion7());
}