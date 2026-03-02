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
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class CreateListingCommandHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateListingCommandCommandHandler _handler;

    private readonly Category _category;

    public CreateListingCommandHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new CreateListingCommandCommandHandler(
            _listingRepository,
            _unitOfWork,
            _categoryRepository,
            _userContext,
            _userRepository);

        _category = ListingTestData.CreateLeafCategory(
            ListingTestData.CreateSpecific("Color", required: true, allowMultiple: true),
            ListingTestData.CreateSpecific("Size", required: false));

        _categoryRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(_category);
    }

    [Fact]
    public async Task Handle_ShouldCreateFixedPriceSingleDraft()
    {
        var command = new CreateListingCommand(
            ListingFormat.FixedPrice,
            ListingType.Single,
            "Title",
            "SKU",
            "Desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            new[] { new ListingImage("http://image", true) },
            20m,
            5,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            null,
            true,
            10m,
            15m,
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateMultiVariationListing()
    {
        var command = new CreateListingCommand(
            ListingFormat.FixedPrice,
            ListingType.MultiVariation,
            "Title",
            "SKU",
            "Desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            null,
            null,
            null,
            new[]
            {
                new VariationDto("SKU-1", 10m, 1, new[] { new VariationSpecific("Color", new[] { "Red" }) }, null),
                new VariationDto("SKU-2", 12m, 2, new[] { new VariationSpecific("Color", new[] { "Blue" }) }, null)
            },
            null,
            null,
            null,
            Duration.Gtc,
            DateTime.UtcNow.AddHours(3),
            false,
            null,
            null,
            false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateAuctionListingAndActivate()
    {
        var command = new CreateListingCommand(
            ListingFormat.Auction,
            null,
            "Title",
            "SKU",
            "Desc",
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            new[] { new ListingImage("http://image", true) },
            null,
            null,
            null,
            10m,
            15m,
            20m,
            Duration.SevenDays,
            null,
            false,
            null,
            null,
            false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryNotFound()
    {
        _categoryRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Category?)null);

        var command = new CreateListingCommand(
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
            20m,
            5,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            null,
            false,
            null,
            null,
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound);
        _listingRepository.DidNotReceive().Add(Arg.Any<Domain.Listings.Entities.Listing>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSpecificValidationFails()
    {
        var command = new CreateListingCommand(
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
            20m,
            5,
            null,
            null,
            null,
            null,
            Duration.Gtc,
            null,
            false,
            null,
            null,
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ItemSpecific.Required");
        _listingRepository.DidNotReceive().Add(Arg.Any<Domain.Listings.Entities.Listing>());
    }
}
