using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ReportDownloadConfiguration : IEntityTypeConfiguration<ReportDownload>
{
    public void Configure(EntityTypeBuilder<ReportDownload> builder)
    {
        builder.ToTable("report_downloads");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.ReferenceCode)
            .HasColumnName("reference_code")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Source)
            .HasColumnName("source")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.RequestedAtUtc)
            .HasColumnName("requested_at_utc")
            .IsRequired();

        builder.Property(x => x.CompletedAtUtc)
            .HasColumnName("completed_at_utc");

        builder.Property(x => x.FileUrl)
            .HasColumnName("file_url")
            .HasMaxLength(512);

        builder.OwnsOne(x => x.DateRange, range =>
        {
            range.Property(r => r.StartUtc)
                .HasColumnName("range_start_utc");

            range.Property(r => r.EndUtc)
                .HasColumnName("range_end_utc");

            range.Property(r => r.TimeZone)
                .HasColumnName("range_timezone")
                .HasMaxLength(64);
        });

        builder.HasIndex(x => new { x.UserId, x.ReferenceCode })
            .IsUnique();

        builder.Ignore(x => x.DomainEvents);
    }
}
