using MediatR;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.BuyerFeedback.Commands;

public record BlacklistBuyerCommand(
    string BuyerId,
    string? Reason
) : ICommand<Guid>;

internal sealed class BlacklistBuyerCommandHandler(
    ISellerBlacklistRepository sellerBlacklistRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : ICommandHandler<BlacklistBuyerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(BlacklistBuyerCommand request, CancellationToken cancellationToken)
    {
        // Validate seller is authenticated
        if (string.IsNullOrEmpty(userContext.UserId))
        {
            return Result.Failure<Guid>(Error.Unauthorized("Seller must be authenticated"));
        }

        // Validate buyer exists
        var buyerId = new Domain.Users.ValueObjects.UserId(Guid.Parse(request.BuyerId));
        var buyer = await userRepository.GetByIdAsync(buyerId, cancellationToken);
        if (buyer == null)
        {
            return Result.Failure<Guid>(Error.NotFound("Blacklist.BuyerNotFound", "Buyer not found"));
        }

        // Validate seller cannot blacklist themselves
        if (userContext.UserId == request.BuyerId)
        {
            return Result.Failure<Guid>(Error.Validation("Blacklist.SelfBlacklist", "Cannot blacklist yourself"));
        }

        // Check if already blacklisted
        var existingBlacklist = await sellerBlacklistRepository.GetBySellerAndBuyerAsync(
            userContext.UserId, request.BuyerId, cancellationToken);

        if (existingBlacklist != null)
        {
            if (existingBlacklist.IsActive)
            {
                return Result.Failure<Guid>(Error.Validation("Blacklist.AlreadyBlacklisted", "Buyer is already blacklisted"));
            }
            
            // Reactivate existing blacklist
            existingBlacklist.Reactivate(request.Reason ?? "Manual blacklist by seller");
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(existingBlacklist.Id);
        }

        // Create new blacklist entry
        var blacklistResult = Domain.BuyerFeedback.Entities.SellerBlacklist.Create(
            userContext.UserId,
            request.BuyerId,
            request.Reason ?? "Manual blacklist by seller",
            false // not auto-generated
        );

        if (blacklistResult.IsFailure)
        {
            return Result.Failure<Guid>(blacklistResult.Error);
        }

        sellerBlacklistRepository.Add(blacklistResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(blacklistResult.Value.Id);
    }
}