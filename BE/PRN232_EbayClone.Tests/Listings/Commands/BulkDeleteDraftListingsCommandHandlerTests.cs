using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class BulkDeleteDraftListingsCommandHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly BulkDeleteDraftListingsCommandHandler _handler;

    public BulkDeleteDraftListingsCommandHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new BulkDeleteDraftListingsCommandHandler(_listingRepository, _unitOfWork, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAllDraftListings_WhenInputValid()
    {
        var listings = Enumerable.Range(0, 3)
            .Select(_ => ListingTestData.CreateDraftSingleListing())
            .ToArray();
        foreach (var listing in listings)
        {
            listing.CreatedBy = "user-123";
        }
        var ids = listings.Select(l => l.Id).ToList();

        for (var i = 0; i < listings.Length; i++)
        {
            var listing = listings[i];
            _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
                .Returns(listing);
        }

        var command = new BulkDeleteDraftListingsCommand(ids);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        foreach (var listing in listings)
        {
            _listingRepository.Received(1).Remove(listing);
        }

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSelectionEmpty()
    {
        var command = new BulkDeleteDraftListingsCommand(new());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContainsEmptyGuid()
    {
        var command = new BulkDeleteDraftListingsCommand(new() { Guid.Empty });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenListingNotFound()
    {
        var listingId = Guid.NewGuid();
        _listingRepository.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
            .Returns((Listing?)null);

        var command = new BulkDeleteDraftListingsCommand(new() { listingId });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ListingErrors.NotFound);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenListingNotDraft()
    {
        var listing = ListingTestData.CreateDraftSingleListing();
    listing.CreatedBy = "user-123";
        listing.Activate();
        _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
            .Returns(listing);

        var command = new BulkDeleteDraftListingsCommand(new() { listing.Id });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Listing.InvalidStatus");
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
