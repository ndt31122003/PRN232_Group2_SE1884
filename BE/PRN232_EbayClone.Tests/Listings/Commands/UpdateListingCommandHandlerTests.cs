using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Application.SaleEvents.Services;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class UpdateListingCommandHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly IPriceIncreaseValidator _priceIncreaseValidator = Substitute.For<IPriceIncreaseValidator>();
    private readonly UpdateListingCommandHandler _handler;
    private readonly Category _category;

    public UpdateListingCommandHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _priceIncreaseValidator.ValidatePriceChange(Arg.Any<Guid>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(true)));
        _handler = new UpdateListingCommandHandler(_unitOfWork, _categoryRepository, _listingRepository, _userContext, _priceIncreaseValidator);
        _category = ListingTestData.CreateLeafCategory(
            ListingTestData.CreateSpecific("Color", required: true),
            ListingTestData.CreateSpecific("Size"));

        _categoryRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(_category);
    }

    [Fact]
    public async Task Handle_ShouldUpdateFixedPriceSingle()
    {
        var listing = ListingTestData.CreateDraftSingleListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var command = new UpdateListingCommand(
            listing.Id,
            ListingFormat.FixedPrice,
            ListingType.Single,
            "New title",
            "SKU-NEW",
            "New desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            new[] { new ListingImage("http://img", true) },
            50m,
            10,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            true,
            10m,
            15m,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true,
            null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Update(listing);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldUpdateFixedPriceMultiVariation()
    {
        var listing = ListingTestData.CreateDraftMultiVariationListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var command = new UpdateListingCommand(
            listing.Id,
            ListingFormat.FixedPrice,
            ListingType.MultiVariation,
            "New title",
            "SKU-NEW",
            "New desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            null,
            null,
            null,
            new[]
            {
                new VariationDto("SKU-1", 15m, 3, new[] { new VariationSpecific("Color", new[] { "Red" }) }, null),
                new VariationDto("SKU-2", 20m, 5, new[] { new VariationSpecific("Color", new[] { "Blue" }) }, null)
            },
            null,
            null,
            null,
            Duration.Gtc,
            false,
            null,
            null,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            false,
            DateTime.UtcNow.AddHours(2));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Update(listing);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        listing.Status.Should().Be(ListingStatus.Scheduled);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAuctionAndActivate()
    {
        var listing = ListingTestData.CreateDraftAuctionListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var command = new UpdateListingCommand(
            listing.Id,
            ListingFormat.Auction,
            null,
            "Title",
            "SKU",
            "Desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            new[] { new ListingImage("http://img", true) },
            null,
            null,
            null,
            30m,
            40m,
            50m,
            Duration.TenDays,
            false,
            null,
            null,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            false,
            null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        listing.Status.Should().Be(ListingStatus.Active);
        _listingRepository.Received(1).Update(listing);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenListingNotFound()
    {
        _listingRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Listing?)null);

        var command = new UpdateListingCommand(
            Guid.NewGuid(),
            ListingFormat.FixedPrice,
            ListingType.Single,
            "Title",
            "SKU",
            "Desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            Array.Empty<ItemSpecific>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            false,
            null,
            null,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true,
            null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ListingErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryNotFound()
    {
        var listing = ListingTestData.CreateDraftSingleListing();
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);
        _categoryRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Category?)null);

        var command = new UpdateListingCommand(
            listing.Id,
            ListingFormat.FixedPrice,
            ListingType.Single,
            "Title",
            "SKU",
            "Desc",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            null,
            10m,
            1,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            true,
            null,
            null,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true,
            null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound);
    }
}
