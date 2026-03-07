using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Address
{
    public Guid Id { get; set; }

    public Guid? Userid { get; set; }

    public string? Fullname { get; set; }

    public string? Phone { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public bool? Isdefault { get; set; }

    public virtual ICollection<Ordertable> Ordertables { get; set; } = new List<Ordertable>();

    public virtual User1? User { get; set; }
}
