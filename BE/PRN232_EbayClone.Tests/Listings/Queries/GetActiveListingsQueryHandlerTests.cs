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

public sealed class GetActiveListingsQueryHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly GetActiveListingsQueryHandler _handler;

    public GetActiveListingsQueryHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new GetActiveListingsQueryHandler(_listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagingResult()
    {
        var items = new List<ActiveListingDto>
        {
            new(
                Guid.NewGuid(),
                "Title",
                "thumb",
                "SKU",
                ListingFormat.FixedPrice,
                10,
                5,
                Duration.Gtc,
                12m,
                "10% off",
                10m,
                0m,
                3m,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7),
                0,    // WatchersCount
                0,    // BidsCount
                0,    // OffersCount
                null, // BestOfferAmount
                null) // BuyItNowPrice
        };

        _listingRepository.GetActiveListingsAsync("user-123", null, null, null, 1, 20, Arg.Any<CancellationToken>())
            .Returns((items, items.Count));

        var result = await _handler.Handle(new GetActiveListingsQuery(PageNumber: 1, PageSize: 20), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo(items);
        result.Value.TotalCount.Should().Be(items.Count);
    }

    [Fact]
    public async Task Handle_ShouldClampPagingValues()
    {
        _listingRepository.GetActiveListingsAsync("user-123", null, null, null, 1, 200, Arg.Any<CancellationToken>())
            .Returns((new List<ActiveListingDto>(), 0));

        await _handler.Handle(new GetActiveListingsQuery(PageNumber: 0, PageSize: 500), CancellationToken.None);

        await _listingRepository.Received(1)
            .GetActiveListingsAsync("user-123", null, null, null, 1, 200, Arg.Any<CancellationToken>());
    }
}
