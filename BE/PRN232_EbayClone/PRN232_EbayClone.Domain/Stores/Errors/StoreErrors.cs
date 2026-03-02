using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Stores.Errors;

public static class StoreErrors
{
    public static readonly Error NameRequired = Error.Validation(
        "Store.NameRequired",
        "Tên cửa hàng là bắt buộc");

    public static readonly Error NameTooLong = Error.Validation(
        "Store.NameTooLong",
        "Tên cửa hàng không được vượt quá 255 ký tự");

    public static readonly Error NotFound = Error.Failure(
        "Store.NotFound",
        "Cửa hàng không tồn tại");

    public static readonly Error AlreadyActive = Error.Validation(
        "Store.AlreadyActive",
        "Cửa hàng đã đang hoạt động");

    public static readonly Error AlreadyInactive = Error.Validation(
        "Store.AlreadyInactive",
        "Cửa hàng đã đang bị vô hiệu hóa");

    public static readonly Error Unauthorized = Error.Failure(
        "Store.Unauthorized",
        "Bạn không có quyền thực hiện hành động này");

    public static readonly Error DuplicateSlug = Error.Failure(
        "Store.DuplicateSlug",
        "Slug cửa hàng đã tồn tại");

    public static readonly Error SubscriptionAlreadyCancelled = Error.Validation(
        "Store.SubscriptionAlreadyCancelled",
        "Gói đăng ký đã bị hủy");
}

