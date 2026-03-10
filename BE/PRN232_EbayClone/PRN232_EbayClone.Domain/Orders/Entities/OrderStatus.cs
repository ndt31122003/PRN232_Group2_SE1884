using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public class OrderStatus
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public int SortOrder { get; private set; }

    private readonly List<OrderStatusTransition> _allowedTransitions = new();   
    public IReadOnlyCollection<OrderStatusTransition> AllowedTransitions => new ReadOnlyCollection<OrderStatusTransition>(_allowedTransitions);
    public OrderStatus(string code, string name, string description, string color, int sortOrder)
    {
        Code = code;
        Name = name;
        Description = description;
        Color = color;
        SortOrder = sortOrder;
    }
    public void AddTransition(OrderStatus targetStatus, params string[] allowedRoles)
    {
        if (_allowedTransitions.Any(t => t.ToStatus.Id == targetStatus.Id))
            throw new InvalidOperationException("Transition already exists.");

        var normalizedRoles = allowedRoles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        _allowedTransitions.Add(new OrderStatusTransition(this, targetStatus, normalizedRoles));
    }

    public bool CanTransitionTo(OrderStatus targetStatus, string role)
    {
        if (targetStatus is null || string.IsNullOrWhiteSpace(role))
        {
            return false;
        }

        return _allowedTransitions.Any(t =>
            t.ToStatus.Id == targetStatus.Id &&
            t.AllowedRoles.Any(allowedRole =>
                string.Equals(allowedRole, role, StringComparison.OrdinalIgnoreCase)));
    }

}
public class OrderStatusTransition
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public OrderStatus FromStatus { get; private set; } = null!;
    public OrderStatus ToStatus { get; private set; } = null!;
    public List<string> AllowedRoles { get; private set; } = new();

    private OrderStatusTransition() { }

    public OrderStatusTransition(OrderStatus fromStatus, OrderStatus toStatus, params string[] allowedRoles)
    {
        FromStatus = fromStatus;
        ToStatus = toStatus;
        AllowedRoles = allowedRoles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim().ToUpperInvariant())
            .Distinct()
            .ToList();
    }
}