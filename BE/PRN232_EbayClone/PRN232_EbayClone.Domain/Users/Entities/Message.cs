using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid? Senderid { get; set; }

    public Guid? Receiverid { get; set; }

    public string? Content { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User1? Receiver { get; set; }

    public virtual User1? Sender { get; set; }
}
