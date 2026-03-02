using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.ListingTemplates.Errors;

public static class ListingTemplateErrors
{
    public static readonly Error NotFound = Error.Failure("ListingTemplate.NotFound", "Listing template was not found.");
    public static readonly Error DuplicateName = Error.Failure("ListingTemplate.DuplicateName", "Template name is already in use.");
    public static Error MaximumLimitReached(int limit) =>
        Error.Failure("ListingTemplate.MaximumLimitReached", $"You can only keep {limit} templates. Delete an existing template before creating or cloning another.");
    public static readonly Error Unauthorized = Error.Failure("Unauthorized", "You are not authorized to access this listing template.");
}
