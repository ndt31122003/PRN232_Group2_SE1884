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
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.Source)
            .HasColumnName("source")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Frequency)
            .HasConversion<string>()
            .HasColumnName("frequency")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.LastRunAtUtc)
            .HasColumnName("last_run_at_utc");

        builder.Property(x => x.NextRunAtUtc)
            .HasColumnName("next_run_at_utc");

        builder.Property(x => x.EndDateUtc)
            .HasColumnName("end_date_utc");

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(x => x.DeliveryEmail)
            .HasColumnName("delivery_email")
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.UserId, x.Source, x.Type, x.IsActive });

        builder.Ignore(x => x.DomainEvents);
    }
}
