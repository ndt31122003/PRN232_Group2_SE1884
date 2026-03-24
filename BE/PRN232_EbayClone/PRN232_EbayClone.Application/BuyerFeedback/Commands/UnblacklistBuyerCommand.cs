using MediatR;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.BuyerFeedback.Commands;

public record UnblacklistBuyerCommand(string BuyerId) : ICommand;

internal sealed class UnblacklistBuyerCommandHandler(
    ISellerBlacklistRepository sellerBlacklistRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : ICommandHandler<UnblacklistBuyerCommand>
{
    public async Task<Result> Handle(UnblacklistBuyerCommand request, CancellationToken cancellationToken)
    {
        // Validate seller is authenticated
        if (string.IsNullOrEmpty(userContext.UserId))
        {
            return Result.Failure(Error.Unauthorized("Seller must be authenticated"));
        }

        // Find active blacklist entry
        var blacklist = await sellerBlacklistRepository.GetBySellerAndBuyerAsync(
            userContext.UserId, request.BuyerId, cancellationToken);

        if (blacklist == null || !blacklist.IsActive)
        {
            return Result.Failure(Error.NotFound("Blacklist.NotFound", "Buyer is not blacklisted"));
        }

        // Deactivate blacklist
        blacklist.Deactivate();
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}