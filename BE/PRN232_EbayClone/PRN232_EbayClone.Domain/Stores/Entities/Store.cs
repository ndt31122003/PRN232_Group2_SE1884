using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Store
{
    public Guid Id { get; set; }

    public Guid? Sellerid { get; set; }

    public string? Storename { get; set; }

    public string? Description { get; set; }

    public string? Bannerimageurl { get; set; }

    public virtual User1? Seller { get; set; }
}
