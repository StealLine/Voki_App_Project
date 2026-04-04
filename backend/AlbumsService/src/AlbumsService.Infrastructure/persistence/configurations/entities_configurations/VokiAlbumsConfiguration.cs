using AlbumsService.Domain.voki_album_aggregate;
using AlbumsService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlbumsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiAlbumsConfiguration : IEntityTypeConfiguration<VokiAlbum>
{
    public void Configure(EntityTypeBuilder<VokiAlbum> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property<AppUserId>("OwnerId")
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Name)
            .HasConversion<AlbumNameConverter>();

        builder
            .Property(x => x.Icon)
            .HasConversion<AlbumIconConverter>();

        builder
            .Property(x => x.MainColor)
            .HasConversion<HexColorConverter>();

        builder
            .Property(x => x.SecondaryColor)
            .HasConversion<HexColorConverter>();

        builder.Property(x => x.CreationDate);

        builder
            .Property(x => x.VokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        //indexes

        builder.HasIndex("OwnerId");
    }
}