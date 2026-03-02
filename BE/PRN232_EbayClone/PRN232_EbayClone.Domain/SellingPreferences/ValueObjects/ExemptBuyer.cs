using System;

namespace PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;

public sealed class ExemptBuyer
{
    private ExemptBuyer() { }

    private ExemptBuyer(Guid id, string identifier, string normalizedIdentifier, DateTime createdAtUtc)
    {
        Id = id;
        Identifier = identifier;
        NormalizedIdentifier = normalizedIdentifier;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }
    public string Identifier { get; private set; } = string.Empty;
    public string NormalizedIdentifier { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    public static ExemptBuyer Create(string identifier)
    {
        var trimmed = identifier.Trim();
        var normalized = trimmed.ToUpperInvariant();
        return new ExemptBuyer(Guid.NewGuid(), trimmed, normalized, DateTime.UtcNow);
    }
}
