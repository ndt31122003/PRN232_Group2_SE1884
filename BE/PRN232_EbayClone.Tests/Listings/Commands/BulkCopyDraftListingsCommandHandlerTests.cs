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
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;
using Xunit;

namespace PRN232_EbayClone.Tests.Listings.Commands;

public sealed class BulkCopyDraftListingsCommandHandlerTests
{
    private readonly IListingRepository _listingRepository = Substitute.For<IListingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserContext _userContext = Substitute.For<IUserContext>();
    private readonly BulkCopyDraftListingsCommandHandler _handler;

    public BulkCopyDraftListingsCommandHandlerTests()
    {
        _userContext.UserId.Returns("user-123");
        _handler = new BulkCopyDraftListingsCommandHandler(_listingRepository, _unitOfWork, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldCopyAllListings_WhenAllValid()
    {
        var originals = Enumerable.Range(0, 2)
            .Select(_ => ListingTestData.CreateDraftSingleListing())
            .ToArray();
        foreach (var listing in originals)
        {
            listing.CreatedBy = "user-123";
        }
        var ids = originals.Select(l => l.Id).ToList();

        foreach (var listing in originals)
        {
            _listingRepository.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>())
                .Returns(listing);
        }

        var command = new BulkCopyDraftListingsCommand(ids);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _listingRepository.Received(ids.Count).Add(Arg.Any<Listing>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSelectionEmpty()
    {
        var command = new BulkCopyDraftListingsCommand(new());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContainsEmptyGuid()
    {
        var command = new BulkCopyDraftListingsCommand(new() { Guid.Empty });

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

        var command = new BulkCopyDraftListingsCommand(new() { listingId });

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

        var command = new BulkCopyDraftListingsCommand(new() { listing.Id });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Listing.InvalidStatus");
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
