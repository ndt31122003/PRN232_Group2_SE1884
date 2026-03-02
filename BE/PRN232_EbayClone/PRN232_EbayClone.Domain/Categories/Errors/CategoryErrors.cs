using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Categories.Errors;

public static class CategoryErrors
{
    public static Error NotFound => Error.Failure(
        "Category.NotFound",
        "The category was not found.");

    public static Error NotLeaf => Error.Failure(
        "Category.NotLeaf",
        "The category is not a leaf category and cannot be used for listings.");
}
