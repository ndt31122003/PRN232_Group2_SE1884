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
            
            .IsRequired();

        builder.Property(x => x.ReferenceCode)
            
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Source)
            
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Type)
            
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.RequestedAtUtc)
            
            .IsRequired();

        builder.Property(x => x.CompletedAtUtc)
            ;

        builder.Property(x => x.FileUrl)
            
            .HasMaxLength(512);

        builder.OwnsOne(x => x.DateRange, range =>
        {
            range.Property(r => r.StartUtc)
                
                .IsRequired();

            range.Property(r => r.EndUtc)
                
                .IsRequired();

            range.Property(r => r.TimeZone)
                
                .HasMaxLength(64);
        });

        builder.HasIndex(x => new { x.UserId, x.ReferenceCode })
            .IsUnique();

        builder.Ignore(x => x.DomainEvents);
    }
}
