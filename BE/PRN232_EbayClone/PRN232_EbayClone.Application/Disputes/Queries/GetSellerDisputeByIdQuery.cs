using Dapper;
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
    IDbConnectionFactory dbConnectionFactory,
    IDisputeStateMachine stateMachine)
    : IQueryHandler<GetSellerDisputeByIdQuery, SellerDisputeDetailDto>
{
    public async Task<Result<SellerDisputeDetailDto>> Handle(
        GetSellerDisputeByIdQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = await dbConnectionFactory.OpenConnectionAsync();

        var disputeSql = @"
            SELECT 
                d.id,
                d.listing_id,
                l.title as listing_title,
                d.raised_by_id,
                u.full_name as buyer_name,
                d.reason,
                d.status,
                d.created_at,
                d.updated_at
            FROM disputes d
            INNER JOIN listings l ON d.listing_id = l.id
            INNER JOIN users u ON d.raised_by_id = u.id
            WHERE d.id = @DisputeId AND l.seller_id = @SellerId";

        var dispute = await connection.QuerySingleOrDefaultAsync<dynamic>(disputeSql, new
        {
            DisputeId = request.DisputeId,
            SellerId = request.SellerId
        });

        if (dispute == null)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute hoặc bạn không có quyền truy cập");
        }

        var evidenceSql = @"
            SELECT 
                fm.id,
                fm.file_name,
                fm.content_type,
                fm.size,
                fm.url,
                fm.created_at as uploaded_at
            FROM file_metadata fm
            WHERE fm.linked_entity_id = @DisputeId
            ORDER BY fm.created_at DESC";

        var evidence = await connection.QueryAsync<dynamic>(evidenceSql, new
        {
            DisputeId = request.DisputeId
        });

        var status = Enum.Parse<DisputeStatus>(dispute.status, ignoreCase: true);
        var deadline = stateMachine.GetDeadline(status, dispute.updated_at ?? dispute.created_at);
        var isDeadlineSoon = deadline.HasValue && 
            stateMachine.IsDeadlineSoon(status, dispute.updated_at ?? dispute.created_at, TimeSpan.FromHours(12));

        var evidenceDtos = evidence.Select(e => new DisputeEvidenceDto(
            e.id,
            e.file_name,
            e.content_type,
            e.size,
            e.url,
            e.uploaded_at
        )).ToList();

        var result = new SellerDisputeDetailDto(
            dispute.id,
            dispute.listing_id,
            dispute.listing_title,
            dispute.raised_by_id,
            dispute.buyer_name,
            dispute.reason,
            dispute.status,
            dispute.created_at,
            dispute.updated_at ?? dispute.created_at,
            deadline,
            isDeadlineSoon,
            evidenceDtos
        );

        return Result.Success(result);
    }
}