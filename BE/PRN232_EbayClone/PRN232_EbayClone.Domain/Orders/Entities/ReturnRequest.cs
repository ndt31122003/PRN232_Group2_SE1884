using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Returnrequest
{
    public int Id { get; set; }

    public int? Orderid { get; set; }

    public int? Userid { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Ordertable? Order { get; set; }

    public virtual User1? User { get; set; }
}
