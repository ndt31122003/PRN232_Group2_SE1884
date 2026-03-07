using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Ordertable
{
    public Guid Id { get; set; }

    public Guid? Buyerid { get; set; }

    public Guid? Addressid { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Totalprice { get; set; }

    public string? Status { get; set; }

    public virtual Address? Address { get; set; }

    public virtual User1? Buyer { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
