using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Errors;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class RemoveListingCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly RemoveListingCommandHandler _handler;

    public RemoveListingCommandHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new RemoveListingCommandHandler(_unitOfWork, _listingRepository, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldRemoveListing_WhenExists()
    {
        var listing = ListingTestData.CreateDraftSingleListing();
        listing.CreatedBy = "user-123";
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var result = await _handler.Handle(new RemoveListingCommand(listing.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(1).Remove(listing);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenListingMissing()
    {
        var listingId = Guid.NewGuid();
        _listingRepository.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
            .Returns((Listing?)null);

        var result = await _handler.Handle(new RemoveListingCommand(listingId), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ListingErrors.NotFound);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
