using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Shippinginfo
{
    public int Id { get; set; }

    public int? Orderid { get; set; }

    public string? Carrier { get; set; }

    public string? Trackingnumber { get; set; }

    public string? Status { get; set; }

    public DateTime? Estimatedarrival { get; set; }

    public virtual Ordertable? Order { get; set; }
}
