using PRN232_EbayClone.Domain.Orders.Entities;
using System;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IReturnRequestRepository : IRepository<ReturnRequest, Guid>
{
}
