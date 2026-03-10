using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ReportScheduleConfiguration : IEntityTypeConfiguration<ReportSchedule>
{
    public void Configure(EntityTypeBuilder<ReportSchedule> builder)
    {
        builder.ToTable("report_schedules");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasConversion(id => id.Value, value => new UserId(value))
            
            .IsRequired();

        builder.Property(x => x.Source)
            
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Type)
            
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Frequency)
            .HasConversion<string>()
            
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            
            .IsRequired();

        builder.Property(x => x.LastRunAtUtc)
            ;

        builder.Property(x => x.NextRunAtUtc)
            ;

        builder.Property(x => x.EndDateUtc)
            ;

        builder.Property(x => x.IsActive)
            
            .HasDefaultValue(true);

        builder.Property(x => x.DeliveryEmail)
            
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.UserId, x.Source, x.Type, x.IsActive });

        builder.Ignore(x => x.DomainEvents);
    }
}
