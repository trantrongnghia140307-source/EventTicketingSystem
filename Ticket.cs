using System;

namespace EventTicketing.Domain;

public class Ticket
{
    public string TicketCode { get; private set; }
    public TicketTier Tier { get; private set; }

    public Ticket(TicketTier tier)
    {
        TicketCode = Guid.NewGuid().ToString("N")[..8].ToUpper();
        Tier = tier;
    }
}
