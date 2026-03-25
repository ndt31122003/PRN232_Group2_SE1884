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
    IUserRepository userRepository,
    IFileMetadataRepository fileMetadataRepository,
    IDisputeStateMachine stateMachine)
    : IQueryHandler<GetSellerDisputesQuery, PagedResult<SellerDisputeDto>>
{
    public async Task<Result<PagedResult<SellerDisputeDto>>> Handle(
        GetSellerDisputesQuery request,
        CancellationToken cancellationToken)
    {
        // Use one paged query instead of loading all seller listings then looping per-listing
        // to avoid N+1 queries and request timeout/cancellation.
        var genericFilter = new DisputeFilterDto(
            ListingId: null,
            RaisedById: null,
            SellerId: request.SellerId,
            Status: request.Filter.Status,
            FromDate: null,
            ToDate: null,
            PageNumber: request.Filter.Page,
            PageSize: request.Filter.PageSize);

        var (disputes, totalCount) = await disputeRepository.GetDisputesAsync(
            genericFilter,
            request.SellerId,
            cancellationToken);

        var disputeDtos = new List<SellerDisputeDto>();

        foreach (var dispute in disputes)
        {
            var listing = dispute.Listing;
            if (listing == null)
            {
                continue;
            }

            var buyer = await userRepository.GetByIdAsync(new Domain.Users.ValueObjects.UserId(Guid.Parse(dispute.RaisedById)), cancellationToken);
            var evidenceCount = await fileMetadataRepository.CountByLinkedEntityAsync(dispute.Id, cancellationToken);

            var status = Enum.Parse<DisputeStatus>(dispute.Status, ignoreCase: true);
            var deadline = stateMachine.GetDeadline(status, dispute.UpdatedAt ?? dispute.CreatedAt);
            var isDeadlineSoon = deadline.HasValue && 
                stateMachine.IsDeadlineSoon(status, dispute.UpdatedAt ?? dispute.CreatedAt, TimeSpan.FromHours(12));
            var responses = dispute.Responses
                .OrderBy(r => r.CreatedAt)
                .Select(r => new SellerDisputeResponseDto(
                    r.Id,
                    r.ResponderId.ToString(),
                    r.Message,
                    r.CreatedAt))
                .ToList();

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
                evidenceCount,
                responses
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