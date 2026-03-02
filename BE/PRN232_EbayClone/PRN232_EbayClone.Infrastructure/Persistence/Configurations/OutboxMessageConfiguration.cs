using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Infrastructure.Outbox;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .ToTable("outbox_message");
    }
}
