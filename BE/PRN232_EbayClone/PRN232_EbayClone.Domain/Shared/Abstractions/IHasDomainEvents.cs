namespace PRN232_EbayClone.Domain.Shared.Abstractions;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
