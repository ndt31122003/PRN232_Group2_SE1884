using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Reviews.Errors;

namespace PRN232_EbayClone.Application.Reviews.Commands;

public sealed record ReplyToReviewCommand(
    Guid ReviewId,
    string Reply
) : ICommand;

public sealed class ReplyToReviewCommandValidator : AbstractValidator<ReplyToReviewCommand>
{
    public ReplyToReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID là bắt buộc");
        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Phản hồi không được để trống")
            .MaximumLength(2000).WithMessage("Phản hồi không được vượt quá 2000 ký tự");
    }
}

public sealed class ReplyToReviewCommandHandler : ICommandHandler<ReplyToReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ReplyToReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReplyToReviewCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return ReviewErrors.Unauthorized;
        }

        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);
        if (review is null)
        {
            return ReviewErrors.NotFound;
        }

        // Verify seller owns the listing
        if (review.Listing is null)
        {
            return ReviewErrors.ListingNotFound;
        }

        // Verify seller owns the listing through CreatedBy
        if (review.Listing.CreatedBy != _userContext.UserId)
        {
            return ReviewErrors.Unauthorized;
        }

        var result = review.AddReply(request.Reply, sellerId);
        if (result.IsFailure)
        {
            return result.Error;
        }

        _reviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
