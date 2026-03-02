using PRN232_EbayClone.Domain.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Domain.Orders.Events;
public record CancellationApprovedEvent(Guid OrderId) : DomainEventBase;

