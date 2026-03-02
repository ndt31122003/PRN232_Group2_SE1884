using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Listings.Common;

public sealed record BulkListingActionFailure(
    Guid ListingId,
    string Code,
    string Message);

public sealed record BulkDeleteListingsResult(
    int TotalRequested,
    int DeletedCount,
    IReadOnlyList<BulkListingActionFailure> Failures);

public sealed record ListingCopySuccess(
    Guid SourceListingId,
    Guid CopiedListingId);

public sealed record BulkCopyListingsResult(
    int TotalRequested,
    int CopiedCount,
    IReadOnlyList<ListingCopySuccess> Copies,
    IReadOnlyList<BulkListingActionFailure> Failures);
