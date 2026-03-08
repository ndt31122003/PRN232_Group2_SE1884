using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid? Orderid { get; set; }

    public Guid? Userid { get; set; }

    public decimal? Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public DateTime? Paidat { get; set; }

    public virtual Ordertable? Order { get; set; }

    public virtual User1? User { get; set; }
}
