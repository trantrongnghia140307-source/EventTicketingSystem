using System.Collections.Generic;

namespace EventTicketing.Domain;

// Event được tạo thành từ nhiều TicketTier -> Composition.
public class Event
{
    public string EventId { get; private set; }
    public string EventName { get; private set; }
    public List<TicketTier> Tiers { get; private set; }

    public Event(string eventId, string eventName)
    {
        EventId = eventId;
        EventName = eventName;
        Tiers = new List<TicketTier>();
    }

    public void AddTier(TicketTier tier) => Tiers.Add(tier);
}
