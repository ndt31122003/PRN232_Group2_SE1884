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

public sealed class GetScheduledListingsQueryHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly GetScheduledListingsQueryHandler _handler;

    public GetScheduledListingsQueryHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new GetScheduledListingsQueryHandler(_listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagingResult()
    {
        var items = new List<ScheduledListingDto>
        {
            new(
                Guid.NewGuid(),
                "Title",
                "thumb",
                "SKU",
                ListingFormat.FixedPrice,
                5,
                Duration.Gtc,
                12m,
                10m,
                0m,
                2m,
                DateTime.UtcNow.AddHours(3))
        };

        _listingRepository.GetScheduledListingsAsync("user-123", null, 1, 20, Arg.Any<CancellationToken>())
            .Returns((items, items.Count));

        var result = await _handler.Handle(new GetScheduledListingsQuery(PageNumber: 1, PageSize: 20), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo(items);
        result.Value.TotalCount.Should().Be(items.Count);
    }

    [Fact]
    public async Task Handle_ShouldClampPagingValues()
    {
        _listingRepository.GetScheduledListingsAsync("user-123", null, 1, 200, Arg.Any<CancellationToken>())
            .Returns((new List<ScheduledListingDto>(), 0));

        await _handler.Handle(new GetScheduledListingsQuery(PageNumber: 0, PageSize: 1000), CancellationToken.None);

        await _listingRepository.Received(1)
            .GetScheduledListingsAsync("user-123", null, 1, 200, Arg.Any<CancellationToken>());
    }
}
