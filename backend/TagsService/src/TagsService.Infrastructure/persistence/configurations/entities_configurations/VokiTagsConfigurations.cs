using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagsService.Domain.voki_tag_aggregate;
using TagsService.Infrastructure.persistence.configurations.value_converters;

namespace TagsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiTagsConfigurations : IEntityTypeConfiguration<VokiTag>
{
    public void Configure(EntityTypeBuilder<VokiTag> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion<VokiTagIdConverter>();
    }
}