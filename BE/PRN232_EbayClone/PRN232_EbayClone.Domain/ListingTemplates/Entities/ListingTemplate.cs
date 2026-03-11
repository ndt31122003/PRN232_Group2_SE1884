using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.ListingTemplates.Entities;

public sealed class ListingTemplate : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string PayloadJson { get; private set; }
    public string? FormatLabel { get; private set; }
    public string? ThumbnailUrl { get; private set; }

    private ListingTemplate(Guid id, string name, string payloadJson) : base(id)
    {
        Name = name;
        PayloadJson = payloadJson;
    }

    public static Result<ListingTemplate> Create(string name, string payloadJson, string? description = null, string? formatLabel = null, string? thumbnailUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Error.Failure("ListingTemplate.InvalidName", "Template name is required.");
        }

        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return Error.Failure("ListingTemplate.EmptyPayload", "Template payload must not be empty.");
        }

        var template = new ListingTemplate(Guid.NewGuid(), name.Trim(), payloadJson)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            FormatLabel = string.IsNullOrWhiteSpace(formatLabel) ? null : formatLabel.Trim(),
            ThumbnailUrl = string.IsNullOrWhiteSpace(thumbnailUrl) ? null : thumbnailUrl.Trim()
        };

        return template;
    }

    public Result Update(string name, string payloadJson, string? description, string? formatLabel, string? thumbnailUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Error.Failure("ListingTemplate.InvalidName", "Template name is required.");
        }

        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return Error.Failure("ListingTemplate.EmptyPayload", "Template payload must not be empty.");
        }

        Name = name.Trim();
        PayloadJson = payloadJson;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        FormatLabel = string.IsNullOrWhiteSpace(formatLabel) ? null : formatLabel.Trim();
        ThumbnailUrl = string.IsNullOrWhiteSpace(thumbnailUrl) ? null : thumbnailUrl.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public ListingTemplate Clone(string name)
    {
        var clone = new ListingTemplate(Guid.NewGuid(), name.Trim(), PayloadJson)
        {
            Description = Description,
            FormatLabel = FormatLabel,
            ThumbnailUrl = ThumbnailUrl
        };

        return clone;
    }
}
