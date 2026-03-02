using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Domain.Orders.Enums;

public enum ReturnStatus
{
    PendingSellerResponse = 0,
    AwaitingBuyerReturn = 1,
    InTransitBackToSeller = 2,
    DeliveredToSeller = 3,
    RefundPending = 4,
    RefundCompleted = 5,
    ReplacementSent = 6,
    Closed = 7,
    SellerDeclined = 8
}
