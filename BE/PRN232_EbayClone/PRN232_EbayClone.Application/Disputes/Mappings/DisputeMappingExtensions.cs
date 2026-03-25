using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Application.Disputes.Mappings;

internal static class DisputeMappingExtensions
{
    public static DisputeDto ToDto(this Dispute dispute)
    {
        var responses = dispute.Responses?
            .Select(r => new DisputeResponseDto(
                r.Id,
                r.ResponderId,
                string.Empty, // Would need to load from User
                r.Message,
                r.CreatedAt))
            .ToList() ?? new List<DisputeResponseDto>();

        var listingCreatedBy = dispute.Listing?.CreatedBy ?? string.Empty;
        Console.WriteLine($"[DisputeMappingExtensions] Dispute {dispute.Id}: ListingId={dispute.ListingId}, Listing is null? {dispute.Listing == null}, ListingCreatedBy={listingCreatedBy}");

        return new DisputeDto(
            dispute.Id,
            dispute.ListingId,
            listingCreatedBy,
            dispute.RaisedById,
            string.Empty, // Would need to load from User
            string.Empty, // Would need to load from User
            dispute.Reason,
            dispute.Status,
            dispute.CreatedAt,
            responses);
    }
}
