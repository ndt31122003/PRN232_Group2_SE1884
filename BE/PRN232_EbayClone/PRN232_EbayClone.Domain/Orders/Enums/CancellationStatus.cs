using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Domain.Orders.Enums;

public enum CancellationStatus
{
    PendingSellerResponse = 0,
    PendingBuyerConfirmation = 1,
    AwaitingRefund = 2,
    Completed = 3,
    Declined = 4,
    AutoCancelled = 5
}
