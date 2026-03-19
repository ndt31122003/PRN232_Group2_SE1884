using Dapper;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using System.Data;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetSellerDisputesQuery(
    string SellerId,
    SellerDisputeFilterDto Filter
) : IQuery<PagedResult<SellerDisputeDto>>;

internal sealed class GetSellerDisputesQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IDisputeStateMachine stateMachine)
    : IQueryHandler<GetSellerDisputesQuery, PagedResult<SellerDisputeDto>>
{
    public async Task<Result<PagedResult<SellerDisputeDto>>> Handle(
        GetSellerDisputesQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = await dbConnectionFactory.OpenConnectionAsync();

        var whereConditions = new List<string> { "l.seller_id = @SellerId" };
        var parameters = new { SellerId = request.SellerId };

        if (!string.IsNullOrEmpty(request.Filter.Status))
        {
            whereConditions.Add("d.status = @Status");
            parameters = new { parameters.SellerId, Status = request.Filter.Status };
        }

        var whereClause = string.Join(" AND ", whereConditions);
        var offset = (request.Filter.Page - 1) * request.Filter.PageSize;

        var sql = $@"
            SELECT 
                d.id,
                d.listing_id,
                l.title as listing_title,
                d.raised_by_id,
                u.full_name as buyer_name,
                d.reason,
                d.status,
                d.created_at,
                d.updated_at,
                COUNT(fm.id) as evidence_count
            FROM disputes d
            INNER JOIN listings l ON d.listing_id = l.id
            INNER JOIN users u ON d.raised_by_id = u.id
            LEFT JOIN file_metadata fm ON fm.linked_entity_id = d.id
            WHERE {whereClause}
            GROUP BY d.id, d.listing_id, l.title, d.raised_by_id, u.full_name, d.reason, d.status, d.created_at, d.updated_at
            ORDER BY d.created_at DESC
            LIMIT @PageSize OFFSET @Offset";

        var countSql = $@"
            SELECT COUNT(DISTINCT d.id)
            FROM disputes d
            INNER JOIN listings l ON d.listing_id = l.id
            WHERE {whereClause}";

        var disputes = await connection.QueryAsync<dynamic>(sql, new
        {
            parameters.SellerId,
            Status = request.Filter.Status,
            PageSize = request.Filter.PageSize,
            Offset = offset
        });

        var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

        var disputeDtos = disputes.Select(d =>
        {
            var status = Enum.Parse<DisputeStatus>(d.status, ignoreCase: true);
            var deadline = stateMachine.GetDeadline(status, d.updated_at ?? d.created_at);
            var isDeadlineSoon = deadline.HasValue && 
                stateMachine.IsDeadlineSoon(status, d.updated_at ?? d.created_at, TimeSpan.FromHours(12));

            return new SellerDisputeDto(
                d.id,
                d.listing_id,
                d.listing_title,
                d.raised_by_id,
                d.buyer_name,
                d.reason,
                d.status,
                d.created_at,
                d.updated_at ?? d.created_at,
                deadline,
                isDeadlineSoon,
                d.evidence_count
            );
        }).ToList();

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