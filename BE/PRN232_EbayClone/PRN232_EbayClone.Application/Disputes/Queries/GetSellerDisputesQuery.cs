using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetSellerDisputesQuery(
    string SellerId,
    SellerDisputeFilterDto Filter
) : IQuery<PagedResult<SellerDisputeDto>>;

internal sealed class GetSellerDisputesQueryHandler(
    IDisputeRepository disputeRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
    IFileMetadataRepository fileMetadataRepository,
    IDisputeStateMachine stateMachine)
    : IQueryHandler<GetSellerDisputesQuery, PagedResult<SellerDisputeDto>>
{
    public async Task<Result<PagedResult<SellerDisputeDto>>> Handle(
        GetSellerDisputesQuery request,
        CancellationToken cancellationToken)
    {
        // Get seller's listings
        var sellerListings = await listingRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);
        var listingIds = sellerListings.Select(l => l.Id).ToList();

        if (!listingIds.Any())
        {
            return Result.Success(new PagedResult<SellerDisputeDto>(
                new List<SellerDisputeDto>(),
                request.Filter.Page,
                request.Filter.PageSize,
                0
            ));
        }

        // Get all disputes for seller's listings
        var allDisputes = new List<Domain.Disputes.Entities.Dispute>();
        foreach (var listingId in listingIds)
        {
            var disputesForListing = await disputeRepository.GetDisputesByListingIdAsync(listingId, cancellationToken);
            allDisputes.AddRange(disputesForListing);
        }

        // Apply status filter
        if (!string.IsNullOrEmpty(request.Filter.Status))
        {
            allDisputes = allDisputes.Where(d => d.Status == request.Filter.Status).ToList();
        }

        var totalCount = allDisputes.Count;

        var disputes = allDisputes
            .OrderByDescending(d => d.CreatedAt)
            .Skip((request.Filter.Page - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToList();

        var disputeDtos = new List<SellerDisputeDto>();

        foreach (var dispute in disputes)
        {
            var listing = sellerListings.First(l => l.Id == dispute.ListingId);
            var buyer = await userRepository.GetByIdAsync(new Domain.Users.ValueObjects.UserId(Guid.Parse(dispute.RaisedById)), cancellationToken);
            var evidenceCount = await fileMetadataRepository.CountByLinkedEntityAsync(dispute.Id, cancellationToken);

            var status = Enum.Parse<DisputeStatus>(dispute.Status, ignoreCase: true);
            var deadline = stateMachine.GetDeadline(status, dispute.UpdatedAt ?? dispute.CreatedAt);
            var isDeadlineSoon = deadline.HasValue && 
                stateMachine.IsDeadlineSoon(status, dispute.UpdatedAt ?? dispute.CreatedAt, TimeSpan.FromHours(12));

            disputeDtos.Add(new SellerDisputeDto(
                dispute.Id,
                dispute.ListingId,
                listing.Title,
                dispute.RaisedById,
                buyer?.FullName ?? "Unknown",
                dispute.Reason,
                dispute.Status,
                dispute.CreatedAt,
                dispute.UpdatedAt ?? dispute.CreatedAt,
                deadline,
                isDeadlineSoon,
                evidenceCount
            ));
        }

        // Apply deadline filter if requested
        if (request.Filter.DeadlineSoon == true)
        {
            disputeDtos = disputeDtos.Where(d => d.IsDeadlineSoon).ToList();
        }

        return Result.Success(new PagedResult<SellerDisputeDto>(
            disputeDtos,
            request.Filter.Page,
            request.Filter.PageSize,
            totalCount
        ));
    }
}