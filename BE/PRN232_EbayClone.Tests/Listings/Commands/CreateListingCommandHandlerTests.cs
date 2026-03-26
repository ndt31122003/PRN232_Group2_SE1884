using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Storage;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.Services;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class CreateListingCommandHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IInventoryRepository _inventoryRepository = Substitute.For<IInventoryRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICloudinaryService _cloudinaryService = Substitute.For<ICloudinaryService>();
    private readonly CreateListingCommandCommandHandler _handler;

    private readonly Category _category;
    private readonly User _seller;

    public CreateListingCommandHandlerTests()
    {
        var sellerId = Guid.NewGuid();
        _userContext.UserId.Returns(sellerId.ToString());
        _handler = new CreateListingCommandCommandHandler(
            _listingRepository,
            _inventoryRepository,
            _unitOfWork,
            _categoryRepository,
            _userContext,
            _userRepository,
            _cloudinaryService);

        _seller = CreateVerifiedSeller().GetAwaiter().GetResult();

        _category = ListingTestData.CreateLeafCategory(
            ListingTestData.CreateSpecific("Color", required: true, allowMultiple: true),
            ListingTestData.CreateSpecific("Size", required: false));

        _categoryRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(_category);
            _userRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(_seller);
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
            null, // ListingImages
            new[] { "base64_string_here" }, // Base64Images
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
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        _inventoryRepository.Received(1).Add(Arg.Any<Domain.Listings.Inventory.Entities.Inventory>());
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
            null, // ListingImages
            null, // Base64Images
            null, // Price
            null, // Quantity
            new[]
            {
                new CreateVariationDto("SKU-1", 10m, 1, new[] { new VariationSpecific("Color", new[] { "Red" }) }, null, null),
                new CreateVariationDto("SKU-2", 12m, 2, new[] { new VariationSpecific("Color", new[] { "Blue" }) }, null, null)
            },
            null,
            null,
            null,
            Duration.Gtc,
            DateTime.UtcNow.AddHours(3),
            false,
            null,
            null,
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        _inventoryRepository.Received(1).Add(Arg.Any<Domain.Listings.Inventory.Entities.Inventory>());
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
            null, // ListingImages
            new[] { "base64_string_here" }, // Base64Images
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
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Add(Arg.Any<Domain.Listings.Entities.Listing>());
        _inventoryRepository.Received(1).Add(Arg.Any<Domain.Listings.Inventory.Entities.Inventory>());
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
            _category.Id,
            Guid.NewGuid(),
            "Cond",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            null, // ListingImages
            null, // Base64Images
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
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound);
        _listingRepository.DidNotReceive().Add(Arg.Any<Domain.Listings.Entities.Listing>());
        _inventoryRepository.DidNotReceive().Add(Arg.Any<Domain.Listings.Inventory.Entities.Inventory>());
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
            null, // ListingImages
            null, // Base64Images
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
            null,  // ShippingPolicyId
            null,  // ReturnPolicyId
            true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ItemSpecific.Required");
        _listingRepository.DidNotReceive().Add(Arg.Any<Domain.Listings.Entities.Listing>());
    }

    private static async Task<User> CreateVerifiedSeller()
    {
        var emailChecker = Substitute.For<IEmailUniquenessChecker>();
        emailChecker.IsUniqueEmail(Arg.Any<PRN232_EbayClone.Domain.Shared.ValueObjects.Email>()).Returns(true);

        var userResult = await User.CreateAsync(
            emailChecker,
            "Seller Test",
            "seller@example.com",
            "hashed-password",
            []);

        var user = userResult.Value;
        user.VerifyEmail();
        user.SetPhoneNumber("0123456789");
        user.VerifyPhone();
        user.SetBusinessInfo(
            "Seller Business",
            BusinessAddress.Create("Street 1", "City", "State", "70000", "VN").Value);

        return user;
    }
}
