using AlbumsService.Domain.voki_album_aggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlbumsService.Infrastructure.persistence.configurations.value_converters;

public class AlbumNameConverter : ValueConverter<AlbumName, string>
{
    public AlbumNameConverter() : base(
        id => id.ToString(),
        value => AlbumName.Create(value).AsSuccess()
    ) { }
}