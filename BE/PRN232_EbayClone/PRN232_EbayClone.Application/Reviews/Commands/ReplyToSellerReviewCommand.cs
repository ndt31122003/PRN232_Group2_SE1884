using PRN232_EbayClone.Application.Abstractions.Data;

namespace PRN232_EbayClone.Application.Reviews.Commands;

public sealed record ReplyToSellerReviewCommand(
    Guid ReviewId,
    Guid SellerId,
    string Reply
) : ICommand;

public sealed class ReplyToSellerReviewCommandValidator : AbstractValidator<ReplyToSellerReviewCommand>
{
    public ReplyToSellerReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID là bắt buộc");
        RuleFor(x => x.SellerId).NotEmpty().WithMessage("Seller ID là bắt buộc");
        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Reply không được để trống")
            .MaximumLength(2000).WithMessage("Reply không được vượt quá 2000 ký tự");
    }
}

public sealed class ReplyToSellerReviewCommandHandler : ICommandHandler<ReplyToSellerReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplyToSellerReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReplyToSellerReviewCommand request, CancellationToken cancellationToken)
    {
        // Validate review belongs to seller and can be replied to
        var review = await _reviewRepository.GetSellerReviewByIdAsync(
            request.ReviewId,
            request.SellerId,
            cancellationToken);

        if (review is null)
        {
            return Error.Failure("Review.NotFound", "Không tìm thấy review hoặc review không thuộc về seller này");
        }

        // Check if already replied (if we don't want to allow editing)
        if (!string.IsNullOrWhiteSpace(review.Reply))
        {
            return Error.Validation("Review.AlreadyReplied", "Review này đã được phản hồi");
        }

        // Add reply to review
        var result = review.AddReply(request.Reply, request.SellerId);
        if (result.IsFailure)
        {
            return result;
        }

        // Update review in repository
        _reviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}