using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Notifications.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notification");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.Type).IsRequired().HasMaxLength(50);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(255);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(1000);
        builder.Property(n => n.ReferenceId);
        builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);
        builder.Property(n => n.CreatedAt).IsRequired();

        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => new { n.UserId, n.IsRead });
    }
}
