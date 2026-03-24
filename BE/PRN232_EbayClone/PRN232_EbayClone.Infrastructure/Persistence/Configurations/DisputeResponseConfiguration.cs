using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class DisputeResponseConfiguration : IEntityTypeConfiguration<DisputeResponse>
{
    public void Configure(EntityTypeBuilder<DisputeResponse> builder)
    {
        builder.ToTable("dispute_response");

        builder.HasKey(dr => dr.Id);

        builder.Property(dr => dr.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(dr => dr.DisputeId)
            .HasColumnName("dispute_id")
            .IsRequired();

        builder.Property(dr => dr.ResponderId)
            .HasColumnName("responder_id")
            .IsRequired();

        builder.Property(dr => dr.Message)
            .HasColumnName("message")
            .IsRequired();

        builder.Property(dr => dr.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(dr => dr.Dispute)
            .WithMany(d => d.Responses)
            .HasForeignKey(dr => dr.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(dr => dr.DisputeId)
            .HasDatabaseName("idx_dispute_response_dispute_id");

        builder.HasIndex(dr => dr.ResponderId)
            .HasDatabaseName("idx_dispute_response_responder_id");

        builder.HasIndex(dr => dr.CreatedAt)
            .HasDatabaseName("idx_dispute_response_created_at");
    }
}
