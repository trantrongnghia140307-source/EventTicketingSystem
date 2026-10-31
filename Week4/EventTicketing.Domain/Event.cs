using System.Collections.Generic;

namespace EventTicketing.Domain;

public class Event
{
    public string EventId { get; private set; }
    public string EventName { get; private set; }
    public List<TicketTier> Tiers { get; private set; }
    public Dictionary<string, Ticket> IssuedTickets { get; private set; }

    public Event(string eventId, string eventName)
    {
        EventId = eventId;
        EventName = eventName;
        Tiers = new List<TicketTier>();
        IssuedTickets = new Dictionary<string, Ticket>();
    }

    public void AddTier(TicketTier tier) => Tiers.Add(tier);
}