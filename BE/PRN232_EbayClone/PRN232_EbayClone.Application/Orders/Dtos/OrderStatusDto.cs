using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderStatusDto(
    Guid Id,
    string StatusCode,
    string StatusName,
    string StatusColor
);
