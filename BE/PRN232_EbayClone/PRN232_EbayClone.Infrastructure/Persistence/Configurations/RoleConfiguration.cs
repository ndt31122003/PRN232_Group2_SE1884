using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Roles.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .ToTable("role");

        builder
            .HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new RoleId(value))
            .ValueGeneratedNever();

        builder
            .Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsMany(r => r.Permissions, rp =>
        {
            rp.ToTable("role_permissions");

            rp.WithOwner().HasForeignKey("RoleId");

            rp.Property(p => p.Permission)
              .HasConversion(new EnumToStringConverter<Permission>())
              
              .IsRequired();

            rp.HasKey("RoleId", "Permission");
        });

        builder
            .HasQueryFilter(r => !r.IsDeleted);
    }
}
