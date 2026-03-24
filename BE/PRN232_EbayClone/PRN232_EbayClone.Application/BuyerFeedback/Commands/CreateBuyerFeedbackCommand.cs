using MediatR;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.BuyerFeedback.Commands;

public record CreateBuyerFeedbackCommand(
    string BuyerId,
    Guid? OrderId,
    Guid? ListingId,
    FeedbackType FeedbackType,
    FeedbackReason? Reason,
    string? Comment
) : ICommand<Guid>;

internal sealed class CreateBuyerFeedbackCommandHandler(
    IBuyerFeedbackRepository buyerFeedbackRepository,
    ISellerBlacklistRepository sellerBlacklistRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : ICommandHandler<CreateBuyerFeedbackCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBuyerFeedbackCommand request, CancellationToken cancellationToken)
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
            return Result.Failure<Guid>(Error.NotFound("BuyerFeedback.BuyerNotFound", "Buyer not found"));
        }

        // Validate seller cannot feedback themselves
        if (userContext.UserId == request.BuyerId)
        {
            return Result.Failure<Guid>(Error.Validation("BuyerFeedback.SelfFeedback", "Cannot create feedback for yourself"));
        }

        // Check if feedback already exists for this order
        if (request.OrderId.HasValue)
        {
            var existingFeedback = await buyerFeedbackRepository.GetByOrderAsync(
                userContext.UserId, request.BuyerId, request.OrderId.Value, cancellationToken);

            if (existingFeedback != null)
            {
                return Result.Failure<Guid>(Error.Validation("BuyerFeedback.DuplicateOrder", "Feedback already exists for this order"));
            }
        }

        // Validate negative feedback must have reason
        if (request.FeedbackType == FeedbackType.Negative && !request.Reason.HasValue)
        {
            return Result.Failure<Guid>(Error.Validation("BuyerFeedback.ReasonRequired", "Negative feedback must include a reason"));
        }

        // Create feedback
        var feedbackResult = Domain.BuyerFeedback.Entities.BuyerFeedbackEntity.Create(
            userContext.UserId,
            request.BuyerId,
            request.OrderId,
            request.ListingId,
            request.FeedbackType,
            request.Reason,
            request.Comment
        );

        if (feedbackResult.IsFailure)
        {
            return Result.Failure<Guid>(feedbackResult.Error);
        }

        buyerFeedbackRepository.Add(feedbackResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Check if auto-blacklist should be triggered
        await CheckAutoBlacklist(userContext.UserId, request.BuyerId, cancellationToken);

        return Result.Success(feedbackResult.Value.Id);
    }

    private async Task CheckAutoBlacklist(string sellerId, string buyerId, CancellationToken cancellationToken)
    {
        // Count negative feedbacks from this seller to this buyer
        var negativeCount = await buyerFeedbackRepository.CountNegativeFeedbacksAsync(
            sellerId, buyerId, cancellationToken);

        // Auto-blacklist after 3 negative feedbacks from same seller
        if (negativeCount >= 3)
        {
            var existingBlacklist = await sellerBlacklistRepository.GetBySellerAndBuyerAsync(
                sellerId, buyerId, cancellationToken);

            if (existingBlacklist == null || !existingBlacklist.IsActive)
            {
                var blacklistResult = Domain.BuyerFeedback.Entities.SellerBlacklist.Create(
                    sellerId,
                    buyerId,
                    "Auto-blacklisted after 3 negative feedbacks",
                    true // auto-generated
                );

                if (blacklistResult.IsSuccess)
                {
                    if (existingBlacklist != null)
                    {
                        existingBlacklist.Reactivate("Auto-blacklisted after 3 negative feedbacks");
                    }
                    else
                    {
                        sellerBlacklistRepository.Add(blacklistResult.Value);
                    }
                    
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}