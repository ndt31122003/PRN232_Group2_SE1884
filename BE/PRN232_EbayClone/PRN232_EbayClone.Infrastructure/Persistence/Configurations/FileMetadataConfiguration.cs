using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.FileMetadata.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class FileMetadataConfiguration : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder
            .ToTable("file_metadata");

        builder
            .HasKey(fm => fm.Id);

        builder
            .Property(fm => fm.Id)
            .ValueGeneratedNever();

        builder
            .HasQueryFilter(fm => !fm.IsDeleted);
    }
}
