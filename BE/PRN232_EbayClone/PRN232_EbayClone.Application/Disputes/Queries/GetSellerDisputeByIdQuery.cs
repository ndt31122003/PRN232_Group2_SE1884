using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetSellerDisputeByIdQuery(
    Guid DisputeId,
    string SellerId
) : IQuery<SellerDisputeDetailDto>;

internal sealed class GetSellerDisputeByIdQueryHandler(
    IDisputeRepository disputeRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
    IFileMetadataRepository fileMetadataRepository,
    IDisputeStateMachine stateMachine)
    : IQueryHandler<GetSellerDisputeByIdQuery, SellerDisputeDetailDto>
{
    public async Task<Result<SellerDisputeDetailDto>> Handle(
        GetSellerDisputeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dispute = await disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute == null)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute");
        }

        // Verify seller owns the listing
        var listing = await listingRepository.GetByIdAsync(dispute.ListingId, cancellationToken);
        if (listing == null || listing.CreatedBy != request.SellerId)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute hoặc bạn không có quyền truy cập");
        }

        var buyer = await userRepository.GetByIdAsync(new Domain.Users.ValueObjects.UserId(Guid.Parse(dispute.RaisedById)), cancellationToken);
        var evidence = await fileMetadataRepository.GetByLinkedEntityAsync(dispute.Id, cancellationToken);

        var status = Enum.Parse<DisputeStatus>(dispute.Status, ignoreCase: true);
        var deadline = stateMachine.GetDeadline(status, dispute.UpdatedAt ?? dispute.CreatedAt);
        var isDeadlineSoon = deadline.HasValue && 
            stateMachine.IsDeadlineSoon(status, dispute.UpdatedAt ?? dispute.CreatedAt, TimeSpan.FromHours(12));

        var evidenceDtos = evidence.Select(e => new DisputeEvidenceDto(
            e.Id,
            e.FileName,
            e.ContentType,
            e.Size,
            e.Url,
            e.CreatedAt
        )).ToList();

        var result = new SellerDisputeDetailDto(
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
            evidenceDtos
        );

        return Result.Success(result);
    }
}