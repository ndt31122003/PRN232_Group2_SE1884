using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Conditions.Entities;

public sealed class Condition(Guid id) : AggregateRoot<Guid>(id)
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
}