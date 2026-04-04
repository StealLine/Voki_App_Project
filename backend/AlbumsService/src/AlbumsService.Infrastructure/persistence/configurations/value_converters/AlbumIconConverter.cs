using AlbumsService.Domain.voki_album_aggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlbumsService.Infrastructure.persistence.configurations.value_converters;

public class AlbumIconConverter : ValueConverter<AlbumIcon, string>
{
    public AlbumIconConverter() : base(
        id => id.ToString(),
        value => AlbumIcon.Create(value).AsSuccess()
    ) { }
}