using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Dispute
{
    public int Id { get; set; }

    public int? Orderid { get; set; }

    public int? Raisedby { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Resolution { get; set; }

    public virtual Ordertable? Order { get; set; }

    public virtual User1? RaisedbyNavigation { get; set; }
}
