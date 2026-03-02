using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Reviews.Errors;

public static class ReviewErrors
{
    public static readonly Error NotFound = Error.Failure(
        "Review.NotFound",
        "Review không tồn tại");

    public static readonly Error Unauthorized = Error.Failure(
        "Review.Unauthorized",
        "Bạn không có quyền thực hiện hành động này");

    public static readonly Error AlreadyReplied = Error.Failure(
        "Review.AlreadyReplied",
        "Review đã có phản hồi");

    public static readonly Error RevisionRequestExists = Error.Failure(
        "Review.RevisionRequestExists",
        "Yêu cầu sửa đổi review đã tồn tại");

    public static readonly Error RatingRequired = Error.Validation(
        "Review.RatingRequired",
        "Đánh giá là bắt buộc");

    public static readonly Error InvalidRating = Error.Validation(
        "Review.InvalidRating",
        "Đánh giá phải từ 1 đến 5");

    public static readonly Error ListingNotFound = Error.Failure(
        "Review.ListingNotFound",
        "Listing không tồn tại");
}
