using System;

namespace EventTicketing.Domain;

public class Ticket
{
    public string TicketCode { get; private set; }
    public TicketTier Tier { get; private set; }
    public bool IsCheckedIn { get; private set; }
    public DateTime? CheckedInAt { get; private set; }

    public Ticket(TicketTier tier)
    {
        TicketCode = Guid.NewGuid().ToString("N")[..8].ToUpper();
        Tier = tier;
        IsCheckedIn = false;
    }

    public void MarkAsCheckedIn()
    {
        IsCheckedIn = true;
        CheckedInAt = DateTime.Now;
    }
}