using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Reviews.Errors;
using PRN232_EbayClone.Domain.Reviews.Events;
using Microsoft.EntityFrameworkCore;

namespace PRN232_EbayClone.Application.Reviews.Commands;

public sealed record ReplyToReviewCommand(
    Guid ReviewId,
    string Reply
) : ICommand;

public sealed class ReplyToReviewCommandValidator : AbstractValidator<ReplyToReviewCommand>
{
    public ReplyToReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID is required");
        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Reply cannot be empty")
            .MaximumLength(500).WithMessage("Reply cannot exceed 500 characters");
    }
}

public sealed class ReplyToReviewCommandHandler : ICommandHandler<ReplyToReviewCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ReplyToReviewCommandHandler(
        IApplicationDbContext context,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReplyToReviewCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return ReviewErrors.Unauthorized;
        }

        // Get the review (BuyerFeedback)
        var review = await _context.BuyerFeedbacks
            .FirstOrDefaultAsync(bf => bf.Id == request.ReviewId, cancellationToken);

        if (review is null)
        {
            return ReviewErrors.NotFound;
        }

        // Verify seller owns the review
        if (review.SellerId != sellerId)
        {
            return ReviewErrors.Unauthorized;
        }

        // Check if reply already exists
        var existingReply = await _context.SellerReviewReplies
            .FirstOrDefaultAsync(srr => srr.ReviewId == request.ReviewId && !srr.IsDeleted, cancellationToken);

        if (existingReply != null)
        {
            // Edit existing reply
            var editResult = existingReply.Edit(request.Reply, DateTimeOffset.UtcNow, _userContext.UserId);
            if (editResult.IsFailure)
            {
                return editResult.Error;
            }

            _context.SellerReviewReplies.Update(existingReply);

            // TODO: Raise ReviewReplyEditedDomainEvent for notifications
        }
        else
        {
            // Create new reply
            var replyResult = SellerReviewReply.Create(
                request.ReviewId,
                sellerId,
                request.Reply,
                DateTimeOffset.UtcNow,
                _userContext.UserId);

            if (replyResult.IsFailure)
            {
                return replyResult.Error;
            }

            await _context.SellerReviewReplies.AddAsync(replyResult.Value, cancellationToken);

            // TODO: Raise ReviewRepliedDomainEvent for notifications
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
