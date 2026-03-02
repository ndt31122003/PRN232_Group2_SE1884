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

public sealed class GetDraftsQueryHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly GetDraftsQueryHandler _handler;

    public GetDraftsQueryHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new GetDraftsQueryHandler(_listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagingResult()
    {
        var items = new List<DraftListingDto>
        {
            new(
                Guid.NewGuid(),
                "Title",
                "thumb",
                "SKU",
                ListingFormat.FixedPrice,
                3,
                10m,
                15m,
                3m,
                Duration.Gtc,
                DateTime.UtcNow,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(2))
        };

        _listingRepository.GetDraftListingsAsync("user-123", null, 1, 20, Arg.Any<CancellationToken>())
            .Returns((items, items.Count));

        var result = await _handler.Handle(new GetDraftsQuery(PageNumber: 1, PageSize: 20), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo(items);
        result.Value.TotalCount.Should().Be(items.Count);
    }

    [Fact]
    public async Task Handle_ShouldClampPagingValues()
    {
        _listingRepository.GetDraftListingsAsync("user-123", null, 1, 200, Arg.Any<CancellationToken>())
            .Returns((new List<DraftListingDto>(), 0));

        await _handler.Handle(new GetDraftsQuery(PageNumber: -5, PageSize: 1000), CancellationToken.None);

        await _listingRepository.Received(1)
            .GetDraftListingsAsync("user-123", null, 1, 200, Arg.Any<CancellationToken>());
    }
}
