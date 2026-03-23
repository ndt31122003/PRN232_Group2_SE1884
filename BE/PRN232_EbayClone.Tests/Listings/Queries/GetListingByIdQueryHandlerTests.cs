using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Queries;

public sealed class GetListingByIdQueryHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly GetListingByIdQueryHandler _handler;

    public GetListingByIdQueryHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new GetListingByIdQueryHandler(_listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnDetails_ForFixedPriceSingle()
    {
        var listing = ListingTestData.CreateDraftSingleListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var result = await _handler.Handle(new GetListingByIdQuery(listing.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.ListingId.Should().Be(listing.Id);
        result.Value.Type.Should().Be(listing.Type);
        result.Value.Price.Should().Be(listing.Pricing.Price);
        result.Value.Quantity.Should().Be(listing.Pricing.Quantity);
        result.Value.IsDraft.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnDetails_ForFixedPriceMultiVariation()
    {
        var listing = ListingTestData.CreateDraftMultiVariationListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var result = await _handler.Handle(new GetListingByIdQuery(listing.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be(listing.Type);
        result.Value.Variations.Should().NotBeNull();
        result.Value.Variations!.Count.Should().Be(listing.Variations.Count);
        result.Value.Price.Should().BeNull();
        result.Value.Quantity.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnDetails_ForAuction()
    {
        var listing = ListingTestData.CreateDraftAuctionListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var result = await _handler.Handle(new GetListingByIdQuery(listing.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.StartPrice.Should().Be(listing.Pricing.StartPrice);
        result.Value.ReservePrice.Should().Be(listing.Pricing.ReservePrice);
        result.Value.BuyItNowPrice.Should().Be(listing.Pricing.BuyItNowPrice);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenListingMissing()
    {
        _listingRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Listing?)null);

        var result = await _handler.Handle(new GetListingByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ListingErrors.NotFound);
    }
}
