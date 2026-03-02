using MediatR;

namespace PRN232_EbayClone.Domain.Shared.Abstractions;
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
