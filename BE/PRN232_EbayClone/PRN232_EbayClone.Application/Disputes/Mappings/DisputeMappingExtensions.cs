using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Application.Disputes.Mappings;

internal static class DisputeMappingExtensions
{
    public static DisputeDto ToDto(this Dispute dispute)
    {
        // Note: RaisedByUsername and RaisedByFullName would need to be loaded from User table
        return new DisputeDto(
            dispute.Id,
            dispute.ListingId,
            dispute.RaisedById,
            string.Empty, // Would need to load from User
            string.Empty, // Would need to load from User
            dispute.Reason,
            dispute.Status,
            dispute.CreatedAt);
    }
}
