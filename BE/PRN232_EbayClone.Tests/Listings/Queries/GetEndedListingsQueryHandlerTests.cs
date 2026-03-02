using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Domain.Listings.Enums;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Queries;

public sealed class GetEndedListingsQueryHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly GetEndedListingsQueryHandler _handler;

    public GetEndedListingsQueryHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new GetEndedListingsQueryHandler(_listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagingResult()
    {
        var items = new List<EndedListingDto>
        {
            new(
                Guid.NewGuid(),
                "Title",
                "thumb",
                "SKU",
                ListingFormat.FixedPrice,
                1,
                Duration.ThreeDays,
                SoldStatus.Sold,
                RelistStatus.NotRelisted,
                10m,
                9m,
                8m,
                DateTime.UtcNow.AddDays(-7),
                DateTime.UtcNow.AddDays(-1))
        };

        _listingRepository.GetEndedListingsAsync("user-123", null, null, null, null, 1, 20, Arg.Any<CancellationToken>())
            .Returns((items, items.Count));

        var result = await _handler.Handle(new GetEndedListingsQuery(PageNumber: 1, PageSize: 20), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo(items);
        result.Value.TotalCount.Should().Be(items.Count);
    }

    [Fact]
    public async Task Handle_ShouldClampPagingValues()
    {
        _listingRepository.GetEndedListingsAsync("user-123", null, null, null, null, 1, 200, Arg.Any<CancellationToken>())
            .Returns((new List<EndedListingDto>(), 0));

        await _handler.Handle(new GetEndedListingsQuery(PageNumber: -10, PageSize: 900), CancellationToken.None);

        await _listingRepository.Received(1)
            .GetEndedListingsAsync("user-123", null, null, null, null, 1, 200, Arg.Any<CancellationToken>());
    }
}
