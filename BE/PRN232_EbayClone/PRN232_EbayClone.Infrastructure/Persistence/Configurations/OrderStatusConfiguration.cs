using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
       public void Configure(EntityTypeBuilder<OrderStatus> builder)
       {
              builder.ToTable("order_statuses");
              builder.HasKey(s => s.Id);

              builder.Property(s => s.Code).HasMaxLength(50);
              builder.Property(s => s.Name).HasMaxLength(100);
              builder.Property(s => s.Description).HasMaxLength(500);
              builder.Property(s => s.Color).HasMaxLength(20);
              builder.HasIndex(s => s.Code).IsUnique();
              builder.HasMany(o => o.AllowedTransitions)
                     .WithOne(t => t.FromStatus)
                     .HasForeignKey("FromStatusId");
              builder.Navigation(o => o.AllowedTransitions)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.HasData(OrderStatusSeed.Statuses);

       }
}
public class OrderStatusTransitionConfiguration : IEntityTypeConfiguration<OrderStatusTransition>
{
       public void Configure(EntityTypeBuilder<OrderStatusTransition> builder)
       {
              builder.ToTable("order_status_transitions");
              builder.HasKey(t => t.Id);

              builder.HasOne(t => t.FromStatus)
                     .WithMany(o => o.AllowedTransitions)
                     .HasForeignKey("FromStatusId")
                     .IsRequired();


              builder.HasOne(t => t.ToStatus)
                     .WithMany()
                     .HasForeignKey("ToStatusId")
                     .IsRequired();

              var stringListComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                  (c1, c2) => c1!.SequenceEqual(c2!),
                  c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                  c => c.ToList());

              builder.Property(t => t.AllowedRoles)
                     
                     .HasConversion(
                          v => string.Join(',', v),
                          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                     )
                     .IsRequired()
                     .Metadata.SetValueComparer(stringListComparer);

              builder.HasData(OrderStatusSeed.Transitions);
       }
}

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
       public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
       {
              builder.ToTable("order_status_histories");
              builder.HasKey(h => h.Id);

              builder.Property(h => h.Id)
                     
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("gen_random_uuid()");

              builder.HasOne(h => h.FromStatus)
                     .WithMany()
                     .HasForeignKey("FromStatusId");

              builder.HasOne(h => h.ToStatus)
                     .WithMany()
                     .HasForeignKey("ToStatusId");

              builder.Property(h => h.OrderId).IsRequired();

       }
}
